<!--
 * @Author: your name
 * @Date: 2021-02-05 14:52:13
 * @LastEditTime: 2021-11-30 18:50:08
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \vue3-element-admin\src\views\layoutpages\system\Users.vue
-->
<template>
    <div class="ve_container">
        <!-- 搜索 -->
        <el-form ref="queryForm" :inline="true" :model="params">
            <el-form-item label="关键字" prop="meetingKeywords">
                <el-input
                    clearable
                    v-model="meetingKeywords"
                    placeholder="请输入关键字"
                ></el-input>
            </el-form-item>
            <el-form-item label="小组" prop="groupName">
                <el-select clearable v-model="groupName" placeholder="请选择">
                    <el-option
                        v-for="item in groupList"
                        :key="item.id"
                        :label="item.groupName"
                        :value="item.groupName"
                    ></el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="时间">
                <el-date-picker
                    v-model="meetingDateTimeRange"
                    type="datetimerange"
                    range-separator="至"
                    start-placeholder="开始日期"
                    end-placeholder="结束日期"
                >
                </el-date-picker>
            </el-form-item>
            <el-form-item>
                <el-button
                    type="primary"
                    @click="onSubmit(params, getDataList)"
                >
                    {{ menus.search.name }}
                </el-button>
                <el-button @click="resetForm(queryForm, params, getDataList)">
                    重置
                </el-button>
            </el-form-item>
        </el-form>

        <!-- 列表 -->
        <ve-table
            :table="{
                data: tableData,
            }"
            :pagination="{
                onSizeChange: (val) =>
                    handleSizeChange(val, params, getDataList),
                onCurrentChange: (val) =>
                    handleCurrentChange(val, params, getDataList),
                currentPage: page,
                pageSize: limit,
                total: total,
            }"
        >
            <template #tool_bar>
                <el-button
                    title="添加会议信息"
                    v-permission="['add']"
                    size="small"
                    type="primary"
                    @click="handleEditRoute(menus.add.name)"
                >
                    {{ menus.add.name }}
                </el-button>
            </template>
            <el-table-column prop="title" label="会议名称"></el-table-column>
            <el-table-column prop="keywords" label="会议主题"></el-table-column>
            <el-table-column prop="groupName" label="小组"></el-table-column>
            <el-table-column prop="meetingType" label="会议类型">
                <template v-slot="{ row }">
                    <a>{{ meetingTypes[row.meetingType] }}</a>
                </template>
            </el-table-column>
            <el-table-column
                prop="startTime"
                label="开始时间"
            ></el-table-column>
            <el-table-column prop="endTime" label="结束时间"></el-table-column>
            <el-table-column prop="place" label="会议地点"></el-table-column>
            <el-table-column prop="fileUrl" label="文件">
                <template v-slot="{ row }">
                    <el-button
                        :title="row.fileUrl"
                        @click.prevent="handleDownloadFile(row.fileUrl, row.id)"
                        type="primary"
                        size="small"
                    >
                        {{ menus.downloadFile.name }}
                    </el-button>
                    <el-progress
                        v-show="showProgress && currentClickRow == row.id"
                        :stroke-width="6"
                        :percentage="progressPercent"
                    ></el-progress>
                </template>
            </el-table-column>
            <el-table-column fixed="right" label="操作" width="270">
                <template v-slot:default="{ row }">
                    <el-button
                        v-if="row.meetingType === 2"
                        :title="row.code"
                        v-permission="['view']"
                        size="small"
                        type="primary"
                        @click="copyCode(row.code)"
                    >
                        {{ menus.view.name }}
                    </el-button>
                    <el-button
                        v-if="showButton(row)"
                        title="编辑"
                        v-permission="['edit']"
                        @click="handleEditRoute(menus.edit.name, row)"
                        type="warning"
                        size="small"
                    >
                        {{ menus.edit.name }}
                    </el-button>
                    <el-button
                        v-if="showButton(row)"
                        title="删除"
                        v-permission="['del']"
                        @click.prevent="handleDel(row.id)"
                        type="danger"
                        size="small"
                    >
                        {{ menus.del.name }}
                    </el-button>
                </template>
            </el-table-column>
        </ve-table>

        <!-- 编辑组件 -->
        <users-edit
            v-if="showDialog"
            :rowData="rowData"
            :title="dialogTitle"
            :showDialog="showDialog"
            @closeDialog="handelDialog($event)"
        />
    </div>
