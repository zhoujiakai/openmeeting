<template>
    <div>
        <el-form :model="form" label-width="80px">
            <el-form-item label="原密码" prop="oldPassword">
                <el-input v-model="form.oldPassword" type="password"></el-input>
            </el-form-item>
            <el-form-item label="新密码" prop="newPassword">
                <el-input v-model="form.newPassword" type="password"></el-input>
            </el-form-item>
            <el-form-item label="确认密码" prop="confirmPassword">
                <el-input
                    v-model="form.confirmPassword"
                    type="password"
                ></el-input>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="submitForm('form')"
                    >提交</el-button
                >
            </el-form-item>
        </el-form>
    </div>
</template>

<script setup>
import { reactive } from "vue";
const form = reactive({
    oldPassword: "123456",
    newPassword: "123456",
    confirmPassword: "123456",
});
const submitForm = async () => {
    console.log(form);
    const { code } = await VE_API.system.changePassword(
        {
            userName: sessionStorage.getItem("userName"),
            oldPassword: form.oldPassword,
            newPassword: form.newPassword,
        },
        { Global: false },
    );
    console.log(code);
    if (code == 0) {
        console.log("修改成功");
    } else {
        console.log("密码错误");
    }
};
</script>
<style scoped>
.el-input {
    width: 400px;
}
</style>
