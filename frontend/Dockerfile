# Frontend Dockerfile
# 开发模式运行，同时包含 Socket.IO 信令服务器

FROM node:20-alpine
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

ENV VUE_APP_API_TARGET=http://backend:8080

EXPOSE 8086 3001

CMD ["npm", "run", "serve"]
