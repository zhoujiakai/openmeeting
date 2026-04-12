<template>
    <div v-if="!isMeeting">
        <el-form ref="ref_form" :model="form" label-width="80px">
            <el-form-item label="会议编号" prop="roomCode">
                <el-input v-model.trim="roomCode" placeholder="会议编号">
                </el-input>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="enterMeetingRoom"
                    >确认参会</el-button
                >
            </el-form-item>
        </el-form>
    </div>
    <div v-else>
        <div
            style="display: flex; justify-content: center; align-items: center"
        >
            <a style="font-weight: bold"
                >深度学习组会1（会议号：{{ form.roomCode }}）</a
            >
        </div>
        <div class="meeting-main">
            <div class="membersList">
                <!-- 我自己的视频 -->
                <a style="font-weight: bold; text-align: center">
                    （我）{{ form.userName }}
                </a>
                <video
                    id="localVideo"
                    ref="localVideo"
                    playsinline
                    autoplay
                    muted
                    style="width: 100%"
                ></video>
                <!-- 其他用户列表 -->
                <el-table
                    :data="members"
                    style="width: 200px"
                    empty-text="暂无人员参会"
                >
                    <el-table-column prop="userName" label="">
                        <template v-slot="{ row }">
                            <div style="display: flex; flex-direction: column">
                                <a
                                    style="
                                        font-weight: bold;
                                        text-align: center;
                                    "
                                >
                                    （参会者）{{ row.userName }}
                                </a>
                                <video
                                    v-bind:id="row.userId"
                                    playsinline
                                    autoplay
                                    style="width: 100%"
                                ></video>
                            </div>
                        </template>
                    </el-table-column>
                </el-table>
            </div>
            <div class="video-show">
                <div class="desktop-share-container">
                    <video
                        id="desktopVideo"
                        ref="desktopVideo"
                        playsinline
                        autoplay
                        muted
                        style="width: 100%"
                    ></video>
                    <div class="controls">
                        <div>
                            <el-button
                                type="primary"
                                @click="startLocalVideo"
                                :disabled="isVideoOpen"
                            >
                                开启视频
                            </el-button>
                            <el-button
                                type="danger"
                                @click="stopLocalVideo"
                                :disabled="!isVideoOpen"
                            >
                                关闭视频
                            </el-button>
                            <el-button
                                type="primary"
                                id="startSharing"
                                @click="startLocalDesktop"
                            >
                                桌面共享
                            </el-button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="chat-view">
                <el-table
                    :data="chatData"
                    style="width: 100%; max-height: 600px; overflow: auto"
                >
                    <el-table-column width="200px" label="房间消息">
                        <template #default="scope">
                            <div style="white-space: pre-line">
                                {{ scope.row.name }}：<br />{{
                                    scope.row.message
                                }}
                            </div>
                        </template>
                    </el-table-column>
                </el-table>
                <div class="message-input">
                    <textarea
                        v-model="messageText"
                        placeholder="输入您的消息..."
                    ></textarea>
                    <el-button type="primary" @click="sendMessage"
                        >发送</el-button
                    >
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { reactive, toRefs, ref, onBeforeUnmount } from "vue";
import io from "socket.io-client";
import socketConfig from "@/config";
const socketServerUrl = socketConfig.socketServerUrl;
/**
 * --------------------------------------------------------------------------------------------------------
 * 会议前
 */
const form = reactive({
    roomCode: "DSSK-kFSD-uHUR-sdNF",
    userName: sessionStorage.getItem("userName"),
});
const isMeeting = ref(false);
const { roomCode } = toRefs(form);
let socket = null;
const configuration = {
    iceServers: [{ urls: "stun:stun.l.google.com:19302" }],
};
let members = ref([]);
const peerConnections = new Map(); // userId -> RTCPeerConnection，持久化，不会因 members 事件而重建
const isVideoOpen = ref(false);
const messageText = ref("");
const chatData = ref([
    {
        id: "1",
        avatar: "https://via.placeholder.com/100",
        name: "管理员",
        message: "欢迎使用研究生例会管理系统在线会议",
    },
    {
        id: "1",
        avatar: "https://via.placeholder.com/100",
        name: "管理员",
        message: "会议已开始",
    },
    {
        id: "1",
        avatar: "https://via.placeholder.com/100",
        name: "管理员",
        message: "请遵守会议规则，\n文明发言",
    },
]);
const sendMessage = () => {
    if (messageText.value.trim() !== "") {
        // 处理发送消息的逻辑
        socket.emit("chatMessage", {
            roomCode: form.roomCode,
            id: "1",
            avatar: "https://via.placeholder.com/100",
            name: form.userName,
            message: messageText.value.trim(),
        });
        console.log("发送消息:", messageText.value);
        // 通常，您会在这里调用一个API或者执行其他操作来发送消息
        // 发送完毕后，清空输入框
        messageText.value = "";
    }
};
/**
 * --------------------------------------------------------------------------------------------------------
 * 会议中
 */
