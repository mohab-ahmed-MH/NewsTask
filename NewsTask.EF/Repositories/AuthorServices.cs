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
using System.Xml.Linq;

namespace NewsTask.EF.Repositories
{
    public class AuthorServices : IAuthorServices
    {
        protected ApplicationDbContext _context;
        public AuthorServices(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<Author> GetById(int id)
        {
            return await _context.Authors.SingleOrDefaultAsync(a=>a.Id == id);
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _context.Authors.OrderBy(a=>a.Name).ToListAsync();
            
        }

        public async Task<Author> Create(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            return author;

        }

        public Author Update(Author author)
        {
            _context.Update(author);
            _context.SaveChangesAsync();
            return author;
        }

        public Author Delete(Author author)
        {
            _context.Remove(author);
            _context.SaveChanges();

            return author;
            
        }

        public async Task<bool> IsValidAuthor(int id)
        {
            return await _context.Authors.AnyAsync(a =>a.Id == id);
        }

    }
}