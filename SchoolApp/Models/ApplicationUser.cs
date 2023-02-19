using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


// PASUL 1 - useri si roluri 
namespace SchoolApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public virtual IEnumerable<UserGroup>? UserGroups { get; set; }

        public virtual ICollection<Message>? Message { get; set; }
    }
}
