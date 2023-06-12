using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsTask.Mvc.Models;

namespace NewsTask.Mvc.Data
{
    public class NewsTaskMvcContext : DbContext
    {
        public NewsTaskMvcContext (DbContextOptions<NewsTaskMvcContext> options)
            : base(options)
        {
        }

        public DbSet<NewsTask.Mvc.Models.AuthorViewModel> AuthorViewModel { get; set; } = default!;

        public DbSet<NewsTask.Mvc.Models.NewsViewModel>? NewsViewModel { get; set; }
    }
}
