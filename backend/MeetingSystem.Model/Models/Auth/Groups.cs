using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingSystem.Model.Models.Auth;

[Table("groups")]
public class Groups
{
    public Groups(int id)
    {
        Id = id;
    }
    public Groups()
    {
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string GroupName { get; set; } = "";
    public int Status { get; set; } = 1;
}
