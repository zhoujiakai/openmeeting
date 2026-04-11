using MeetingSystem.Common.Utils;
using MeetingSystem.IService.Meeting;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;
using Microsoft.AspNetCore.Mvc;

namespace MeetingSystem.WEB.Controllers.Meeting
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;
        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpPost]
        public R AddMeetingInfos(MeetingInfos meetingInfos)
            => new R().OK().SetData(_meetingService.AddMeetingInfos(meetingInfos)).SetMessage("添加成功");

        [HttpGet]
        public R DeleteMeetingInfos(int id)
            => new R().OK().SetData(_meetingService.DeleteMeetingInfos(id)).SetMessage("删除成功");

        [HttpPost]
        public R UpdateMeetingInfos(MeetingInfos dto)
            => new R().OK().SetData(_meetingService.UpdateMeetingInfos(dto)).SetMessage("更新成功");

        [HttpGet]
        public R GetMeetingInfos(int id)
            => new R().OK().SetData(_meetingService.GetMeetingInfos(id));

        [HttpPost]
        public R GetMeetingInfosPage(PageParams page)
            => new R().OK().SetData(_meetingService.GetMeetingInfosPage(page));
    }
}
