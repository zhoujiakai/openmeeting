using MeetingSystem.Common.Utils;
using MeetingSystem.IService.Meeting;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;
using Microsoft.AspNetCore.Mvc;

namespace MeetingSystem.WEB.Controllers.MeetingReport
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MeetingReportController : ControllerBase
    {
        private readonly IMeetingReportService _meetingReportService;
        public MeetingReportController(IMeetingReportService meetingReportService)
        {
            _meetingReportService = meetingReportService;
        }

        [HttpPost]
        public R AddMeetingReport(MeetingReportsDto meetingReports)
            => new R().OK().SetData(_meetingReportService.AddMeetingReport(meetingReports)).SetMessage("添加成功");

        [HttpGet]
        public R DeleteMeetingReport(int id)
        {
            _meetingReportService.Delete(new MeetingReports(id));
            return new R().OK().SetData(0);
        }

        [HttpPost]
        public R UpdateMeetingReport(MeetingReportsDto dto)
        {
            _meetingReportService.UpdateMeetingReport(dto);
            return new R().OK().SetMessage("更新成功");
        }

        [HttpGet]
        public R GetMeetingReport(int id)
            => new R().OK().SetData(_meetingReportService.GetMeetingReports(id)!);

        [HttpPost]
        public R GetMeetingReportPage(PageParams pageParams)
            => new R().OK().SetData(_meetingReportService.GetMeetingReportsPage(pageParams));
    }
}
