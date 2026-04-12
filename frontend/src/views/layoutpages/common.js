/*
 * @Author: your name
 * @Date: 2021-02-07 17:11:28
 * @LastEditTime: 2021-08-18 17:51:13
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \vue3-element-admin\src\views\layoutpages\common.js
 */

/**
 * 计算文件的哈希值
 */
export const fileToHash = (file) => {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => {
            const arrayBuffer = reader.result;
            const buffer = new Uint8Array(arrayBuffer);
            window.crypto.subtle.digest("SHA-256", buffer).then((hash) => {
                const hashArray = Array.from(new Uint8Array(hash));
                const hashHex = hashArray
                    .map((b) => b.toString(16).padStart(2, "0"))
                    .join("");
                resolve(hashHex);
            });
        };
        reader.onerror = () => reject(reader.error);
        reader.readAsArrayBuffer(file);
    });
};

/**
 * 文件上传
 */
export const uploadFile = async (event, showProgress, progressPercent) => {
    progressPercent.value = 0;
    showProgress.value = true;
    const file = event.target.files[0];
    let hash = await fileToHash(file);

    let formData = new FormData();
    formData.append("hash", hash);
    formData.append("size", file.size);
    const { data } = await VE_API.system.queryChunksTable(formData, {
        Global: false,
    });
    if (data.fileUrl != "") {
        // 文件已经存在
        progressPercent.value = 100;
        return data.fileUrl;
    } else {
        // 文件不存在或存在分片，分片传输
        let fileUrl2 = await sendFileInChunks(
            hash,
            data.chunksTable,
            file,
            progressPercent,
        );
        console.log("怎么没了？" + fileUrl2);
        return fileUrl2;
    }
};
/**
 * 上传分片
 */
export const sendFileInChunks = async (
    hash,
    chunksTable,
    file,
    progressPercent,
) => {
    return new Promise((resolve) => {
        const chunkSize = 1024 * 1024; // 1MB
        const totalChunks = Math.ceil(file.size / chunkSize);
        let _fileUrl = "";
        while (chunksTable != null && chunksTable.length > 0) {
            let newChunksTable;
            chunksTable.forEach(async (i) => {
                // 执行分片操作
                const start = i * chunkSize;
                const end = Math.min(start + chunkSize, file.size);
                const chunk = file.slice(start, end);
                // const tempType = file.type.split("/");
                // const fileType = tempType[tempType.length - 1];
                // 上传分片
                let formData = new FormData();
                formData.append("hash", hash);
                formData.append("file", chunk);
                formData.append("index", i);
                formData.append("totalChunks", totalChunks);
                // formData.append("fileType", fileType);
                formData.append("filename", file.name);
                console.log(i);
                // 发送请求到后端
                const response = await VE_API.system.uploadFile(formData, {
                    Global: false,
                });
                newChunksTable = response.data.chunksTable;
                if (response.data.fileUrl != "") {
                    progressPercent.value = 100;
                    _fileUrl = response.data.fileUrl;
                    console.log("新文件路径：" + _fileUrl);
                    resolve(_fileUrl);
                } else if (
                    newChunksTable != undefined &&
                    newChunksTable.length > 0
                ) {
                    progressPercent.value =
                        Math.ceil(
                            (1 - newChunksTable.length / totalChunks) * 100,
                        ) + 1;
                }
            });
            chunksTable = newChunksTable;
        }
    });
};
/**
 * 文件下载
 */
export const downloadFile = async (
    fileUrl,
    id,
    currentClickRow,
    showProgress,
    progressPercent,
) => {
    currentClickRow.value = id;
    showProgress.value = !showProgress.value;
    const formData = new FormData();
    formData.append("fileUrl", fileUrl);
    console.log(JSON.stringify(formData));
    // 获取文件的总片数
    const response = await VE_API.system.chunkCount(formData);
    const totalChunks = response.data;
    console.log("278  一共有" + totalChunks);
    // 创建一个空的Blob对象来存储文件
    let file = "";

    // 并发下载所有文件分片
    for (let i = 0; i < totalChunks; i++) {
        progressPercent.value = Math.ceil((i / totalChunks) * 100) + 1;
        let chunkForm = new FormData();
        chunkForm.append("i", i);
        chunkForm.append("totalChunks", totalChunks);
        chunkForm.append("fileUrl", fileUrl);
        let response = await VE_API.system.downloadFile(chunkForm, {
            responseType: "blob",
            Global: false,
        });
        console.log(response);
        file = new Blob([file, response]);
    }

    // 保存文件
    let url = window.URL.createObjectURL(file);
    let a = document.createElement("a");
    a.href = url;
    let parts = fileUrl.split("\\");
    let filename = parts.pop();
    console.log("文件名：" + filename);
    a.download = filename;
    a.click();
    showProgress.value = !showProgress.value;
    progressPercent.value = 0;
};
/**
 * @description:提交搜索
 * @param {*}
 * @return {*}
 */
