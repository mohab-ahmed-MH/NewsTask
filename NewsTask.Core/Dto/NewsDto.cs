using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTask.Core.Dto
{
    public class NewsDto
    {
        public int? Id { get; set; }
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
