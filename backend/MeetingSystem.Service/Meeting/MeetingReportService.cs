using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.Meeting;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;
using MeetingSystem.Service.Base;

namespace MeetingSystem.Service.Meeting
{
    /// <summary>
    /// 会议报告服务，提供会议报告的增删改查及分页查询功能。
    /// 支持按分组名称和会议ID进行筛选，并自动关联查询会议标题。
    /// </summary>
    public class MeetingReportService : BaseService, IMeetingReportService
    {
        /// <summary>
        /// 构造函数，注入数据库上下文
        /// </summary>
        /// <param name="context">会议系统数据库上下文</param>
        public MeetingReportService(MeetingSystemDbContext context) : base(context) { }

        /// <summary>
        /// 添加会议报告
        /// </summary>
        /// <param name="dto">会议报告 DTO 对象</param>
        /// <returns>始终返回 0</returns>
        public int AddMeetingReport(MeetingReportsDto dto)
        {
            var entity = ToEntity(dto); // DTO 转换为实体对象
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 更新会议报告
        /// </summary>
        /// <param name="dto">会议报告 DTO 对象</param>
        /// <returns>始终返回 0</returns>
        public int UpdateMeetingReport(MeetingReportsDto dto)
        {
            var entity = ToEntity(dto); // DTO 转换为实体对象
            _dbContext.Update(entity);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 根据ID获取会议报告详情，自动关联查询对应的会议标题
        /// </summary>
        /// <param name="id">报告ID</param>
        /// <returns>会议报告 DTO 对象，未找到则返回 null</returns>
        public MeetingReportsDto? GetMeetingReports(int id)
        {
            var entity = _dbContext.MeetingReports.Find(id);
            if (entity == null) return null;
            var dto = ToDto(entity);

            // 关联查询会议信息表获取会议标题
            dto.MeetingTitle = _dbContext.MeetingInfos.FirstOrDefault(b => b.Id == entity.MeetingId)?.Title ?? "";
            return dto;
        }

        /// <summary>
        /// 获取会议报告分页列表，支持按分组名称和会议ID筛选。
        /// 采用批量加载会议标题的方式避免 N+1 查询问题。
        /// </summary>
        /// <param name="page">分页参数，包含筛选条件和分页信息</param>
        /// <returns>包含报告列表和总数的分页结果 DTO</returns>
        public PageDto GetMeetingReportsPage(PageParams page)
        {
            // 构建查询条件：根据分组名称和会议ID动态筛选
            var query = _dbContext.Set<MeetingReports>().Where(a =>
                    (!string.IsNullOrEmpty(page.GroupName) ? a.GroupName == page.GroupName : true)
                    && (page.MeetingId != 0 ? a.MeetingId == page.MeetingId : true)
                );

            var total = query.Count(); // 总记录数
            var paged = query.OrderBy(a => a.Id).Skip((page.Page - 1) * page.Limit).Take(page.Limit).ToList(); // 分页查询

            // 批量加载会议标题，避免 N+1 查询性能问题
            var meetingIds = paged.Select(a => a.MeetingId).Distinct().ToList();
            var meetingTitles = _dbContext.MeetingInfos
                .Where(m => meetingIds.Contains(m.Id))
                .ToDictionary(m => m.Id, m => m.Title);

            // 将实体转换为 DTO 并填充会议标题
            var list = paged.Select(a =>
            {
                var dto = ToDto(a);
                dto.MeetingTitle = meetingTitles.GetValueOrDefault(a.MeetingId, "");
                return dto;
            }).ToList();

            return new PageDto().SetList(list).SetTotal(total);
        }

        /// <summary>
        /// 将 DTO 转换为实体对象
        /// </summary>
        /// <param name="dto">会议报告 DTO</param>
        /// <returns>会议报告实体对象</returns>
        private static MeetingReports ToEntity(MeetingReportsDto dto) => new()
        {
            Id = dto.Id, Title = dto.Title, MeetingId = dto.MeetingId,
            GroupName = dto.GroupName, FileUrl = dto.FileUrl, UserName = dto.UserName,
            UploadDate = dto.UploadDate, DownloadCount = dto.DownloadCount, Status = dto.Status,
        };

        /// <summary>
        /// 将实体对象转换为 DTO
        /// </summary>
        /// <param name="entity">会议报告实体对象</param>
        /// <returns>会议报告 DTO</returns>
        private static MeetingReportsDto ToDto(MeetingReports entity) => new()
        {
            Id = entity.Id, Title = entity.Title, MeetingId = entity.MeetingId,
            GroupName = entity.GroupName, FileUrl = entity.FileUrl, UserName = entity.UserName,
            UploadDate = entity.UploadDate, DownloadCount = entity.DownloadCount, Status = entity.Status,
        };
    }
}
