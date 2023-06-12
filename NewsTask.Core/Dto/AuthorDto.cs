using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTask.Core.Dto
{
    public class AuthorDto
    {
        public AuthorDto(string name, int? id)
        {
            this.Name = name;
            this.Id = id;
        }
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }
        public int? Id { get; set; }
    }
}
