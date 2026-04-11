using MeetingSystem.IService.Base;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.WeeklyReport;

namespace MeetingSystem.IService.WeeklyReport
{
    /// <summary>
    /// 周报服务接口，提供周报的增删改查及分页查询功能
    /// </summary>
    public interface IWeeklyReportService : IGenericService<WeeklyReports>
    {
        /// <summary>
        /// 获取所有周报列表
        /// </summary>
        /// <returns>周报集合</returns>
        IEnumerable<WeeklyReports> GetWeeklyReports();

        /// <summary>
        /// 根据ID获取周报信息
        /// </summary>
        /// <param name="id">周报ID</param>
        /// <returns>周报信息，未找到时返回null</returns>
        WeeklyReports? GetWeeklyReports(int id);

        /// <summary>
        /// 更新周报信息
        /// </summary>
        /// <param name="dto">周报信息</param>
        /// <returns>受影响的行数</returns>
        int UpdateWeeklyReports(WeeklyReports dto);

        /// <summary>
        /// 添加周报
        /// </summary>
        /// <param name="dto">周报信息</param>
        /// <returns>受影响的行数</returns>
        int AddWeeklyReports(WeeklyReports dto);

        /// <summary>
        /// 根据ID删除周报
        /// </summary>
        /// <param name="id">周报ID</param>
        /// <returns>受影响的行数</returns>
        int DeleteWeeklyReports(int id);

        /// <summary>
        /// 分页查询周报列表
        /// </summary>
        /// <param name="page">分页参数</param>
        /// <returns>分页结果</returns>
        PageDto GetPage(PageParams page);
    }
}
