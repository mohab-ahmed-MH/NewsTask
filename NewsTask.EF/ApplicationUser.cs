﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTask.EF
{
    public class ApplicationUser : IdentityUser 
    {
        [MaxLength(100)]
        public string UserName { get; set; }
    }
}
