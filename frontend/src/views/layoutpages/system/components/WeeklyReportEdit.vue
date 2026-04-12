<!--
 * @Author: your name
 * @Date: 2021-02-09 15:24:23
 * @LastEditTime: 2022-04-28 16:32:27
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \vue3-element-admin\src\views\layoutpages\system\components\usersEdit.vue
-->
<template>
    <div class="weekly-summary">
        <el-card class="box-card">
            <el-form :model="form">
                <el-form-item label="本周主题">
                    <el-input
                        type="textarea"
                        v-model="theme"
                        placeholder="请输入本周总结主题"
                        :autosize="{ minRows: 1, maxRows: 2 }"
                    ></el-input>
                </el-form-item>
                <el-form-item label="工作内容">
                    <el-input
                        type="textarea"
                        v-model="workContent"
                        placeholder="请输入本周的工作内容"
                        :autosize="{ minRows: 4, maxRows: 8 }"
                    ></el-input>
                </el-form-item>
                <el-form-item label="进展安排">
                    <el-input
                        type="textarea"
                        v-model="workSchedule"
                        placeholder="请输入未来的工作安排"
                        :autosize="{ minRows: 4, maxRows: 8 }"
                    ></el-input>
                </el-form-item>
                <el-form-item label="问题方案">
                    <el-input
                        type="textarea"
                        v-model="problemAndSolution"
                        placeholder="请输入遇到的问题及解决方案"
                        :autosize="{ minRows: 4, maxRows: 8 }"
                    ></el-input>
                </el-form-item>
                <input
                    type="file"
                    @change="handleFileUpload"
                    style="margin-left: 80px"
                />
                <div class="show-file" v-if="showFile">
                    原文件：{{ fileUrl }}
                </div>
                <el-progress
                    v-show="showProgress"
                    :stroke-width="6"
                    :percentage="progressPercent"
                    style="width: 500px; padding-left: 80px"
                ></el-progress>
            </el-form>
            <!-- 按钮操作区域 -->
            <div class="operation-area">
                <el-button @click="closeDialog()">取消</el-button>
                <el-button type="primary" @click="onSubmit">提交总结</el-button>
            </div>
        </el-card>
    </div>
</template>

<script setup>
/**
 * 引入组件
 */
import { reactive, toRefs, ref, computed } from "vue";
import { uploadFile } from "@/views/layoutpages/common";
/**
 * 从父组件传参
 */
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
const { title, rowData } = toRefs(props);
const form = reactive({
    theme: "",
    userName: "",
    workContent: "",
    workSchedule: "",
    problemAndSolution: "",
    fileUrl: "",
    teacherComment: "",
    status: 0,
});
const { theme, workContent, workSchedule, problemAndSolution, fileUrl } =
    toRefs(form);
rowData.value &&
    ((form.theme = rowData.value.theme),
    (form.userName = rowData.value.userName),
    (form.workContent = rowData.value.workContent),
    (form.workSchedule = rowData.value.workSchedule),
    (form.problemAndSolution = rowData.value.problemAndSolution),
    (form.fileUrl = rowData.value.fileUrl),
    (form.teacherComment = rowData.value.teacherComment),
    (form.status = rowData.value.status));
rowData.value &&
    ((theme.value = rowData.value.theme),
    (workContent.value = rowData.value.workContent),
    (workSchedule.value = rowData.value.workSchedule),
    (problemAndSolution.value = rowData.value.problemAndSolution),
    (fileUrl.value = rowData.value.fileUrl));
/**
 * 处理文件上传
 */
const newFileUrl = ref("");
const showProgress = ref(false);
const progressPercent = ref(0);
const showFile = computed(() => {
    // 1. 旧的文件路径存在 2. 新的文件路径为空
    return fileUrl.value !== "" && newFileUrl.value === "";
});
const handleFileUpload = async (event) => {
    newFileUrl.value = await uploadFile(event, showProgress, progressPercent);
    console.log("这是新的文件：" + newFileUrl.value);
};
/**
 * 提交
 */
const onSubmit = async () => {
    let res;
    form.userName = sessionStorage.getItem("userName");
    // 检查用户是否重新上传了文件
    form.fileUrl = newFileUrl.value !== "" ? newFileUrl.value : form.fileUrl;
    if (title.value === "添加") {
        res = await VE_API.system.weeklyReportAdd(form);
    } else {
        let newObj = { ...rowData.value };
        newObj.theme = theme.value;
        newObj.workContent = workContent.value;
        newObj.workSchedule = workSchedule.value;
        newObj.problemAndSolution = problemAndSolution.value;
        newObj.fileUrl =
            newFileUrl.value !== "" ? newFileUrl.value : form.fileUrl;
        res = await VE_API.system.weeklyReportEdit(newObj);
    }
    const { code } = res;
    if (code === 0) {
        closeDialog();
    }
};
const closeDialog = () => {
    emit("closeDialog", false);
};

const emit = defineEmits(["closeDialog"]);
</script>

<style lang="scss" scoped>
.weekly-summary {
    margin: 30px;
}
.box-card {
    margin-bottom: 20px;
}
.upload-component {
    margin: 30px;
}
.operation-area {
    display: flex;
    justify-content: space-between;
    margin-top: 20px;
    .el-button {
        margin-left: 10px;
    }
}
.show-file {
    font-size: small;
    padding-left: 80px;
    padding-top: 20px;
}
</style>
