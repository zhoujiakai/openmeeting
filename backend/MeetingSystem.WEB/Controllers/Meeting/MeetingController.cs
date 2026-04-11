using MeetingSystem.Common.Utils;
using MeetingSystem.IService.Meeting;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;
using Microsoft.AspNetCore.Mvc;

namespace MeetingSystem.WEB.Controllers.Meeting
{
    /// <summary>
    /// 会议信息管理控制器，提供会议信息的增删改查及分页查询功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        /// <summary>
        /// 构造函数，通过依赖注入获取会议服务实例
        /// </summary>
        /// <param name="meetingService">会议服务接口</param>
        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        /// <summary>
        /// 添加会议信息
        /// </summary>
        /// <param name="meetingInfos">会议信息实体</param>
        /// <returns>添加结果</returns>
        [HttpPost]
        public R AddMeetingInfos(MeetingInfos meetingInfos)
            => new R().OK().SetData(_meetingService.AddMeetingInfos(meetingInfos)).SetMessage("添加成功");

        /// <summary>
        /// 根据ID删除会议信息
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns>删除结果</returns>
        [HttpGet]
        public R DeleteMeetingInfos(int id)
            => new R().OK().SetData(_meetingService.DeleteMeetingInfos(id)).SetMessage("删除成功");

        /// <summary>
        /// 更新会议信息
        /// </summary>
        /// <param name="dto">会议信息实体</param>
        /// <returns>更新结果</returns>
        [HttpPost]
        public R UpdateMeetingInfos(MeetingInfos dto)
            => new R().OK().SetData(_meetingService.UpdateMeetingInfos(dto)).SetMessage("更新成功");

        /// <summary>
        /// 根据ID获取单个会议信息
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns>会议信息</returns>
        [HttpGet]
        public R GetMeetingInfos(int id)
            => new R().OK().SetData(_meetingService.GetMeetingInfos(id));

        /// <summary>
        /// 分页获取会议信息列表
        /// </summary>
        /// <param name="page">分页参数</param>
        /// <returns>分页会议数据</returns>
        [HttpPost]
        public R GetMeetingInfosPage(PageParams page)
            => new R().OK().SetData(_meetingService.GetMeetingInfosPage(page));
    }
}
