const { createServer } = require("http");
const { Server } = require("socket.io");

function startSocketServer(port = 3001) {
    const httpServer = createServer();
    const io = new Server(httpServer, {
        cors: {
            origin: "*",
        },
    });

    // 维护房间列表中的用户列表
    let rooms = {};

    // 监听连接事件
    io.on("connection", (socket) => {
        const { query } = socket.handshake;
        const { userName, roomCode } = query;
        console.log(
            `[Socket] 姓名：${userName}，房间：${roomCode}，用户Id：${socket.id} 进入了房间`
        );
        let user = {
            userId: socket.id,
            userName: userName,
            offer: "",
            candidates: [],
        };
        // 没有房间先创建房间
        if (!rooms.hasOwnProperty(roomCode)) {
            rooms[roomCode] = {};
        }
        // 把这个人加入房间
        rooms[roomCode][socket.id] = user;
        socket.join(roomCode);
        // 每次有新用户加入房间，向房间内发送用户列表对象
        io.to(roomCode).emit("members", rooms[roomCode]);

        socket.on("disconnect", (reason) => {
            let roomCode;
            for (var key in rooms) {
                for (var key2 in rooms[key]) {
                    if (key2 === socket.id) {
                        roomCode = key;
                    }
                }
            }
            if (roomCode && rooms[roomCode] && rooms[roomCode][socket.id]) {
                console.log(
                    `[Socket] 姓名：${rooms[roomCode][socket.id].userName}，房间：${roomCode}，用户Id：${socket.id} 离开了房间`
                );
            }
            for (var key in rooms) {
                if (key === roomCode) {
                    let newMembers = {};
                    for (var key2 in rooms[key]) {
                        if (key2 !== socket.id) {
                            newMembers[key2] = rooms[key][key2];
                        }
                    }
                    rooms[key] = newMembers;
                }
            }
            if (roomCode) {
                socket.broadcast.emit("members", rooms[roomCode]);
            }
        });

        // 客户端点击按钮时，服务器响应
        socket.on("getCount", (room) => {
            const count = io.engine.clientsCount;
            console.log(
                `[Socket] 回复${JSON.stringify(room)}房间一共有${count}个人`
            );
            io.to(room).emit("countResponse", count);
        });

        // 全部消息（WebRTC 信令）
        socket.on("message", (message) => {
            message.fromUserId = socket.id;
            socket.to(message.toUserId).emit("message", message);
        });

        // 聊天消息
        socket.on("chatMessage", (chatMessage) => {
            io.to(chatMessage.roomCode).emit("chatMessage", chatMessage);
        });

        // 屏幕共享
        socket.on("screemShare", () => {
            socket.broadcast.emit("screemShare", socket.id);
        });
    });

    httpServer.listen(port, () => {
        console.log(`[Socket] Socket.IO server is running on port ${port}`);
    });

    return httpServer;
}

module.exports = { startSocketServer };
