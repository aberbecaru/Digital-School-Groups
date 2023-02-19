using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }

        public int? GroupId { get; set; }

        public virtual Group? Group { get; set; }   

        [Required(ErrorMessage = "Continutul comentariului este obligatoriu")]
        public string? Content { get; set; }
    }

}
