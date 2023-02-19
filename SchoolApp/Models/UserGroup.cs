using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class UserGroup
    {
        [Key] 
        public int UserGroupId { get; set; }

        public string? UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int? GroupId { get; set; } 
        public virtual Group? Group{ get; set; }

        public bool IsModerator { get; set; }
    }
}
