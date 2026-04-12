using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.Meeting;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;
using MeetingSystem.Service.Base;

namespace MeetingSystem.Service.Meeting
{
    /// <summary>
    /// 会议信息服务，提供会议信息的增删改查及分页查询功能。
    /// 支持按时间范围、关键词和分组名称进行筛选。
    /// </summary>
    public class MeetingService : BaseService, IMeetingService
    {
        /// <summary>
        /// 构造函数，注入数据库上下文
        /// </summary>
        /// <param name="dbContext">会议系统数据库上下文</param>
        public MeetingService(MeetingSystemDbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// 添加会议信息。若会议类型为2（需要邀请码），则自动生成唯一邀请码。
        /// </summary>
        /// <param name="meetingInfos">会议信息实体对象</param>
        /// <returns>始终返回 0</returns>
        public int AddMeetingInfos(MeetingInfos meetingInfos)
        {
            // 会议类型为2时生成唯一邀请码
            if (meetingInfos.MeetingType == 2)
                meetingInfos.Code = Guid.NewGuid().ToString();
            _dbContext.Add(meetingInfos);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 根据ID删除会议信息
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns>始终返回 0，会议不存在也返回 0</returns>
        public int DeleteMeetingInfos(int id)
        {
            var entity = _dbContext.MeetingInfos.Find(id);
            if (entity == null) return 0;
            _dbContext.MeetingInfos.Remove(entity);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 根据ID获取会议信息
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns>会议信息实体对象，未找到则返回包含默认ID的空对象</returns>
        public MeetingInfos GetMeetingInfos(int id)
        {
            return _dbContext.MeetingInfos.Find(id) ?? new MeetingInfos { Id = id };
        }

        /// <summary>
        /// 获取会议信息分页列表，支持按时间范围、关键词和分组名称筛选。
        /// </summary>
        /// <param name="page">分页参数，包含时间范围、关键词、分组名称等筛选条件</param>
        /// <returns>包含会议信息列表和总数的分页结果 DTO</returns>
        public PageDto GetMeetingInfosPage(PageParams page)
        {
            // 构建查询条件：根据传入参数动态筛选
            var query = _dbContext.MeetingInfos
                .Where(a => page.MeetingDateTimeRange != null ? page.MeetingDateTimeRange[0] < a.StartTime : true) // 开始时间筛选
                .Where(a => (page.MeetingDateTimeRange != null && page.MeetingDateTimeRange.Count > 1) ? a.EndTime < page.MeetingDateTimeRange[1] : true) // 结束时间筛选
                .Where(a => !string.IsNullOrEmpty(page.MeetingKeywords) ?
                    a.Keywords.Contains(page.MeetingKeywords) || a.Title.Contains(page.MeetingKeywords) : true) // 关键词在标题或关键词字段中模糊匹配
                .Where(a => !string.IsNullOrEmpty(page.GroupName) ? a.GroupName == page.GroupName : true); // 按分组名称精确筛选

            var total = query.Count(); // 总记录数
            var list = query.OrderBy(a => a.Id).Skip((page.Page - 1) * page.Limit).Take(page.Limit).ToList(); // 分页查询
            return new PageDto().SetList(list).SetTotal(total);
        }

        /// <summary>
        /// 更新会议信息。若会议类型为2（需要邀请码），则重新生成唯一邀请码。
        /// </summary>
        /// <param name="meetingInfos">会议信息实体对象</param>
        /// <returns>始终返回 0</returns>
        public int UpdateMeetingInfos(MeetingInfos meetingInfos)
        {
            // 会议类型为2时重新生成唯一邀请码
            if (meetingInfos.MeetingType == 2)
                meetingInfos.Code = Guid.NewGuid().ToString();
            _dbContext.Update(meetingInfos);
            _dbContext.SaveChanges();
            return 0;
        }
    }
}