const a = {};
a["ccc"] = {};
a["ccc"]["ggg"] = "rrr";
console.log(JSON.stringify(a));
const enterMeetingRoom = async () => {
    initSocketIOClient();
    initPeerConnection();
    isMeeting.value = !isMeeting.value;
    // TODO 获取其他人的视频
    // socket.emit("getCount", form.roomCode);
};
/**
 * --------------------------------------------------------------------------------------------------------
 * 信令通信
 */
const initPeerConnection = () => {
    members.value.forEach((member) => {
        // 跳过已存在的连接，避免多人加入时重建连接
        if (peerConnections.has(member.userId)) {
            member.peerConnection = peerConnections.get(member.userId);
            return;
        }
        const pc = new RTCPeerConnection(configuration);
        member.peerConnection = pc;
        peerConnections.set(member.userId, pc);

        pc.addEventListener("track", async (event) => {
            let [remoteStream] = event.streams;
            const videoElement = document.querySelector("#" + member.userId);
            console.log(
                "用户：" +
                    member.userId +
                    "元素：" +
                    JSON.stringify(videoElement),
            );
            videoElement.srcObject = remoteStream;
        });
        pc.addEventListener("icecandidate", (event) => {
            console.log(
                "发送的icecandidate:" + JSON.stringify(event.candidate),
            );
            if (event.candidate) {
                socket.emit("message", {
                    iceCandidate: event.candidate,
                    toUserId: member.userId,
                });
            }
        });
        pc.onconnectionstatechange = (event) => {
            console.log("连接状态改变：" + pc.connectionState);
            if (pc.connectionState === "connected") {
                console.log("连接成功：" + JSON.stringify(event));
            }
            // 连接断开或失败时清理
            if (
                pc.connectionState === "failed" ||
                pc.connectionState === "closed"
            ) {
                peerConnections.delete(member.userId);
            }
        };
    });
};
const initSocketIOClient = () => {
    socket = io(socketServerUrl, {
        query: { ...form },
    }).connect();
    socket.on("countResponse", (count) => {
        console.log(`收到一共有${count}个人`);
    });
    socket.on("message", async (message) => {
        if (message.offer) {
            console.log("收到offer: " + JSON.stringify(message.offer));
            const pc = peerConnections.get(message.fromUserId);
            if (!pc) return;
            const state = pc.signalingState;
            if (state === "stable") {
                // 正常情况：可以接受 offer
                await pc.setRemoteDescription(
                    new RTCSessionDescription(message.offer),
                );
                const answer = await pc.createAnswer();
                await pc.setLocalDescription(answer);
                socket.emit("message", {
                    answer: answer,
                    toUserId: message.fromUserId,
                });
            } else if (state === "have-local-offer") {
                // 冲突(Glare)：双方同时发了 offer，用 userId 较小的一方作为 offerer
                if (socket.id < message.fromUserId) {
                    // 我优先，忽略对方的 offer，等对方收到我的 offer 后回 answer
                    console.log(" glare：忽略对方 offer，本端优先");
                } else {
                    // 对方优先，rollback 我的 offer，接受对方的
                    console.log(" glare：rollback 本端 offer，接受对方");
                    await pc.setLocalDescription({ type: "rollback" });
                    await pc.setRemoteDescription(
                        new RTCSessionDescription(message.offer),
                    );
                    const answer = await pc.createAnswer();
                    await pc.setLocalDescription(answer);
                    socket.emit("message", {
                        answer: answer,
                        toUserId: message.fromUserId,
                    });
                }
            } else {
                console.warn("收到 offer 时信令状态异常: " + state);
            }
        }
        if (message.iceCandidate) {
            console.log(
                "收到iceCandidate: " + JSON.stringify(message.iceCandidate),
            );
            const pc = peerConnections.get(message.fromUserId);
            if (!pc) return;
            try {
                await pc.addIceCandidate(message.iceCandidate);
            } catch (e) {
                console.error("Error adding received ice candidate", e);
            }
        }
        if (message.answer) {
            console.log("收到answer: " + JSON.stringify(message.answer));
            const pc = peerConnections.get(message.fromUserId);
            if (!pc) return;
            // 只在 have-local-offer 状态下接受 answer
            if (pc.signalingState === "have-local-offer") {
                const remoteDesc = new RTCSessionDescription(message.answer);
                await pc.setRemoteDescription(remoteDesc);
            } else {
                console.warn(
                    "收到 answer 时信令状态为 " + pc.signalingState + "，忽略",
                );
            }
        }
    });
    socket.on("members", async (receivedMembers) => {
        members.value = [];
        for (var key in receivedMembers) {
            if (receivedMembers[key].userName !== form.userName) {
                members.value.push(receivedMembers[key]);
            }
        }
        // 只为新增成员创建连接和发起呼叫，避免重建已有连接
        initPeerConnection();
        pushLocalVideo();
        await makeCall();
        console.log("这是所有成员：" + JSON.stringify(members.value));
    });
    socket.on("chatMessage", (chatMessage) => {
        console.log("收到文本消息: " + JSON.stringify(chatMessage));
        chatData.value.push(chatMessage);
    });
    socket.on("screemShare", (fromUserId) => {
        console.log("-------------------------------------------------");
        console.log("获取到桌面共享: " + JSON.stringify(fromUserId));
        members.value.forEach((member) => {
            if (member.userId == fromUserId) {
                if (!member.peerConnection) {
                    member.peerConnection = new RTCPeerConnection(
                        configuration,
                    );
                }
                member.peerConnection.addEventListener(
                    "track",
                    async (event) => {
                        let [remoteStream] = event.streams;
                        const videoElement =
                            document.querySelector("#desktopVideo");
                        console.log(
                            "用户：" +
                                member.userId +
                                "元素：" +
                                JSON.stringify(videoElement),
                        );
                        videoElement.srcObject = remoteStream;
                    },
                );
            }
        });
    });
};
/**
 * --------------------------------------------------------------------------------------------------------
 * 开启本地音视频流
 */
