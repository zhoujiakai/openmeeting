/*
 * @Author: your name
 * @Date: 2021-01-06 17:24:12
 * @LastEditTime: 2021-11-30 15:34:54
 * @LastEditors: Please set LastEditors
 * @Description: 修改配置需重启服务后生效
 * @FilePath: \vue3-element-admin\src\config.js
 */
module.exports = {
    dev_mock: false, //开发环境启用mock true:启用
    pro_mock: true, //生产环境启用mock true:启用
    socketServerUrl: "http://localhost:3001", // Socket.IO 信令服务器地址
    // resolve: {
    //     fallback: {
    //         util: false,
    //     },
    // },
};
