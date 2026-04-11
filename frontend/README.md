# Frontend

> Based on [vue3-element-admin](https://github.com/asaasa/vue3-element-admin) by [Asa](https://github.com/asaasa), thanks for the great work.

Vue 3 + Element Plus + Vue Router + Vuex

## 启动

```bash
npm install
npm run serve
```

## 项目目录结构

### 简洁版

```
frontend/
├── public/                  # 静态资源
├── src/
│   ├── api/                 # API 接口
│   ├── assets/              # 静态资源（图片等）
│   ├── components/          # 公共组件（布局、仪表盘、评论等）
│   ├── directives/          # 自定义指令（权限、resize）
│   ├── plugins/             # 插件配置（Axios、Element Plus、Mock、权限）
│   ├── router/              # 路由配置
│   ├── store/               # Vuex 状态管理
│   ├── styles/              # 样式文件
│   ├── utils/               # 工具函数
│   ├── views/               # 页面视图（登录、首页、会议室、系统管理等）
│   ├── App.vue              # 根组件
│   ├── config.js            # 全局配置
│   └── main.js              # 入口文件
├── vue.config.js
└── package.json
```

### 详细版

```
frontend/
├── public/                          # 静态资源
│   ├── favicon.ico
│   └── index.html
├── src/
│   ├── api/                         # API 接口
│   │   ├── index.js                 # API 入口
│   │   ├── mock-server.js           # Mock 服务
│   │   └── modules/
│   │       └── system.js            # 系统相关接口
│   ├── assets/                      # 静态资源（图片等）
│   ├── components/                  # 公共组件
│   │   ├── comment/                 # 评论组件
│   │   │   ├── CommentList.vue
│   │   │   └── Comments.vue
│   │   ├── dashboard/               # 仪表盘组件
│   │   │   ├── LiveChart.vue
│   │   │   └── Shortcuts.vue
│   │   ├── layout/                  # 布局组件
│   │   │   ├── NavigateBar.vue      # 顶部导航栏
│   │   │   ├── SideBar.vue          # 侧边栏
│   │   │   └── components/
│   │   │       ├── Breadcrumb.vue   # 面包屑
│   │   │       ├── Hamburger.vue
│   │   │       ├── Logo.vue
│   │   │       ├── Personal.vue     # 个人信息
│   │   │       └── SlideMenu.vue    # 滑动菜单
│   │   ├── veBaseComponents/        # 基础组件封装
│   │   │   ├── index.js
│   │   │   └── VeTable.vue
│   │   ├── Common.vue               # 公共组件
│   │   └── FunctionPage.vue         # 功能页面组件
│   ├── directives/                  # 自定义指令
│   │   ├── index.js
│   │   └── modules/
│   │       ├── permission.js         # 权限指令
│   │       └── resize.js             # 尺寸调整指令
│   ├── plugins/                     # 插件配置
│   │   ├── axios.js                 # Axios 配置
│   │   ├── element.js               # Element Plus 配置
│   │   ├── mock.js                  # Mock 数据配置
│   │   ├── permission.js            # 路由权限控制
│   │   └── svgicon.js               # SVG 图标配置
│   ├── router/                      # 路由配置
│   │   ├── index.js                 # 路由入口
│   │   ├── globalRoutes.js          # 全局路由
│   │   ├── mainRoutes.js            # 主路由
│   │   └── buttonRoutes.js          # 按钮路由
│   ├── store/                       # Vuex 状态管理
│   │   ├── index.js                 # Store 入口
│   │   ├── getters.js               # Getters
│   │   └── modules/
│   │       └── app/                 # 应用状态模块
│   │           ├── index.js
│   │           └── type.js
│   ├── styles/                      # 样式文件
│   │   ├── common.scss              # 公共样式
│   │   └── variables.scss.js        # SCSS 变量
│   ├── utils/                       # 工具函数
│   │   ├── index.js
│   │   └── validate.js              # 验证工具
│   ├── views/                       # 页面视图
│   │   ├── Home.vue                 # 首页
│   │   ├── Login.vue                # 登录页
│   │   ├── AppMain.vue              # 主内容区
│   │   ├── IFrame.vue               # 内嵌页
│   │   ├── 404.vue                  # 404 页面
│   │   └── layoutpages/             # 布局页面
│   │       ├── common.js
│   │       ├── ossFileHelper.js     # OSS 文件辅助
│   │       ├── leisure/
│   │       │   └── Game.vue         # 休闲游戏
│   │       └── system/              # 系统管理页面
│   │           ├── ChangePassword.vue
│   │           ├── Groups.vue        # 分组管理
│   │           ├── MeetingInfos.vue  # 会议信息
│   │           ├── MeetingInfosView.vue
│   │           ├── MeetingReports.vue  # 会议报告
│   │           ├── MeetingRoom.vue   # 会议室（WebRTC 视频会议）
│   │           ├── Menus.vue         # 菜单管理
│   │           ├── Roles.vue         # 角色管理
│   │           ├── Users.vue         # 用户管理
│   │           ├── UserTable.vue
│   │           ├── WeeklyReports.vue  # 周报
│   │           ├── WeeklyReportsView.vue
│   │           └── components/       # 系统管理子组件
│   │               ├── GroupsEdit.vue
│   │               ├── MeetingInfosEdit.vue
│   │               ├── MeetingReportEdit.vue
│   │               ├── MenuEdit.vue
│   │               ├── RoleEdit.vue
│   │               ├── UsersEdit.vue
│   │               ├── UsersEditRoute.vue
│   │               └── WeeklyReportEdit.vue
│   ├── App.vue                      # 根组件
│   ├── config.js                    # 全局配置
│   └── main.js                      # 入口文件
├── .browserslistrc
├── .eslintrc.js
├── .gitignore
├── .npmrc
├── babel.config.js
├── jsconfig.json
├── package.json
├── vue.config.js
└── LICENSE
```
