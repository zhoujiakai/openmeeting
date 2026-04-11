using MeetingSystem.IService.Base;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;

namespace MeetingSystem.IService.Meeting
{
    public interface IMeetingReportService : IBaseService
    {
        public int AddMeetingReport(MeetingReportsDto dto);
        public int UpdateMeetingReport(MeetingReportsDto dto);
        public MeetingReportsDto? GetMeetingReports(int id);
        public PageDto GetMeetingReportsPage(PageParams pageParams);
    }
}
