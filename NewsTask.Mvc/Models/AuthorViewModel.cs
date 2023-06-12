using System.ComponentModel.DataAnnotations;

namespace NewsTask.Mvc.Models
{
    public class AuthorViewModel
    {
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }
    }
}
