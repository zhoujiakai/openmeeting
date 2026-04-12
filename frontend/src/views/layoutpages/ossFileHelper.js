/**
 * 使用阿里云对象存储做断点上传
 * OSS 配置通过 .env 文件中的 VUE_APP_OSS_* 变量注入
 */
export const ossFileUpload = async (filename, filePath) => {
    const OSS = require("ali-oss");

    const client = new OSS({
        region: process.env.VUE_APP_OSS_REGION,
        accessKeyId: process.env.VUE_APP_OSS_ACCESS_KEY_ID,
        accessKeySecret: process.env.VUE_APP_OSS_ACCESS_KEY_SECRET,
        bucket: process.env.VUE_APP_OSS_BUCKET,
    });
    // yourfilepath填写已上传文件所在的本地路径。
    // const filePath = "yourfilepath";
    let checkpoint;
    async function resumeUpload() {
        // 重试五次。
        for (let i = 0; i < 5; i++) {
            try {
                console.log(filePath + filename);
                const result = await client.multipartUpload(
                    filename,
                    filePath,
                    {
                        checkpoint,
                        async progress(percentage, cpt) {
                            checkpoint = cpt;
                        },
                    },
                );
                console.log(result);
                break; // 跳出当前循环。
            } catch (e) {
                console.log(e);
            }
        }
    }

    resumeUpload();
};

/**
 * 断点下载/流式下载
 */
// const ossFileDownload = async () =>{
//     const OSS = require("ali-oss");
//     const fs = require("fs");
//     const client = new OSS({
//         region: process.env.VUE_APP_OSS_REGION,
//         accessKeyId: process.env.VUE_APP_OSS_ACCESS_KEY_ID,
//         accessKeySecret: process.env.VUE_APP_OSS_ACCESS_KEY_SECRET,
//         bucket: process.env.VUE_APP_OSS_BUCKET,
//     });

//     async function getStream () {
//         try {
//             // 填写Object完整路径。Object完整路径中不能包含Bucket名称。
//             const result = await client.getStream("exampleobject.txt");
//             console.log(result);
//             // 填写本地文件的完整路径。如果指定的本地文件存在会覆盖，不存在则新建。
//             // 如果未指定本地路径，则下载后的文件默认保存到示例程序所属项目对应本地路径中。
//             const writeStream = fs.createWriteStream("D:\\localpath\\examplefile.txt");
//             result.stream.pipe(writeStream);
//         } catch (e) {
//             console.log(e);
//         }
//     }
//     getStream()
// }
