using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.WeeklyReport;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.WeeklyReport;
using MeetingSystem.Service.Base;

namespace MeetingSystem.Service.WeeklyReport
{
    /// <summary>
    /// 周报服务，提供周报的增删改查及分页查询功能。
    /// 继承自 GenericService 泛型服务，支持按分组、用户名和主题进行筛选。
    /// 新增周报时会自动关联用户的分组信息并记录上传时间。
    /// </summary>
    public class WeeklyReportService : GenericService<MeetingSystemDbContext, WeeklyReports>, IWeeklyReportService
    {
        /// <summary>
        /// 构造函数，注入数据库上下文
        /// </summary>
        /// <param name="dbContext">会议系统数据库上下文</param>
        public WeeklyReportService(MeetingSystemDbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// 根据ID获取单条周报记录
        /// </summary>
        /// <param name="id">周报ID</param>
        /// <returns>周报实体对象，未找到则返回 null</returns>
        public WeeklyReports? GetWeeklyReports(int id)
            => _dbContext.WeeklyReports.FirstOrDefault(p => p.Id == id);

        /// <summary>
        /// 获取所有周报记录列表
        /// </summary>
        /// <returns>周报实体集合</returns>
        public IEnumerable<WeeklyReports> GetWeeklyReports()
            => _dbContext.WeeklyReports.ToList();

        /// <summary>
        /// 添加周报记录。根据用户名自动填充分组信息，管理员用户不允许添加周报。
        /// </summary>
        /// <param name="weeklyReports">周报实体对象</param>
        /// <returns>始终返回 0</returns>
        public int AddWeeklyReports(WeeklyReports weeklyReports)
        {
            weeklyReports.Id = 0; // 重置ID，确保作为新记录插入

            // 根据用户名查找用户信息，自动填充分组名称
            var user = _dbContext.Users.FirstOrDefault(a => a.UserName == weeklyReports.UserName);
            if (user != null && user.UserName != "管理员")
            {
                weeklyReports.GroupName = user.GroupName; // 自动填充用户所属分组
                weeklyReports.UploadDate = DateTime.Now; // 设置上传时间为当前时间
                _dbContext.WeeklyReports.Add(weeklyReports);
                _dbContext.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// 更新周报记录
        /// </summary>
        /// <param name="weeklyReports">周报实体对象</param>
        /// <returns>始终返回 0</returns>
        public int UpdateWeeklyReports(WeeklyReports weeklyReports)
        {
            _dbContext.WeeklyReports.Update(weeklyReports);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 根据ID删除周报记录
        /// </summary>
        /// <param name="id">周报ID</param>
        /// <returns>始终返回 0，周报不存在也返回 0</returns>
        public int DeleteWeeklyReports(int id)
        {
            var entity = _dbContext.WeeklyReports.Find(id);
            if (entity == null) return 0;
            _dbContext.WeeklyReports.Remove(entity);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 获取周报分页列表，支持按分组名称、用户名和主题进行筛选
        /// </summary>
        /// <param name="page">分页参数，包含筛选条件和分页信息</param>
        /// <returns>包含周报列表和总数的分页结果 DTO</returns>
        public PageDto GetPage(PageParams page)
        {
            // 构建查询条件：根据分组名称、用户名和主题动态筛选
            var query = _dbContext.WeeklyReports.Where(a =>
                (!string.IsNullOrEmpty(page.GroupName) ? a.GroupName == page.GroupName : true)
                && (!string.IsNullOrEmpty(page.UserName) ? a.UserName.Contains(page.UserName) : true)
                && (!string.IsNullOrEmpty(page.WeeklyReportTheme) ? a.Theme.Contains(page.WeeklyReportTheme) : true)
            );

            var total = query.Count(); // 总记录数
            var list = query.Skip((page.Page - 1) * page.Limit).Take(page.Limit).ToList(); // 分页查询
            return new PageDto().SetList(list).SetTotal(total);
        }
    }
}
