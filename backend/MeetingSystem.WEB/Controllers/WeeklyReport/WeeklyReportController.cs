using MeetingSystem.Common.Utils;
using MeetingSystem.IService.WeeklyReport;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.WeeklyReport;
using Microsoft.AspNetCore.Mvc;

namespace MeetingSystem.WEB.Controllers.WeeklyReport
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeeklyReportController : ControllerBase
    {
        private readonly IWeeklyReportService _weeklyReportService;
        public WeeklyReportController(IWeeklyReportService weeklyReportService)
        {
            _weeklyReportService = weeklyReportService;
        }

        [HttpPost]
        public R List() => new R().OK().SetData(_weeklyReportService.GetWeeklyReports().ToList());

        [HttpGet]
        public R Get(int id) => new R().OK().SetData(_weeklyReportService.GetWeeklyReports(id)!);

        [HttpPost]
        public R Update(WeeklyReports weeklyReports) => new R().OK().SetData(_weeklyReportService.UpdateWeeklyReports(weeklyReports));

        [HttpPost]
        public R Add(WeeklyReports weeklyReports) => new R().OK().SetData(_weeklyReportService.AddWeeklyReports(weeklyReports));

        [HttpGet]
        public R Delete(int id) => new R().OK().SetData(_weeklyReportService.DeleteWeeklyReports(id));

        [HttpPost]
        public R GetPage(PageParams page) => new R().OK().SetData(_weeklyReportService.GetPage(page));
    }
}
