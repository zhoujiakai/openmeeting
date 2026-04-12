<template>
    <div>
        <el-page-header
            @back="$router.back()"
            :content="title"
        ></el-page-header>
    </div>
    <div class="page-container">
        <!-- 页面顶部的加粗标题 -->
        <h3 class="page-title">{{ reportContent.title }}</h3>

        <!-- 工作内容部分 -->
        <div class="section">
            <h4 class="section-title">会议主题</h4>
            <el-card class="content-card">
                <div class="content">
                    <p>
                        {{ reportContent.keywords }}
                    </p>
                </div>
            </el-card>
        </div>

        <!-- 工作计划部分 -->
        <div class="section">
            <h4 class="section-title">会议时间</h4>
            <el-card class="content-card">
                <div class="content">
                    <p>
                        {{ reportContent.startTime }}
                    </p>
                </div>
            </el-card>
        </div>

        <div class="section">
            <h4 class="section-title">会议地点</h4>
            <el-card class="content-card">
                <div class="content">
                    <p>
                        {{ reportContent.place }}
                    </p>
                </div>
            </el-card>
        </div>

        <div class="section">
            <h4 class="section-title">会议编码</h4>
            <el-card class="content-card">
                <div class="content">
                    <p>
                        {{ reportContent.code }}
                    </p>
                </div>
            </el-card>
        </div>
    </div>
    <!-- <Comments></Comments> -->
</template>

<script setup>
// reportContent部分
// import { useRoute } from "vue-router";
import { onUnmounted } from "vue";
// const route = useRoute();
// const reportContent = JSON.parse(route.params.reportContent);
const reportContent = JSON.parse(
    sessionStorage.getItem("meetingInfosViewrowData"),
);

// 评论部分
// import { ref } from "vue";
// import UserAvatar from './UserAvatar.vue';
// import Comments from "@/components/comment/Comments.vue";

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
/* 评论部分样式 */
.comments-container {
    padding: 20px;
}

.comment {
    margin-bottom: 20px;
    border-bottom: 1px solid #eee;
    padding-bottom: 10px;
}

.comment-content {
    display: flex;
    align-items: center;
}

.comment-content p {
    margin: 0;
}
</style>
