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
                <el-form-item label="会议名称">
                    <el-input
                        type="textarea"
                        v-model="theme"
                        placeholder="会议名称"
                        :autosize="{ minRows: 1, maxRows: 2 }"
                    ></el-input>
                </el-form-item>
                <el-form-item label="会议主题">
                    <el-input
                        type="textarea"
                        v-model="keywords"
                        placeholder="会议主题"
                        :autosize="{ minRows: 1, maxRows: 2 }"
                    ></el-input>
                </el-form-item>
                <el-form-item label="小组名称" prop="groupName">
                    <el-select
                        clearable
                        v-model="groupName"
                        placeholder="请选择"
                    >
                        <el-option
                            v-for="item in groupList"
                            :key="item.id"
                            :label="item.groupName"
                            :value="item.groupName"
                        ></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="会议类型" prop="meetingType">
                    <el-select v-model="meetingType" placeholder="" clearable>
                        <el-option
                            v-for="item in meetingTypeList"
                            :key="item.id"
                            :label="item.type"
                            :value="item.id"
                        ></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="会议时间" style="width: 500px">
                    <el-date-picker
                        v-model="meetingDateTimeRange"
                        type="datetimerange"
                        range-separator="至"
                        start-placeholder="开始日期"
                        end-placeholder="结束日期"
                    >
                    </el-date-picker>
                </el-form-item>
                <el-form-item label="会议地点">
                    <el-input
                        type="textarea"
                        v-model="place"
                        placeholder="请输入会议地点"
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
                <el-button type="primary" @click="onSubmit"
                    >提交会议信息</el-button
                >
            </div>
        </el-card>
    </div>
</template>

<script setup>
/**
 * 引入组件
 */
import { reactive, toRefs, ref, computed, onMounted } from "vue";
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
    keywords: "",
    groupName: "",
    meetingType: "",
    startTime: "",
    endTime: "",
    place: "",
    fileUrl: "",
});
const { theme, keywords, groupName, meetingType, place, fileUrl } =
    toRefs(form);
const meetingDateTimeRange = ref([]);
rowData.value &&
    ((form.id = rowData.value.id),
    (form.theme = rowData.value.title),
    (form.keywords = rowData.value.keywords),
    (form.groupName = rowData.value.groupName),
    (form.meetingType = rowData.value.meetingType),
    (form.startTime = rowData.value.startTime),
    (form.endTime = rowData.value.endTime),
    (form.place = rowData.value.place),
    (form.place = rowData.value.place),
    (form.fileUrl = rowData.value.fileUrl),
    (meetingDateTimeRange.value[0] = rowData.value.startTime),
    (meetingDateTimeRange.value[1] = rowData.value.endTime));
// rowData.value &&
//     ((theme.value = rowData.value.title),
//     (keywords.value = rowData.value.keywords),
//     (groupName.value = rowData.value.groupName),
//     (meetingType.value = rowData.value.meetingType),
//     (startTime.value = rowData.value.startTime),
//     (endTime.value = rowData.value.endTime),
//     (place.value = rowData.value.place),
//     (fileUrl.value = rowData.value.fileUrl));

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
    form.startTime =
        meetingDateTimeRange.value.length > 1
            ? meetingDateTimeRange.value[0]
            : Date.now;
    form.endTime =
        meetingDateTimeRange.value.length > 1
            ? meetingDateTimeRange.value[1]
            : Date.now;
    form.title = form.theme;
    // 检查用户是否重新上传了文件
    form.fileUrl = newFileUrl.value !== "" ? newFileUrl.value : form.fileUrl;
    if (title.value === "添加") {
        console.log("开始添加，主题：" + form.meetingType);
        res = await VE_API.system.meetingInfoAdd(form);
    } else {
        // let newObj = { ...rowData.value };
        // newObj.theme = theme.value;
        // newObj.keywords = keywords.value;
        // newObj.groupName = groupName.value;
        // newObj.meetingType = meetingType.value;
        // newObj.startTime = startTime.value;
        // newObj.endTime = endTime.value;
        // newObj.place = place.value;
        // res = await VE_API.system.weeklyReportEdit(newObj);
        res = await VE_API.system.meetingInfoEdit(form);
    }
    const { code } = res;
    if (code === 0) {
        closeDialog();
    }
};
/**
 * 小组列表和会议类型列表
 */
const groupList = ref([]);
const meetingTypeList = [
    {
        id: 0,
        type: "学术会议",
    },
    {
        id: 1,
        type: "线下会议",
    },
    {
        id: 2,
        type: "线上会议",
    },
];
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
 * 钩子函数
 */
onMounted(() => {
    getGroupList();
});
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
