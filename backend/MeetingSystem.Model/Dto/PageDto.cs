namespace MeetingSystem.Model.Dto
{
    public class PageDto
    {
        public int Limit { get; set; } = 10;
        public int Page { get; set; } = 1;
        public int Total { get; set; } = 30;
        public dynamic? List { get; set; }

        public PageDto SetList(dynamic List)
        {
            this.List = List;
            return this;
        }

        public PageDto SetTotal(int total)
        {
            this.Total = total;
            return this;
        }
    }
}
