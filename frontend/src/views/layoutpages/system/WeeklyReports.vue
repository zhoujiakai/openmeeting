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
            <el-form-item label="学生姓名" prop="userName">
                <el-input
                    clearable
                    v-model="userName"
                    placeholder="请输入学生姓名"
                ></el-input>
            </el-form-item>
            <el-form-item label="总结标题" prop="weeklyReportTheme">
                <el-input
                    clearable
                    v-model="weeklyReportTheme"
                    placeholder="请输入总结标题"
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
                    title="添加报告"
                    v-permission="['add']"
                    size="small"
                    type="primary"
                    @click="handleEditRoute(menus.add.name)"
                >
                    {{ menus.add.name }}
                </el-button>
            </template>
            <el-table-column prop="theme" label="总结标题"></el-table-column>
            <el-table-column prop="groupName" label="小组"></el-table-column>
            <el-table-column prop="userName" label="发布者"></el-table-column>
            <el-table-column
                prop="uploadDate"
                label="更新时间"
            ></el-table-column>
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
            <el-table-column
                prop="downloadCount"
                label="下载次数"
            ></el-table-column>
            <el-table-column prop="status" label="审核状态">
                <template v-slot="{ row }">
                    <a
                        v-if="row.status === 0"
                        style="
                            color: red;
                            background-color: antiquewhite;
                            border-radius: 4px;
                        "
                        >未通过</a
                    >
                    <a
                        v-else
                        style="
                            color: green;
                            background-color: antiquewhite;
                            border-radius: 4px;
                        "
                        >通过</a
                    >
                </template>
            </el-table-column>
            <el-table-column fixed="right" label="操作" width="220">
                <template v-slot:default="{ row }">
                    <el-button
                        title="查看报告"
                        v-permission="['view']"
                        size="small"
                        type="primary"
                        @click="handleViewRoute(menus.view.name, row.id)"
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
                        :title="menus.del.name"
                        v-if="showButton(row)"
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
            edit: { name: "修改" },
            del: { name: "删除" },
            downloadFile: { name: "下载" },
            view: { name: "查看报告" },
        },
    }),
};
</script>

<script setup>
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
    // getAsyncViewRouteName,
    downloadFile,
} from "@/views/layoutpages/common";
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

// 页面的所有数据
const { proxy } = getCurrentInstance();
const route = useRoute();
const router = useRouter();
const queryForm = ref(null);
const tableData = ref([]);
const params = reactive({
    userName: "",
    groupName: "",
    weeklyReportTheme: "",
    limit: 10,
    page: 1,
    total: 0,
});
const { userName, groupName, weeklyReportTheme, limit, page, total } =
    toRefs(params);

const groupList = ref([]);
/**
 * 查看总结
 */
const handleViewRoute = async (title, id) => {
    // sessionStorage.setItem("weeklyReportsId", id);
    console.log(`这是展示id: ${id}`);
    console.log(`这是内容: ${JSON.stringify(tableData.value[id])}`);
    router.push({
        name: "WeeklyReportsView",
        query: {
            id: id,
        },
    });
};
/**
 * 添加页面路由式
 */
const handleEditRoute = async (title, row = null) => {
    let pathId = "system/components/WeeklyReportEdit";
    const toName = await getAsyncRouteName(title, row, pathId, title, {
        router,
        route,
    });
    console.log("要过去了：" + toName + " L:" + title);
    router.push({ name: toName });
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
            const { code } = await VE_API.system.weeklyReportDel({ id });
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
    const { code, data } = await VE_API.system.weeklyReportList(params);
    console.log(data);
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
});
</script>

<style lang="scss" scoped>
.weekly-summary {
    margin: 30px;
}
.box-card {
    margin-bottom: 20px;
}
.ve_container {
    background-image: "https://meeting.oss-cn-qingdao.aliyuncs.com/systembackground.jpg";
}
</style>
