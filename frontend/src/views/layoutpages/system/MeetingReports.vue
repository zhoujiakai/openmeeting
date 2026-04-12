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
            <el-form-item label="会议" prop="meetingId">
                <el-select clearable v-model="meetingId" placeholder="请选择">
                    <el-option
                        v-for="item in meetingList"
                        :key="item.id"
                        :label="item.title"
                        :value="item.id"
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
                    :title="menus.add.name"
                    v-permission="['add']"
                    size="small"
                    type="primary"
                    @click="handleEdit(menus.add.name)"
                >
                    {{ menus.add.name }}
                </el-button>
            </template>
            <el-table-column prop="title" label="标题"></el-table-column>
            <el-table-column
                prop="meetingTitle"
                label="会议名"
            ></el-table-column>
            <el-table-column prop="groupName" label="小组"></el-table-column>
            <el-table-column prop="userName" label="发布者"></el-table-column>
            <el-table-column
                prop="uploadDate"
                label="发布时间"
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
            <el-table-column fixed="right" label="操作">
                <template v-slot:default="{ row }">
                    <el-button
                        :title="menus.edit.name"
                        v-permission="['edit']"
                        @click.prevent="handleEdit(menus.edit.name, row)"
                        type="primary"
                        size="small"
                    >
                        {{ menus.edit.name }}
                    </el-button>
                    <el-button
                        :title="menus.del.name"
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
        <meetingReport-edit
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
        },
    }),
};
</script>

<script setup>
import MeetingReportEdit from "./components/MeetingReportEdit";
import { reactive, toRefs, ref, onMounted, getCurrentInstance } from "vue";
/**
 * 导入公共查询方法
 */
import {
    onSubmit,
    resetForm,
    handleCurrentChange,
    downloadFile,
} from "@/views/layoutpages/common";
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
 * 页面的所有数据
 */
const { proxy } = getCurrentInstance();
const rowData = ref(null);
const dialogTitle = ref("");
const showDialog = ref(false);
const queryForm = ref(null);
const tableData = ref([]);
const params = reactive({
    meetingId: null,
    groupName: "",
    limit: 10,
    page: 1,
    total: 0,
});
const { meetingId, groupName, limit, page, total } = toRefs(params);

const groupList = ref([]);
const meetingList = ref([]);
/**
 * @description:添加or编辑事件
 * @param {*}
 * @return {*}
 */
const handleEdit = (title, row = null) => {
    showDialog.value = true;
    dialogTitle.value = title;
    rowData.value = row;
};
/**
 * @description: dialog事件
 * @param {*}
 * @return {*}
 */
const handelDialog = (e) => {
    showDialog.value = e;
    getDataList();
};

/**
 * 删除行数据
 */
const handleDel = (id) => {
    proxy
        .$confirm("此操作将永久删除该数据, 是否继续?", "提示", {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "error",
        })
        .then(async () => {
            const { code } = await VE_API.system.meetingReportDel({ id });
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
const getMeetingList = async () => {
    const { code, data } = await VE_API.system.meetingInfoList(
        {
            page: 1,
            limit: 100,
        },
        { Global: false },
    );
    if (code == 0) {
        const { list } = data;
        meetingList.value = list;
    }
};
const getDataList = async () => {
    let obj = { ...params };
    obj.meetingId = obj.meetingId ? obj.meetingId : 0;
    const { code, data } = await VE_API.system.meetingReportList(obj);
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

onMounted(async () => {
    await getMeetingList();
    await getGroupList();
    await getDataList();
    // maxHeight(pagination, queryForm, toolBar, ve_max_height);
});
</script>

<style lang="scss" scoped></style>
