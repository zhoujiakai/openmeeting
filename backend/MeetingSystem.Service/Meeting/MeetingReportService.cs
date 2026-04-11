using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.Meeting;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;
using MeetingSystem.Service.Base;

namespace MeetingSystem.Service.Meeting
{
    public class MeetingReportService : BaseService, IMeetingReportService
    {
        public MeetingReportService(MeetingSystemDbContext context) : base(context) { }

        public int AddMeetingReport(MeetingReportsDto dto)
        {
            var entity = ToEntity(dto);
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
            return 0;
        }

        public int UpdateMeetingReport(MeetingReportsDto dto)
        {
            var entity = ToEntity(dto);
            _dbContext.Update(entity);
            _dbContext.SaveChanges();
            return 0;
        }

        public MeetingReportsDto? GetMeetingReports(int id)
        {
            var entity = _dbContext.MeetingReports.Find(id);
            if (entity == null) return null;
            var dto = ToDto(entity);
            dto.MeetingTitle = _dbContext.MeetingInfos.FirstOrDefault(b => b.Id == entity.MeetingId)?.Title ?? "";
            return dto;
        }

        public PageDto GetMeetingReportsPage(PageParams page)
        {
            var query = _dbContext.Set<MeetingReports>().Where(a =>
                    (!string.IsNullOrEmpty(page.GroupName) ? a.GroupName == page.GroupName : true)
                    && (page.MeetingId != 0 ? a.MeetingId == page.MeetingId : true)
                );

            var total = query.Count();
            var paged = query.Skip((page.Page - 1) * page.Limit).Take(page.Limit).ToList();

            // Batch load meeting titles to avoid N+1
            var meetingIds = paged.Select(a => a.MeetingId).Distinct().ToList();
            var meetingTitles = _dbContext.MeetingInfos
                .Where(m => meetingIds.Contains(m.Id))
                .ToDictionary(m => m.Id, m => m.Title);

            var list = paged.Select(a =>
            {
                var dto = ToDto(a);
                dto.MeetingTitle = meetingTitles.GetValueOrDefault(a.MeetingId, "");
                return dto;
            }).ToList();

            return new PageDto().SetList(list).SetTotal(total);
        }

        private static MeetingReports ToEntity(MeetingReportsDto dto) => new()
        {
            Id = dto.Id, Title = dto.Title, MeetingId = dto.MeetingId,
            GroupName = dto.GroupName, FileUrl = dto.FileUrl, UserName = dto.UserName,
            UploadDate = dto.UploadDate, DownloadCount = dto.DownloadCount, Status = dto.Status,
        };

        private static MeetingReportsDto ToDto(MeetingReports entity) => new()
        {
            Id = entity.Id, Title = entity.Title, MeetingId = entity.MeetingId,
            GroupName = entity.GroupName, FileUrl = entity.FileUrl, UserName = entity.UserName,
            UploadDate = entity.UploadDate, DownloadCount = entity.DownloadCount, Status = entity.Status,
        };
    }
}
