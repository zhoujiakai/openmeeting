<!--
 * @Author: your name
 * @Date: 2021-01-18 15:46:34
 * @LastEditTime: 2021-11-30 18:54:36
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \vue3-element-admin\src\views\Home.vue
-->
<template>
    <div class="ve_home">
        <el-row>
            <el-col :span="16"><dashboard /></el-col>
            <el-col :span="8">
                <el-calendar class="ve_calendar"></el-calendar>
            </el-col>
        </el-row>
        <el-carousel :interval="3000">
            <el-carousel-item v-for="(image, index) in images" :key="index">
                <img :src="image" />
            </el-carousel-item>
        </el-carousel>
    </div>
</template>

<script setup>
import dashboard from "@/components/dashboard/Shortcuts";
import lunbo1 from "@/assets/lunbo1.jpg";
import lunbo2 from "@/assets/lunbo2.jpg";
import lunbo3 from "@/assets/lunbo3.jpg";
import lunbo4 from "@/assets/lunbo4.jpg";

const images = [lunbo1, lunbo2, lunbo3, lunbo4];
</script>

<style lang="scss" scoped>
.ve_calendar {
    background: #799be5;
    border-radius: 10px;
    &:hover {
        box-shadow: 3px 3px 6px 1px rgba(0, 0, 0, 0.2);
    }

    :deep(.el-calendar__body) {
        padding-bottom: 12px !important;
        .el-calendar-day {
            height: auto !important;
        }
        .el-calendar-table td {
            border: none;
            text-align: center;
        }
    }
}
.el-carousel {
    padding-top: 30px;
}
.el-carousel__item {
    width: 100%;
    height: 300px; /* 根据需要设置高度 */
    object-fit: cover; /* 确保图片覆盖整个容器 */
}
img {
    border-radius: 20px; /* 设置圆角的半径 */
    height: 100%;
    width: 100%;
    object-fit: cover; /* 确保图片覆盖整个容器 */
}
</style>
