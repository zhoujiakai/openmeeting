using MeetingSystem.Common.Utils;
using MeetingSystem.IService.WeeklyReport;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.WeeklyReport;
using Microsoft.AspNetCore.Mvc;

namespace MeetingSystem.WEB.Controllers.WeeklyReport
{
    /// <summary>
    /// 周报管理控制器，提供周报的增删改查及分页查询功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeeklyReportController : ControllerBase
    {
        private readonly IWeeklyReportService _weeklyReportService;

        /// <summary>
        /// 构造函数，通过依赖注入获取周报服务实例
        /// </summary>
        /// <param name="weeklyReportService">周报服务接口</param>
        public WeeklyReportController(IWeeklyReportService weeklyReportService)
        {
            _weeklyReportService = weeklyReportService;
        }

        /// <summary>
        /// 获取所有周报列表
        /// </summary>
        /// <returns>周报列表</returns>
        [HttpPost]
        public R List() => new R().OK().SetData(_weeklyReportService.GetWeeklyReports().ToList());

        /// <summary>
        /// 根据ID获取单个周报
        /// </summary>
        /// <param name="id">周报ID</param>
        /// <returns>周报详情</returns>
        [HttpGet]
        public R Get(int id) => new R().OK().SetData(_weeklyReportService.GetWeeklyReports(id)!);

        /// <summary>
        /// 更新周报信息
        /// </summary>
        /// <param name="weeklyReports">周报信息</param>
        /// <returns>更新结果</returns>
        [HttpPost]
        public R Update(WeeklyReports weeklyReports) => new R().OK().SetData(_weeklyReportService.UpdateWeeklyReports(weeklyReports));

        /// <summary>
        /// 添加新周报
        /// </summary>
        /// <param name="weeklyReports">周报信息</param>
        /// <returns>添加结果</returns>
        [HttpPost]
        public R Add(WeeklyReports weeklyReports) => new R().OK().SetData(_weeklyReportService.AddWeeklyReports(weeklyReports));

        /// <summary>
        /// 根据ID删除周报
        /// </summary>
        /// <param name="id">周报ID</param>
        /// <returns>删除结果</returns>
        [HttpGet]
        public R Delete(int id) => new R().OK().SetData(_weeklyReportService.DeleteWeeklyReports(id));

        /// <summary>
        /// 分页获取周报列表
        /// </summary>
        /// <param name="page">分页参数</param>
        /// <returns>分页周报数据</returns>
        [HttpPost]
        public R GetPage(PageParams page) => new R().OK().SetData(_weeklyReportService.GetPage(page));
    }
}
