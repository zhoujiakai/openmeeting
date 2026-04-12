<!--
 * @Author: your name
 * @Date: 2021-01-18 09:41:14
 * @LastEditTime: 2022-01-20 14:26:41
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \vue3-element-admin\src\components\Common.vue
-->
<template>
    <div class="ve_404">
        <slot>
            <div class="error">
                <div class="error__title">404</div>
                <div class="error__subtitle">🐕🐕🐕(⓿_⓿)🐕🐕🐕</div>
                <div class="error__description">看来你是迷路了......</div>
                <router-link to="/">
                    <button class="error__button error__button--active">
                        回到首页
                    </button>
                </router-link>
            </div>
        </slot>
    </div>
</template>

<script setup></script>

<style lang="scss" scoped>
.ve_404 {
    height: 100vh;
    width: 100vw;
    position: relative;
    overflow: hidden;
    background: linear-gradient(90deg, #2f3640 23%, #181b20 100%);
    background-position: center;
    background-image: url("@/assets/lunbo2.jpg");
    // background-size: cover;
    // background-position: center;
    // background-repeat: no-repeat;
    // min-height: 100vh;
    // width: 100%;
    // display: flex;
    // align-items: center;
    // justify-content: center;
}

.error {
    position: absolute;
    left: 100px;
    top: 400px;
    transform: translateY(-60%);
    font-family: "Righteous", cursive;
    color: #363e49;
    z-index: 1;
}

.error__title {
    font-size: 10em;
    font-weight: bold;
    color: #d0d0d0;
    text-shadow: -5px -5px 0 rgba(0, 0, 0, 0.7);
    background-image: linear-gradient(90deg, #d0d0d0 48%, #919191 100%);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
}

.error__subtitle {
    font-size: 2em;
}

.error__description {
    opacity: 0.8;
    color: #ffffff;
}

.error__button {
    min-width: 7em;
    margin-top: 3em;
    margin-right: 0.5em;
    padding: 0.5em 2em;
    outline: none;
    border: 2px solid #2f3640;
    background-color: transparent;
    border-radius: 8em;
    color: #576375;
    cursor: pointer;
    transition-duration: 0.2s;
    font-size: 0.75em;
    font-family: "Righteous", cursive;
}

.error__button:hover {
    color: #21252c;
}

.error__button--active {
    background-color: $base-color;
    border: 2px solid $base-color;
    color: white;
}

.error__button--active:hover {
    box-shadow: 0px 0px 8px 0px rgba(0, 0, 0, 0.5);
    color: white;
}
</style>
