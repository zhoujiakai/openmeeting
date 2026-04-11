using MeetingSystem.IService.Base;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.WeeklyReport;

namespace MeetingSystem.IService.WeeklyReport
{
    public interface IWeeklyReportService : IGenericService<WeeklyReports>
    {
        /// <summary>
        /// 查询WeeklyReport列表
        /// </summary>
        /// <returns>AuthorBooks</returns>
        IEnumerable<WeeklyReports> GetWeeklyReports();
        WeeklyReports? GetWeeklyReports(int id);
        int UpdateWeeklyReports(WeeklyReports dto);
        int AddWeeklyReports(WeeklyReports dto);
        int DeleteWeeklyReports(int id);
        PageDto GetPage(PageParams page);
    }
}
