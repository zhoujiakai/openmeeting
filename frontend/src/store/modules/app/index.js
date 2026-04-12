/*
 * @Author: your name
 * @Date: 2021-12-02 16:55:35
 * @LastEditTime: 2021-12-02 16:58:05
 * @LastEditors: Please set LastEditors
 * @Description: 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 * @FilePath: \vue3-element-admin\src\store\modules\app\index.js
 */
import {
    TOGGLE_SLIDER,
    SET_TOKEN,
    SET_USERNAME,
    SET_MENU_LIST,
    SET_PERMISSION_LIST,
} from "./type.js";
export default {
    namespaced: true,
    state: {
        slider: {
            opened: JSON.parse(sessionStorage.getItem("opened")),
        },
        token: sessionStorage.getItem("token") || "",
        menuList: null,
        permissionList: [],
        userName: sessionStorage.getItem("userName") || "",
    },
    mutations: {
        [TOGGLE_SLIDER](state) {
            state.slider.opened = !state.slider.opened;
            sessionStorage.setItem(
                "opened",
                JSON.stringify(state.slider.opened),
            );
        },
        [SET_TOKEN](state, token) {
            state.token = token;
            sessionStorage.setItem("token", state.token);
        },
        [SET_USERNAME](state, userName) {
            state.userName = userName;
            sessionStorage.setItem("userName", state.userName);
        },
        [SET_MENU_LIST](state, menuList) {
            state.menuList = menuList;
        },
        [SET_PERMISSION_LIST](state, permissionList) {
            state.permissionList = permissionList;
        },
    },
    actions: {
        [TOGGLE_SLIDER]({ commit }) {
            commit(TOGGLE_SLIDER);
        },
        [SET_TOKEN]({ commit }, token) {
            commit(SET_TOKEN, token);
        },
        [SET_USERNAME]({ commit }, userName) {
            commit(SET_USERNAME, userName);
        },
        [SET_MENU_LIST]({ commit }, menuList) {
            commit(SET_MENU_LIST, menuList);
        },
        [SET_PERMISSION_LIST]({ commit }, menuList) {
            let allMenus = XE.filterTree(
                menuList,
                (item) => item.menuType == 1,
            );
            let permissionList = [];
            allMenus.forEach((item) => {
                if (item.children && item.children.length > 0) {
                    item.children.forEach((menu) => {
                        permissionList.push(
                            `${item.menuUrl}/${item.id}/${menu.buttonName}`,
                        );
                    });
                }
            });
            commit(SET_PERMISSION_LIST, permissionList);
        },
    },
};
