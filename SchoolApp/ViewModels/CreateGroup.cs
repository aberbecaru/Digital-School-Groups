using SchoolApp.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.ViewModels
{
    public class CreateGroup
    {
        public Group group { get; set; }

        public IEnumerable<Category>? categories { get; set; }
    }
}
