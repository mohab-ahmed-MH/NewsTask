using System.ComponentModel.DataAnnotations;

namespace NewsTask.Mvc.Models
{
    public class NewsViewModel
    {
        [Required, MaxLength(250)]
        public string Title { get; set; }
        [Required, MaxLength(2500)]
        public string NewsDescription { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        public IFormFile Image { get; set; }

        public int AuthorId { get; set; }
    }
}
