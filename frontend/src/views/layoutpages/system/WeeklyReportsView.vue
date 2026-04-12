<template>
    <div>
        <el-page-header
            @back="$router.back()"
            :content="title"
        ></el-page-header>
    </div>
    <div class="page-container">
        <!-- 页面顶部的加粗标题 -->
        <h3 class="page-title">{{ reportContent.theme }}</h3>

        <!-- 工作内容部分 -->
        <div class="section">
            <h4 class="section-title">工作内容</h4>
            <el-card class="content-card">
                <div class="content">
                    <p>
                        {{ reportContent.workContent }}
                    </p>
                </div>
            </el-card>
        </div>

        <!-- 工作计划部分 -->
        <div class="section">
            <h4 class="section-title">工作计划</h4>
            <el-card class="content-card">
                <div class="content">
                    <p>
                        {{ reportContent.workSchedule }}
                    </p>
                </div>
            </el-card>
        </div>

        <!-- 问题与解决方案部分 -->
        <div class="section">
            <h4 class="section-title">问题与解决方案</h4>
            <el-card class="content-card">
                <div class="content">
                    <p>
                        {{ reportContent.problemAndSolution }}
                    </p>
                </div>
            </el-card>
        </div>
        <div class="middle-buttons">
            <div>
                <el-button
                    :title="reportContent.fileUrl"
                    size="small"
                    type="warning"
                    @click="handleDownloadFile"
                    class="downloadButton"
                >
                    下载报告附件
                </el-button>
                <el-progress
                    v-show="showProgress"
                    :stroke-width="6"
                    :percentage="progressPercent"
                    style="
                        width: 200px;
                        padding-left: 120px;
                        padding-bottom: 20px;
                        margin-top: -40px;
                    "
                ></el-progress>
            </div>

            <el-button
                v-if="isTeacher && reportContent.status === 0"
                size="small"
                type="primary"
                @click="changeWeeklyReportsStatus"
                class="rejectWeeklyReportsButton"
            >
                通过审核
            </el-button>
            <el-button
                v-if="isTeacher && reportContent.status === 1"
                size="small"
                type="danger"
                @click="changeWeeklyReportsStatus"
                class="rejectWeeklyReportsButton"
            >
                打回报告
            </el-button>
            <el-button
                v-if="isTeacher"
                size="small"
                type="primary"
                @click="editTeacherComment"
                class="rejectWeeklyReportsButton"
            >
                修改审核意见
            </el-button>
        </div>
        <div v-if="!isTeacher || !isEditingTeacherComment" class="section">
            <h4 class="section-title">导师审核意见</h4>
            <el-card class="content-card">
                <div class="content">
                    <p>
                        {{ reportContent.teacherComment }}
                    </p>
                </div>
            </el-card>
        </div>
        <div v-if="isEditingTeacherComment">
            <h4 class="section-title">导师审核意见</h4>
            <el-input
                type="textarea"
                rows="5"
                :placeholder="reportContent.teacherComment"
                v-model="reportContent.teacherComment"
            ></el-input>
            <div class="submit-button">
                <el-button type="cancel" @click="cancelEditTeacherComment"
                    >取消</el-button
                >
                <el-button type="primary" @click="submitTeacherComment"
                    >确认修改</el-button
                >
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, onUnmounted, computed } from "vue";
import { useRoute } from "vue-router";
import { downloadFile } from "@/views/layoutpages/common";
const route = useRoute();
const reportContent = ref({});
const isEditingTeacherComment = ref(false);
const getDataList = async () => {
    let id = route.query.id;
    const { data } = await VE_API.system.weeklyReportGet({ id: id });
    reportContent.value = data;
};

/**
 * 下载报告
 */
const showProgress = ref(false);
const progressPercent = ref(0);
const currentClickRow = ref(-1);
const handleDownloadFile = async () => {
    await downloadFile(
        reportContent.value.fileUrl,
        route.query.id,
        currentClickRow,
        showProgress,
        progressPercent,
    );
};
/**
 * 教师审核
 */
const isTeacher = computed(() => {
    return sessionStorage.getItem("roleName") === "老师";
});
const editTeacherComment = () => {
    isEditingTeacherComment.value = !isEditingTeacherComment.value;
};
const cancelEditTeacherComment = () => {
    isEditingTeacherComment.value = !isEditingTeacherComment.value;
};
const submitTeacherComment = async () => {
    const { data, code } = await VE_API.system.weeklyReportEdit(
        reportContent.value,
    );
    console.log(data, code);
    isEditingTeacherComment.value = false;
    getDataList();
};
const changeWeeklyReportsStatus = async () => {
    reportContent.value.status = (reportContent.value.status + 1) % 2;
    await VE_API.system.weeklyReportEdit(reportContent.value);
    console.log("审核状态：" + reportContent.value.status);
};

onMounted(async () => {
    await getDataList();
    // maxHeight(pagination, queryForm, toolBar, ve_max_height);
});
onBeforeUnmount(() => {
    // 执行一些清理操作
    console.log("Component is about to be unmounted的事发生事故的");
});
onUnmounted(() => {
    window.location.reload();
});
</script>

<style scoped>
.page-container {
    padding: 20px;
}
.page-title {
    font-weight: bold;
    margin-bottom: 20px;
    text-align: center;
}
.section {
    margin-bottom: 30px;
    display: flex;
    flex-direction: row;
}
.section-title {
    font-weight: bold;
    width: 130px;
    margin-bottom: 10px;
}
.content-card {
    padding: 15px;
    width: 1000px;
    height: auto;
}
.content {
    /* 自适应行高的样式 */
    display: -webkit-box;
    -webkit-box-orient: vertical;
    /* -webkit-line-clamp: 2; 限制在两行内 */
    overflow: hidden;
    text-overflow: ellipsis;
    /* 其他样式 */
    word-break: break-word; /* 防止单词在行末被截断 */
    /* 取消内联元素的默认行高，以便自适应 */
    line-height: normal;
}

.downloadButton {
    margin-left: 150px;
    margin-bottom: 50px;
}
.rejectWeeklyReportsButton {
    margin-left: 50px;
    margin-bottom: 50px;
}
.submit-button {
    margin-top: 10px;
    text-align: right;
}
.middle-buttons {
    display: flex;
    flex-direction: row;
}
</style>
