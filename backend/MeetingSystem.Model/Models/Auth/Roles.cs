using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingSystem.Model.Models.Auth
{
    [Table("roles")]
    public class Roles
    {
        public Roles(){}
        public Roles(int id)
        {
            Id = id;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RoleName { get; set; } = "";
        public int Status { get; set; }
        [NotMapped]
        public List<MenuItems> MenuItemList { get; set; } = new List<MenuItems>();
    }
}
