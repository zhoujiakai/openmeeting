<!--
 * @Author: your name
 * @Date: 2021-02-09 15:24:23
 * @LastEditTime: 2022-04-28 16:32:16
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \vue3-element-admin\src\views\layoutpages\system\components\usersEdit.vue
-->
<template>
    <el-dialog
        :title="title"
        append-to-body
        destroy-on-close
        :model-value="showDialog"
        @close="closeDialog()"
    >
        <!-- <span>{{ rowData }}</span> -->
        <!-- 表单 -->
        <el-form
            :model="form"
            ref="formRef"
            :rules="rules"
            label-width="80px"
            :inline="false"
        >
            <el-form-item label="小组名称" prop="groupName">
                <el-input
                    v-model="groupName"
                    placeholder=""
                    clearable
                ></el-input>
            </el-form-item>
            <el-form-item label="状态">
                <el-radio-group v-model="status">
                    <el-radio-button :label="1">启用</el-radio-button>
                    <el-radio-button :label="0">停用</el-radio-button>
                </el-radio-group>
            </el-form-item>
        </el-form>

        <template v-slot:footer>
            <span>
                <el-button @click="closeDialog()">取消</el-button>
                <el-button type="primary" @click="onSubmit()">确定</el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script setup>
import { reactive, toRefs, ref } from "vue";
const rules = {
    groupName: [
        {
            required: true,
            message: "请输入账户",
            trigger: "blur",
        },
    ],
};
const props = defineProps({
    showDialog: {
        type: Boolean,
        default: true,
    },
    title: {
        type: String,
        default: "添加",
    },
    rowData: {
        type: Object,
        default: null,
    },
});
const emit = defineEmits(["closeDialog"]);
const { title, rowData } = toRefs(props);
const closeDialog = () => {
    emit("closeDialog", false);
};
const formRef = ref(null);
const form = reactive({
    groupName: "",
    status: 1,
});
const { groupName, status } = toRefs(form);
/**
 * @description: 初始化
 * @param {*}
 * @return {*}
 */
rowData.value &&
    ((groupName.value = rowData.value.groupName),
    (status.value = rowData.value.status));
/**
 * @description:提交
 * @param {*}
 * @return {*}
 */
const onSubmit = () => {
    formRef.value.validate(async (valid) => {
        if (valid) {
            let res;
            if (title.value == "添加") {
                res = await VE_API.system.groupAdd(form);
            } else {
                let temp = {
                    id: rowData.value.id,
                    ...form,
                };
                console.log(temp);
                res = await VE_API.system.groupEdit({
                    id: rowData.value.id,
                    ...form,
                });
            }
            const { code } = res;
            if (code == 0) {
                closeDialog();
            }
        } else {
            console.log("error submit!!");
            return false;
        }
    });
};
</script>

<style lang="scss" scoped></style>
