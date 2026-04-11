namespace MeetingSystem.Model.Dto
{
    /// <summary>
    /// 分页返回数据类，用于封装分页查询的结果数据
    /// </summary>
    public class PageDto
    {
        /// <summary>
        /// 每页显示数量，默认10条
        /// </summary>
        public int Limit { get; set; } = 10;

        /// <summary>
        /// 当前页码，默认第1页
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 总记录数，默认30条
        /// </summary>
        public int Total { get; set; } = 30;

        /// <summary>
        /// 当前页的数据列表
        /// </summary>
        public dynamic? List { get; set; }

        /// <summary>
        /// 设置数据列表
        /// </summary>
        /// <param name="List">数据列表</param>
        /// <returns>当前PageDto对象，支持链式调用</returns>
        public PageDto SetList(dynamic List)
        {
            this.List = List;
            return this;
        }

        /// <summary>
        /// 设置总记录数
        /// </summary>
        /// <param name="total">总记录数</param>
        /// <returns>当前PageDto对象，支持链式调用</returns>
        public PageDto SetTotal(int total)
        {
            this.Total = total;
            return this;
        }
    }
}
