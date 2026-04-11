using MeetingSystem.Common.Helper;
using MeetingSystem.Model.Models;
using MeetingSystem.Model.Models.Auth;
using MeetingSystem.Model.Models.Meeting;
using MeetingSystem.Model.Models.WeeklyReport;
using Microsoft.EntityFrameworkCore;

namespace MeetingSystem.DBFactory.Database;

/// <summary>
/// 会议系统数据库上下文类，继承自EF Core的DbContext，管理所有实体的数据库访问
/// </summary>
public class MeetingSystemDbContext : DbContext
{
    /// <summary>
    /// 会议信息数据集
    /// </summary>
    public DbSet<MeetingInfos> MeetingInfos { get; set; }

    /// <summary>
    /// 会议报告数据集
    /// </summary>
    public DbSet<MeetingReports> MeetingReports { get; set; }

    /// <summary>
    /// 小组数据集
    /// </summary>
    public DbSet<Groups> Groups { get; set; }

    /// <summary>
    /// 菜单项数据集
    /// </summary>
    public DbSet<MenuItems> MenuItems { get; set; }

    /// <summary>
    /// 用户数据集
    /// </summary>
    public DbSet<Users> Users { get; set; }

    /// <summary>
    /// 角色数据集
    /// </summary>
    public DbSet<Roles> Roles { get; set; }

    /// <summary>
    /// 周报数据集
    /// </summary>
    public DbSet<WeeklyReports> WeeklyReports { get; set; }

    /// <summary>
    /// 带数据库上下文选项的构造函数
    /// </summary>
    /// <param name="options">数据库上下文配置选项</param>
    public MeetingSystemDbContext(DbContextOptions<MeetingSystemDbContext> options) : base(options)
    {

    }

    /// <summary>
    /// 数据库配置方法，在配置上下文时调用
    /// </summary>
    /// <param name="optionsBuilder">数据库上下文选项构建器</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    /// <summary>
    /// 模型构建器，用于配置实体模型和种子数据
    /// </summary>
    /// <param name="modelBuilder">模型构建器实例</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TODO: 种子数据暂时禁用 - PostgreSQL timestamp with time zone 需要 UTC DateTime
        // 可以通过 API 或手动插入数据后再启用
        /*
        //添加种子数据 - 使用相对于项目根目录的路径
        string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataSeed");
        if (!Directory.Exists(basePath))
        {
            // 开发环境下的替代路径
            basePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "MeetingSystem.DBFactory", "DataSeed");
        }

        if (Directory.Exists(basePath))
        {
            string groupsPath = Path.Combine(basePath, "Groups.json");
            if (File.Exists(groupsPath))
            {
                IList<Groups>? groupsList = JsonHelper.FromJson<IList<Groups>>(File.ReadAllText(groupsPath));
                if (groupsList != null) modelBuilder.Entity<Groups>().HasData(groupsList);
            }

            string meetingInfosPath = Path.Combine(basePath, "MeetingInfos.json");
            if (File.Exists(meetingInfosPath))
            {
                IList<MeetingInfos>? meetingInfosList = JsonHelper.FromJson<IList<MeetingInfos>>(File.ReadAllText(meetingInfosPath));
                if (meetingInfosList != null) modelBuilder.Entity<MeetingInfos>().HasData(meetingInfosList);
            }

            string meetingReportsPath = Path.Combine(basePath, "MeetingReports.json");
            if (File.Exists(meetingReportsPath))
            {
                IList<MeetingReports>? meetingReportsList = JsonHelper.FromJson<IList<MeetingReports>>(File.ReadAllText(meetingReportsPath));
                if (meetingReportsList != null) modelBuilder.Entity<MeetingReports>().HasData(meetingReportsList);
            }

            string menuItemsPath = Path.Combine(basePath, "MenuItems.json");
            if (File.Exists(menuItemsPath))
            {
                IList<MenuItems>? menuItemsList = JsonHelper.FromJson<IList<MenuItems>>(File.ReadAllText(menuItemsPath));
                if (menuItemsList != null) modelBuilder.Entity<MenuItems>().HasData(menuItemsList);
            }

            string rolesPath = Path.Combine(basePath, "Roles.json");
            if (File.Exists(rolesPath))
            {
                IList<Roles>? rolesList = JsonHelper.FromJson<IList<Roles>>(File.ReadAllText(rolesPath));
                if (rolesList != null) modelBuilder.Entity<Roles>().HasData(rolesList);
            }

            string usersPath = Path.Combine(basePath, "Users.json");
            if (File.Exists(usersPath))
            {
                IList<Users>? usersList = JsonHelper.FromJson<IList<Users>>(File.ReadAllText(usersPath));
                if (usersList != null) modelBuilder.Entity<Users>().HasData(usersList);
            }

            string weeklyReportsPath = Path.Combine(basePath, "WeeklyReports.json");
            if (File.Exists(weeklyReportsPath))
            {
                IList<WeeklyReports>? weeklyReportsList = JsonHelper.FromJson<IList<WeeklyReports>>(File.ReadAllText(weeklyReportsPath));
                if (weeklyReportsList != null) modelBuilder.Entity<WeeklyReports>().HasData(weeklyReportsList);
            }
        }
        */
    }
}