</template>
<script>
export default {
    data: () => ({
        // 对当前页面服务的描述
        description: "例会总结查询与操作",
        // 当前页面所有按钮
        menus: {
            search: { name: "查询" },
            add: { name: "添加" },
            edit: { name: "编辑" },
            del: { name: "删除" },
            downloadFile: { name: "下载" },
            view: { name: "会议编码" },
        },
    }),
};
</script>

<script setup>
// <!-- @click="handleViewRoute(menus.view.name, row.id)" -->
import { reactive, toRefs, ref, onMounted, getCurrentInstance } from "vue";
import { useRoute, useRouter } from "vue-router";
/**
 * 导入公共查询方法
 */
import {
    onSubmit,
    resetForm,
    handleCurrentChange,
    getAsyncRouteName,
    downloadFile,
    // getAsyncViewRouteName,
} from "@/views/layoutpages/common";
/**
 * 页面的所有数据
 */
const { proxy } = getCurrentInstance();
const route = useRoute();
const router = useRouter();
const rowData = ref(null);
const dialogTitle = ref("");
const showDialog = ref(false);
const queryForm = ref(null);
const tableData = ref([]);
const params = reactive({
    meetingKeywords: "",
    groupName: "",
    meetingDateTimeRange: null,
    limit: 10,
    page: 1,
    total: 0,
});
const { meetingKeywords, groupName, meetingDateTimeRange, limit, page, total } =
    toRefs(params);
const groupList = ref([]);
const meetingTypes = ["学术会议", "线下组会", "线上组会"];
/**
 * 文件下载
 */
const showProgress = ref(false);
const progressPercent = ref(0);
const currentClickRow = ref(-1);
const handleDownloadFile = async (fileUrl, id) => {
    await downloadFile(
        fileUrl,
        id,
        currentClickRow,
        showProgress,
        progressPercent,
    );
};
/**
 * 复制会议编码
 */
const copyCode = (code) => {
    const textarea = document.createElement("textarea");
    textarea.value = code;
    document.body.appendChild(textarea);
    textarea.select();
    document.execCommand("copy");
    document.body.removeChild(textarea);
    alert("已复制到剪贴板");
};
/**
 * 查看总结路由式
 */
// const handleViewRoute = async (title, id) => {
//     sessionStorage.removeItem("meetingInfosViewrowData");
//     sessionStorage.setItem(
//         "meetingInfosViewrowData",
//         JSON.stringify(tableData.value[id])
//     );
//     let toName = "MeetingInfosView";
//     router.push({ name: toName });
// };
/**
 * 添加页面
 */
const handleEditRoute = async (title, row = null) => {
    let pathId = "system/components/MeetingInfosEdit";
    const toName = await getAsyncRouteName(title, row, pathId, title, {
        router,
        route,
    });
    console.log("要过去了：" + toName + " L:" + title);
    router.push({ name: toName });
};
/**
 * dialog事件
 */
const handelDialog = (e) => {
    showDialog.value = e;
    getDataList();
};

/**删除行数据
 * @description:
 * @param {*}
 * @return {*}
 */
const handleDel = (id) => {
    proxy
        .$confirm("此操作将永久删除该数据, 是否继续?", "提示", {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "error",
        })
        .then(async () => {
            const { code } = await VE_API.system.meetingInfoDel({ id });
            if (code == 0) {
                getDataList();
            }
        })
        .catch(() => {
            proxy.$message({
                type: "info",
                message: "已取消删除",
            });
        });
};
/**
 * @description: 获取列表数据
 * @param {*}
 * @return {*}
 */
const getDataList = async () => {
    const { code, data } = await VE_API.system.meetingInfoList(params);
    if (code == 0) {
        const { limit, page, total, list } = data;
        params.limit = limit;
        params.page = page;
        params.total = total;
        tableData.value = list;
    }
};
const getGroupList = async () => {
    const { code, data } = await VE_API.system.groupList(
        {
            page: 1,
            limit: 10,
        },
        { Global: false },
    );
    if (code == 0) {
        const { list } = data;
        groupList.value = list;
    }
};
const showButton = (row) => {
    let enableList = ["管理员", "老师", "小组管理员"];
    return (
        row.userName === sessionStorage.getItem("userName") ||
        enableList.includes(sessionStorage.getItem("roleName"))
    );
};
onMounted(async () => {
    await getGroupList();
    await getDataList();
    // maxHeight(pagination, queryForm, toolBar, ve_max_height);
});
</script>

<style lang="scss" scoped>
.weekly-summary {
    margin: 30px;
}
.box-card {
    margin-bottom: 20px;
}
</style>