export const onSubmit = (params, getDataList) => {
    params.limit = 10;
    params.page = 1;
    getDataList();
};
/**
 * @description:重置
 * @param {*}
 * @return {*}
 */
export const resetForm = (queryForm, params, getDataList) => {
    queryForm.resetFields();
    onSubmit(params, getDataList);
};
/**
 * @description:每页条数事件
 * @param {*}
 * @return {*}
 */
export const handleSizeChange = (val, params, getDataList) => {
    params.page = 1;
    params.limit = val;
    getDataList();
};
/**
 * @description:改变页数事件
 * @param {*}
 * @return {*}
 */
export const handleCurrentChange = (val, params, getDataList) => {
    params.page = val;
    getDataList();
};

/**
 * @description: 获取按钮跳转菜单的路径
 * @param {btnName} 跳转按钮的key值
 * @param {toPathUrl} 需要跳转到的菜单的路径 该路径为layoutpages下的文件子路径
 * @param {pathId} 当前页面的路由id
 * @param {menuList} 所有注册过的路由列表
 * @param {proxy} vue实例
 * @return {name} 跳转路由的name值
 */
export const findName = (btnName, toPathUrl, pathId, menuList, proxy) => {
    let toId = "";
    let _item = XE.findTree(menuList, (item) => item.id == pathId);
    if (
        _item &&
        _item.item &&
        _item.item.children &&
        _item.item.children.length > 0
    ) {
        let btn = _item.item.children.find((item) => item.menu == btnName);

        btn && (toId = btn.toPath);
    }
    if (toId != "") {
        let _toItem = XE.findTree(menuList, (item) => item.id == toId);
        if (_toItem && _toItem.item) {
            if (_toItem.item.iframe == 0) {
                if (_toItem.item.url == toPathUrl) {
                    return `${toPathUrl.replace(/\//g, "-")}-${toId}`;
                }
            } else {
                return `i-${toId}`;
            }
        }
    }
    proxy.$message({
        type: "error",
        message: "无法跳转,请联系系统管理员!",
    });
};

/**
 * @description:根据权限动态添加路由
 * @param {title} 标题名称
 * @param {path} 组件路径 layoutpages下的组件路径
 * @param {name} 按钮key值
 * @param {{ router, route }} 路由对象
 * @return {_route.name} 返回注册后的name值
 */
export const getAsyncRouteName = async (
    title,
    rowData,
    path,
    name,
    { router, route },
) => {
    const FunctionPage = require("@/components/FunctionPage.vue").default;
    const AsyncComponent = require(
        "@/views/layoutpages/" + path + ".vue",
    ).default;
    const app = {
        components: {
            FunctionPage,
            AsyncComponent,
        },
        data: () => ({
            rName: null,
        }),
        methods: {
            reload(e) {
                return (e.returnValue = "");
            },
        },
        mounted() {
            this.rName = this.$route.name;
            window.addEventListener("beforeunload", this.reload);
        },
        beforeUnmount() {
            window.removeEventListener("beforeunload", this.reload);
            this.$router.removeRoute(this.rName);
        },
        render() {
            return (
                <function-page title={title}>
                    <async-component rowData={rowData} title={title} />
                </function-page>
            );
        },
    };
    const _route = {
        name: route.name + "/" + name,
        path: route.name + "/" + name,

        component: app,
    };
    await router.addRoute("AppMain", _route);
    return _route.name;
};
export const getAsyncViewRouteName = async (myName, { router }) => {
    const _route1 = {
        name: myName,
        path: myName,
        component: () => import("@/views/layoutpages/" + myName + ".vue"),
    };
    await router.addRoute("AppMain", _route1);
    return _route1.name;
};
export const getAsyncRouteName1 = async (
    title,
    rowData,
    path,
    name,
    { router, route },
) => {
    const AsyncComponent = require(
        "@/views/layoutpages/" + path + ".vue",
    ).default;
    const app = {
        components: {
            AsyncComponent,
        },
        data: () => ({
            rName: null,
        }),
        methods: {
            reload(e) {
                return (e.returnValue = "");
            },
        },
        mounted() {
            this.rName = this.$route.name;
            window.addEventListener("beforeunload", this.reload);
        },
        beforeUnmount() {
            window.removeEventListener("beforeunload", this.reload);
            // this.$router.removeRoute(this.rName);
        },
        render() {
            return <async-component />;
        },
    };
    const _route = {
        name: route.name + "/" + name,
        path: route.name + "/" + name,
        component: app,
    };
    await router.addRoute("AppMain", _route);
    return _route.name;
};
