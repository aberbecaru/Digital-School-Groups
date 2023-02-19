using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }


        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(100, ErrorMessage = "Titlul nu poate avea mai mult de 100 de caractere")]
        [MinLength(5, ErrorMessage = "Titlul trebuie sa aiba mai mult de 5 caractere")]
        public string? GroupName { get; set; }


        [Required(ErrorMessage = "Categoria este obligatorie")]

        
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        

        [NotMapped]
        public Message? Message { get; set; }

        public virtual ICollection<Message>? Messages { get; set; }
    }

}

