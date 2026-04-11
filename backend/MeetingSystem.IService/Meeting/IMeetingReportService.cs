using MeetingSystem.IService.Base;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;

namespace MeetingSystem.IService.Meeting
{
    /// <summary>
    /// 会议报告服务接口，提供会议报告的增删改查及分页查询功能
    /// </summary>
    public interface IMeetingReportService : IBaseService
    {
        /// <summary>
        /// 添加会议报告
        /// </summary>
        /// <param name="dto">会议报告数据传输对象</param>
        /// <returns>受影响的行数</returns>
        public int AddMeetingReport(MeetingReportsDto dto);

        /// <summary>
        /// 更新会议报告
        /// </summary>
        /// <param name="dto">会议报告数据传输对象</param>
        /// <returns>受影响的行数</returns>
        public int UpdateMeetingReport(MeetingReportsDto dto);

        /// <summary>
        /// 根据ID获取会议报告
        /// </summary>
        /// <param name="id">会议报告ID</param>
        /// <returns>会议报告数据传输对象，未找到时返回null</returns>
        public MeetingReportsDto? GetMeetingReports(int id);

        /// <summary>
        /// 分页查询会议报告列表
        /// </summary>
        /// <param name="pageParams">分页参数</param>
        /// <returns>分页结果</returns>
        public PageDto GetMeetingReportsPage(PageParams pageParams);
    }
}