const localVideo = ref(null);
let localStream = null;
const desktopVideo = ref(null);
let desktopStream = null;
const startLocalVideo = async () => {
    try {
        const stream = await navigator.mediaDevices.getUserMedia({
            audio: true,
            video: true,
        });
        localVideo.value.srcObject = stream;
        localStream = stream;
        isVideoOpen.value = true;
        // 把本地流推给对方
        pushLocalVideo();
        await makeCall();
    } catch (e) {
        alert(`getUserMedia() error: ${e.name}`);
    }
};
const pushLocalVideo = () => {
    console.log("还没开启视频");
    if (localStream) {
        console.log("视频开启了");
        localStream.getTracks().forEach((track) => {
            members.value.forEach((member) => {
                if (member.peerConnection) {
                    // 检查是否已经添加过该 track，避免重复添加
                    const senders = member.peerConnection.getSenders();
                    const alreadyAdded = senders.some(
                        (s) => s.track && s.track.id === track.id,
                    );
                    if (!alreadyAdded) {
                        member.peerConnection.addTrack(track, localStream);
                    }
                }
            });
        });
    }
};
const stopLocalVideo = () => {
    isVideoOpen.value = false;
    if (localStream) {
        localStream.getTracks().forEach((track) => {
            track.stop();
        });
        localStream.value.srcObject = null;
    }
};
const startLocalDesktop = async () => {
    if (navigator.mediaDevices.getDisplayMedia) {
        try {
            const displayMediaOptions = { video: true };
            const stream =
                await navigator.mediaDevices.getDisplayMedia(
                    displayMediaOptions,
                );
            desktopVideo.value.srcObject = stream;
            desktopStream = stream;
            pushLocalDesktop();
            await makeCall();
        } catch (error) {
            console.error("无法共享屏幕：", error);
        }
    } else {
        alert("当前浏览器不支持：getDisplayMedia");
    }
};
const pushLocalDesktop = () => {
    console.log("桌面共享流：" + desktopStream);
    if (desktopStream) {
        console.log("开始传输桌面共享");
        desktopStream.getTracks().forEach((track) => {
            members.value.forEach((member) => {
                if (member.peerConnection) {
                    const senders = member.peerConnection.getSenders();
                    const alreadyAdded = senders.some(
                        (s) => s.track && s.track.id === track.id,
                    );
                    if (!alreadyAdded) {
                        member.peerConnection.addTrack(track, desktopStream);
                    }
                }
            });
        });
        socket.emit("screemShare");
        isVideoOpen.value = false;
    }
};
/**
 * --------------------------------------------------------------------------------------------------------
 * 开始视频通话
 */
