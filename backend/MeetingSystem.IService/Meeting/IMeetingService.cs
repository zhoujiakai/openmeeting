using MeetingSystem.IService.Base;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;

namespace MeetingSystem.IService.Meeting
{
    public interface IMeetingService : IBaseService
    {
        MeetingInfos GetMeetingInfos(int id);
        int UpdateMeetingInfos(MeetingInfos dto);
        int AddMeetingInfos(MeetingInfos dto);
        int DeleteMeetingInfos(int id);
        PageDto GetMeetingInfosPage(PageParams page);
    }
}
