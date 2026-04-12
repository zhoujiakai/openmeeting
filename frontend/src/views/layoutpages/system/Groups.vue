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
            <el-form-item label="小组名称" prop="groupName">
                <el-input
                    clearable
                    v-model="groupName"
                    placeholder="请输入小组名称"
                ></el-input>
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
                    title="弹窗式"
                    v-permission="['add']"
                    size="small"
                    type="primary"
                    @click="handleEdit(menus.add.name)"
                >
                    {{ menus.add.name }}
                </el-button>
            </template>
            <el-table-column
                prop="groupName"
                label="小组名称"
            ></el-table-column>
            <el-table-column prop="status" label="状态">
                <template v-slot="{ row }">
                    <el-switch
                        :loading="row.load ? true : false"
                        v-model="row.status"
                        :active-value="1"
                        :inactive-value="0"
                        active-color="#13ce66"
                        inactive-color="#ff4949"
                        @change="(val) => handelSwitch(val, row)"
                    >
                        >
                    </el-switch>
                </template>
            </el-table-column>
            <el-table-column fixed="right" label="操作">
                <template v-slot:default="{ row }">
                    <el-button
                        v-permission="['edit']"
                        @click.prevent="handleEdit(menus.edit.name, row)"
                        type="primary"
                        size="small"
                    >
                        {{ menus.edit.name }}
                    </el-button>
                    <el-button
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
        <groups-edit
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
        description: "用户信息查询与设置",
        menus: {
            search: { name: "查询" },
            add: { name: "添加" },
            edit: { name: "编辑" },
            del: { name: "删除" },
        },
    }),
};
</script>

<script setup>
import GroupsEdit from "./components/GroupsEdit";
import { reactive, toRefs, ref, onMounted, getCurrentInstance } from "vue";
//导入公共查询方法
import {
    onSubmit,
    resetForm,
    handleSizeChange,
    handleCurrentChange,
} from "@/views/layoutpages/common";

const { proxy } = getCurrentInstance();
const rowData = ref(null);
const dialogTitle = ref("");
const showDialog = ref(false);
const queryForm = ref(null);
const tableData = ref([]);
const params = reactive({
    groupName: "",
    limit: 10,
    page: 1,
    total: 0,
});
const { groupName, limit, page, total } = toRefs(params);

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
 * @description:用户状态切换
 * @param {*}
 * @return {*}
 */
const handelSwitch = async (val, row) => {
    if (row.id == undefined) return;
    row.load = 1;
    const { code } = await VE_API.system.groupStatus(
        {
            id: row.id,
            status: val,
        },
        { Global: false },
    );
    setTimeout(() => {
        row.load = 0;
        if (code != 0) {
            row.status = val == 1 ? 0 : 1;
        }
    }, 500);
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
            console.log({ id: id });
            const { code } = await VE_API.system.groupDel({ id: id });
            console.log(code);
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
    const { code, data } = await VE_API.system.groupList(params);
    if (code == 0) {
        const { limit, page, total, list } = data;
        params.limit = limit;
        params.page = page;
        params.total = total;
        tableData.value = list;
    }
};
onMounted(async () => {
    await getDataList();
    // maxHeight(pagination, queryForm, toolBar, ve_max_height);
});
</script>

<style lang="scss" scoped></style>
