using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.Meeting;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Meeting;
using MeetingSystem.Service.Base;

namespace MeetingSystem.Service.Meeting
{
    public class MeetingService : BaseService, IMeetingService
    {
        public MeetingService(MeetingSystemDbContext dbContext) : base(dbContext) { }

        public int AddMeetingInfos(MeetingInfos meetingInfos)
        {
            if (meetingInfos.MeetingType == 2)
                meetingInfos.Code = Guid.NewGuid().ToString();
            _dbContext.Add(meetingInfos);
            _dbContext.SaveChanges();
            return 0;
        }

        public int DeleteMeetingInfos(int id)
        {
            var entity = _dbContext.MeetingInfos.Find(id);
            if (entity == null) return 0;
            _dbContext.MeetingInfos.Remove(entity);
            _dbContext.SaveChanges();
            return 0;
        }

        public MeetingInfos GetMeetingInfos(int id)
        {
            return _dbContext.MeetingInfos.Find(id) ?? new MeetingInfos { Id = id };
        }

        public PageDto GetMeetingInfosPage(PageParams page)
        {
            var query = _dbContext.MeetingInfos
                .Where(a => page.MeetingDateTimeRange != null ? page.MeetingDateTimeRange[0] < a.StartTime : true)
                .Where(a => (page.MeetingDateTimeRange != null && page.MeetingDateTimeRange.Count > 1) ? a.EndTime < page.MeetingDateTimeRange[1] : true)
                .Where(a => !string.IsNullOrEmpty(page.MeetingKeywords) ?
                    a.Keywords.Contains(page.MeetingKeywords) || a.Title.Contains(page.MeetingKeywords) : true)
                .Where(a => !string.IsNullOrEmpty(page.GroupName) ? a.GroupName == page.GroupName : true);

            var total = query.Count();
            var list = query.Skip((page.Page - 1) * page.Limit).Take(page.Limit).ToList();
            return new PageDto().SetList(list).SetTotal(total);
        }

        public int UpdateMeetingInfos(MeetingInfos meetingInfos)
        {
            if (meetingInfos.MeetingType == 2)
                meetingInfos.Code = Guid.NewGuid().ToString();
            _dbContext.Update(meetingInfos);
            _dbContext.SaveChanges();
            return 0;
        }
    }
}
