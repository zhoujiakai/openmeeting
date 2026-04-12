// 所有按钮要跳转的页面
export default [
    // {
    //     path: "/webRtcView",
    //     name: "WebRtcView",
    //     component: () =>
    //         import("@/views/layoutpages/system/components/WebRtcView.vue"),
    // },
    {
        path: "/weeklyReportsView",
        name: "WeeklyReportsView",
        component: () =>
            import("@/views/layoutpages/system/WeeklyReportsView.vue"),
    },
    {
        path: "/meetingInfosView",
        name: "MeetingInfosView",
        component: () =>
            import("@/views/layoutpages/system/MeetingInfosView.vue"),
    },
];
