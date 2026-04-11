using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.WeeklyReport;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.WeeklyReport;
using MeetingSystem.Service.Base;

namespace MeetingSystem.Service.WeeklyReport
{
    public class WeeklyReportService : GenericService<MeetingSystemDbContext, WeeklyReports>, IWeeklyReportService
    {
        public WeeklyReportService(MeetingSystemDbContext dbContext) : base(dbContext) { }

        public WeeklyReports? GetWeeklyReports(int id)
            => _dbContext.WeeklyReports.FirstOrDefault(p => p.Id == id);

        public IEnumerable<WeeklyReports> GetWeeklyReports()
            => _dbContext.WeeklyReports.ToList();

        public int AddWeeklyReports(WeeklyReports weeklyReports)
        {
            weeklyReports.Id = 0;
            var user = _dbContext.Users.FirstOrDefault(a => a.UserName == weeklyReports.UserName);
            if (user != null && user.UserName != "管理员")
            {
                weeklyReports.GroupName = user.GroupName;
                weeklyReports.UploadDate = DateTime.Now;
                _dbContext.WeeklyReports.Add(weeklyReports);
                _dbContext.SaveChanges();
            }
            return 0;
        }

        public int UpdateWeeklyReports(WeeklyReports weeklyReports)
        {
            _dbContext.WeeklyReports.Update(weeklyReports);
            _dbContext.SaveChanges();
            return 0;
        }

        public int DeleteWeeklyReports(int id)
        {
            var entity = _dbContext.WeeklyReports.Find(id);
            if (entity == null) return 0;
            _dbContext.WeeklyReports.Remove(entity);
            _dbContext.SaveChanges();
            return 0;
        }

        public PageDto GetPage(PageParams page)
        {
            var query = _dbContext.WeeklyReports.Where(a =>
                (!string.IsNullOrEmpty(page.GroupName) ? a.GroupName == page.GroupName : true)
                && (!string.IsNullOrEmpty(page.UserName) ? a.UserName.Contains(page.UserName) : true)
                && (!string.IsNullOrEmpty(page.WeeklyReportTheme) ? a.Theme.Contains(page.WeeklyReportTheme) : true)
            );

            var total = query.Count();
            var list = query.Skip((page.Page - 1) * page.Limit).Take(page.Limit).ToList();
            return new PageDto().SetList(list).SetTotal(total);
        }
    }
}
