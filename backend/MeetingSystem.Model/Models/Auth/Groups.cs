using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingSystem.Model.Models.Auth;

/// <summary>
/// 小组实体类，对应数据库groups表，存储小组信息
/// </summary>
[Table("groups")]
public class Groups
{
    /// <summary>
    /// 带ID的构造函数
    /// </summary>
    /// <param name="id">小组ID</param>
    public Groups(int id)
    {
        Id = id;
    }

    /// <summary>
    /// 默认构造函数
    /// </summary>
    public Groups()
    {
    }

    /// <summary>
    /// 小组ID，主键，自增
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 小组名称
    /// </summary>
    public string GroupName { get; set; } = "";

    /// <summary>
    /// 小组状态：0-不启用，1-启用
    /// </summary>
    public int Status { get; set; } = 1;
}