const makeCall = async () => {
    for (const member of members.value) {
        if (!member.peerConnection) continue;
        // 只对 stable 状态的连接发起新的 offer
        if (member.peerConnection.signalingState !== "stable") continue;
        const offer = await member.peerConnection.createOffer();
        await member.peerConnection.setLocalDescription(offer);
        socket.emit("message", { offer: offer, toUserId: member.userId });
        console.log("这是发送的offer: " + JSON.stringify(offer));
    }
};
/**
 * --------------------------------------------------------------------------------------------------------
 * 离开会议
 */
onBeforeUnmount(() => {
    if (socket) {
        // 退出的时候断开连接
        socket.disconnect();
        console.log("连接已断开。。。");
    }
    // 关闭所有 peer connection
    peerConnections.forEach((pc) => pc.close());
    peerConnections.clear();
});
</script>

<style lang="scss" scoped>
.meeting-main {
    display: flex; /* 使用Flexbox布局 */
    flex-direction: row; /* 设置为横向布局 */
    height: 100vh; /* 使容器高度占满整个视口高度 */
    width: 100vw; /* 使容器宽度占满整个视口宽度 */
}
.video-show {
    width: 900px;
    margin-left: 20px;
    margin-right: 20px;
}
// 共享屏幕
.desktop-share-container {
    position: relative;
    width: 100%;
    height: calc(100vh - 60px); /* 减去底部控制栏的高度 */
}
.controls {
    position: fixed; /* 固定定位，脱离文档流 */
    bottom: 0; /* 定位到页面底部 */
    left: 50%;
    right: auto; /* 确保right属性不会影响left: 50%的定位 */
    transform: translateX(-50%);
    // left: 0;
    // right: 0; /* 宽度设置为100%，横跨整个屏幕 */
    display: flex;
    align-items: center;
    padding: 10px; /* 一些内边距 */
    margin-bottom: 15px;
    // box-sizing: border-box; /* 确保宽度包括padding */
    background-color: #f9f9f9; /* 背景颜色，可以自定义 */
    box-shadow: 0 -2px 4px rgba(0, 0, 0, 0.1); /* 底部阴影效果 */
    // z-index: 1000; /* 确保消息框在页面其他内容之上 */
}
.el-form {
    width: 500px;
    // padding: 200px;
}
.membersList {
    display: flex;
    flex-direction: column;
    width: 200px;
}
.video-show {
    width: 1100px;
}
.chat-view {
    // flex: 1; /* 让每个子元素平分容器的宽度 */
    // overflow: auto; /* 如果内容超出，显示滚动条 */
    // display: flex; /* 使用Flexbox布局 */
    // flex-direction: column; /* 设置为纵向排列 */
    width: 200px;
    height: 600px;
}
.avatar {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    margin-right: 10px;
}

.description {
    color: #666;
    font-size: 14px;
    padding-left: 15px;
}
// 消息输入框
.message-input {
    position: fixed; /* 固定定位，脱离文档流 */
    bottom: 0; /* 定位到页面底部 */
    right: 50px; /* 定位到屏幕右侧 */
    width: 250px; /* 设置宽度为200px */
    display: flex;
    flex-direction: column; /* 设置子元素纵向排列 */
    align-items: center; /* 子元素在交叉轴上居中对齐 */
    padding: 10px; /* 一些内边距 */
    margin-bottom: 15px;
    box-sizing: border-box; /* 确保宽度包括padding */
    background-color: #f9f9f9; /* 背景颜色，可以自定义 */
    box-shadow: 0 -2px 4px rgba(0, 0, 0, 0.1); /* 底部阴影效果 */
    z-index: 1000; /* 确保消息框在页面其他内容之上 */
}
.message-input textarea {
    flex-grow: 1;
    width: 250px;
}
.message-input button {
    margin-top: 10px;
    white-space: nowrap;
}
</style>
