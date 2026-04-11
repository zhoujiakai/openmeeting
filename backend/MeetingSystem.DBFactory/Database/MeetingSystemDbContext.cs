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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("meeting_system");
        base.OnModelCreating(modelBuilder);
    }
}
