using MeetingSystem.Common.Utils;
using MeetingSystem.IService.Meeting;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;
using Microsoft.AspNetCore.Mvc;

namespace MeetingSystem.WEB.Controllers.MeetingReport
{
    /// <summary>
    /// 会议纪要管理控制器，提供会议纪要的增删改查及分页查询功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MeetingReportController : ControllerBase
    {
        private readonly IMeetingReportService _meetingReportService;

        /// <summary>
        /// 构造函数，通过依赖注入获取会议纪要服务实例
        /// </summary>
        /// <param name="meetingReportService">会议纪要服务接口</param>
        public MeetingReportController(IMeetingReportService meetingReportService)
        {
            _meetingReportService = meetingReportService;
        }

        /// <summary>
        /// 添加会议纪要
        /// </summary>
        /// <param name="meetingReports">会议纪要信息</param>
        /// <returns>添加结果</returns>
        [HttpPost]
        public R AddMeetingReport(MeetingReportsDto meetingReports)
            => new R().OK().SetData(_meetingReportService.AddMeetingReport(meetingReports)).SetMessage("添加成功");

        /// <summary>
        /// 根据ID删除会议纪要
        /// </summary>
        /// <param name="id">会议纪要ID</param>
        /// <returns>删除结果</returns>
        [HttpGet]
        public R DeleteMeetingReport(int id)
        {
            _meetingReportService.Delete(new MeetingReports(id));
            return new R().OK().SetData(0);
        }

        /// <summary>
        /// 更新会议纪要信息
        /// </summary>
        /// <param name="dto">会议纪要信息</param>
        /// <returns>更新结果</returns>
        [HttpPost]
        public R UpdateMeetingReport(MeetingReportsDto dto)
        {
            _meetingReportService.UpdateMeetingReport(dto);
            return new R().OK().SetMessage("更新成功");
        }

        /// <summary>
        /// 根据ID获取单个会议纪要
        /// </summary>
        /// <param name="id">会议纪要ID</param>
        /// <returns>会议纪要详情</returns>
        [HttpGet]
        public R GetMeetingReport(int id)
            => new R().OK().SetData(_meetingReportService.GetMeetingReports(id)!);

        /// <summary>
        /// 分页获取会议纪要列表
        /// </summary>
        /// <param name="pageParams">分页参数</param>
        /// <returns>分页会议纪要数据</returns>
        [HttpPost]
        public R GetMeetingReportPage(PageParams pageParams)
            => new R().OK().SetData(_meetingReportService.GetMeetingReportsPage(pageParams));
    }
}
