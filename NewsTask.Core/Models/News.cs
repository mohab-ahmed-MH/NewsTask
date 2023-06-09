using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTask.Core.Models
{
    public class News
    {
        private DateTime _CreationDate = DateTime.Now;


        public int Id { get; set; }
        [Required,MaxLength(250)]
        public string Title { get; set; }
        [Required,MaxLength(2500)]
        public string NewsDescription { get; set; }
        public DateTime CreationDate { get { return _CreationDate; } set { _CreationDate = value; } }
        [Required,MaxLength(250)]
        public string PublicationDate { get; set; }
        public byte[]? Image { get; set; }
        public bool IsPublish { get; set; }


        public int AuthorId { get; set; }
        public Author Author { get; set; }

    }
}
