using MeetingSystem.IService.Base;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;

namespace MeetingSystem.IService.Meeting
{
    /// <summary>
    /// 会议信息服务接口，提供会议信息的增删改查及分页查询功能
    /// </summary>
    public interface IMeetingService : IBaseService
    {
        /// <summary>
        /// 根据ID获取会议信息
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns>会议信息</returns>
        MeetingInfos GetMeetingInfos(int id);

        /// <summary>
        /// 更新会议信息
        /// </summary>
        /// <param name="dto">会议信息</param>
        /// <returns>受影响的行数</returns>
        int UpdateMeetingInfos(MeetingInfos dto);

        /// <summary>
        /// 添加会议信息
        /// </summary>
        /// <param name="dto">会议信息</param>
        /// <returns>受影响的行数</returns>
        int AddMeetingInfos(MeetingInfos dto);

        /// <summary>
        /// 根据ID删除会议信息
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns>受影响的行数</returns>
        int DeleteMeetingInfos(int id);

        /// <summary>
        /// 分页查询会议信息列表
        /// </summary>
        /// <param name="page">分页参数</param>
        /// <returns>分页结果</returns>
        PageDto GetMeetingInfosPage(PageParams page);
    }
}
