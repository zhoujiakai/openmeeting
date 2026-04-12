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
            <el-form-item label="标题" prop="meetingReportTitle">
                <el-input
                    v-model="meetingReportTitle"
                    placeholder=""
                    clearable
                ></el-input>
            </el-form-item>
            <el-form-item label="会议" prop="meetingId">
                <el-select
                    style="width: 100%"
                    v-model="meetingId"
                    placeholder=""
                    clearable
                >
                    <el-option
                        v-for="item in meetingList"
                        :key="item.id"
                        :label="item.title"
                        :value="item.id"
                        :disabled="item.status == 0"
                    ></el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="小组" prop="groupName">
                <el-select
                    style="width: 100%"
                    v-model="groupName"
                    placeholder=""
                    clearable
                >
                    <el-option
                        v-for="item in groupList"
                        :key="item.groupName"
                        :label="item.groupName"
                        :value="item.groupName"
                        :disabled="item.status == 0"
                    ></el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="" prop="fileUrl">
                <input type="file" @change="handleFileUpload" />
                <div class="show-file" v-if="showFile">
                    原文件：{{ fileUrl }}
                </div>
                <el-progress
                    v-show="showProgress"
                    :stroke-width="6"
                    :percentage="progressPercent"
                    style="width: 200px"
                ></el-progress>
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
import { reactive, toRefs, ref, onMounted, computed } from "vue";
import { uploadFile } from "@/views/layoutpages/common";
/**
 * 初始化
 */
const { title, rowData } = toRefs(props);
const formRef = ref(null);
const form = reactive({
    meetingReportTitle: "",
    meetingId: "",
    groupName: "",
    fileUrl: "",
    status: 1,
});
const { meetingReportTitle, meetingId, groupName, fileUrl, status } =
    toRefs(form);
rowData.value &&
    ((meetingReportTitle.value = rowData.value.title),
    (meetingId.value = rowData.value.meetingId),
    (groupName.value = rowData.value.groupName),
    (fileUrl.value = rowData.value.fileUrl),
    (status.value = rowData.value.status));
/**
 * 获取列表数据
 */
const groupList = ref([]);
const meetingList = ref([]);
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
/**
 * 文件上传
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
const onSubmit = () => {
    formRef.value.validate(async (valid) => {
        if (valid) {
            let res;
            form.userName = sessionStorage.getItem("userName");
            // 检查用户是否重新上传了文件
            form.fileUrl =
                newFileUrl.value !== "" ? newFileUrl.value : form.fileUrl;
            if (title.value === "添加") {
                let obj = { ...form };
                obj.title = obj.meetingReportTitle;
                res = await VE_API.system.meetingReportAdd(obj);
            } else {
                console.log("修改数据");
                let newObj = { ...rowData.value };
                newObj.title = form.meetingReportTitle;
                newObj.groupName = form.groupName;
                newObj.meetingId = form.meetingId;
                newObj.fileUrl =
                    newFileUrl.value !== "" ? newFileUrl.value : form.fileUrl;
                res = await VE_API.system.meetingReportEdit(newObj);
            }
            const { code } = res;
            if (code === 0) {
                closeDialog();
            }
        } else {
            console.log("error submit!!");
            return false;
        }
    });
};
const rules = {
    name: [
        {
            required: true,
            message: "请输入用户名",
            trigger: "blur",
        },
    ],
    userName: [
        {
            required: true,
            message: "请输入账户",
            trigger: "blur",
        },
    ],
    password: [
        {
            required: true,
            message: "请输入密码",
            trigger: "blur",
        },
    ],
    role: [
        {
            required: true,
            message: "请选择角色",
            trigger: "change",
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
const closeDialog = () => {
    emit("closeDialog", false);
};

onMounted(async () => {
    await getMeetingList();
    await getGroupList();
});
</script>

<style lang="scss" scoped></style>
