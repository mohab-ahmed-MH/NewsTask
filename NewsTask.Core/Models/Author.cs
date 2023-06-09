using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTask.Core.Models
{
    public class Author
    {
        public int Id { get; set; }
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }
    }
}
