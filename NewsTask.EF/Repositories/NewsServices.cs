using Microsoft.EntityFrameworkCore;
using NewsTask.Core.Dto;
using NewsTask.Core.Models;
using NewsTask.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsTask.EF.Repositories
{
    public class NewsServices : INewsServices
    {
        protected ApplicationDbContext _context;
        public NewsServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<News> GetById(int id)
        {
            return await _context.News.Include(n => n.Author).SingleOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<News>> GetAll(int authorId = 0)
        {

            return await _context.News
                .Where(n => n.AuthorId == authorId || authorId == 0)
                .Include(n => n.Author).ToListAsync();
        }

        public async Task<News> Create(News news)
        {
            await _context.AddAsync(news);
            await _context.SaveChangesAsync();

            return news;
        }

        public News Update(News news)
        {
            _context.Update(news);
            _context.SaveChanges();
            return news;
        }

        public List<News> UpdateNews(List<News> news)
        {
            _context.Update(news);
            _context.SaveChangesAsync();
            return news;
        }

        public News Delete(News news)
        {
            _context.Remove(news);
            _context.SaveChanges();

            return news;
        }

        public async Task PublishToBePublished()
        {
            var newsList = await _context.News.Where(x => !x.IsPublish && x.PublicationDate.Date <= DateTime.Now.Date).ToListAsync();
            if (newsList.Count > 0)
            {
                newsList.ForEach(x =>
                {
                    x.IsPublish = true;
                });

                UpdateNews(newsList);
            }
        }
    }
}
