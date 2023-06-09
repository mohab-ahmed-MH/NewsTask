using NewsTask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsTask.Core.Repository
{
    public interface IAuthorServices
    {

        Task<Author> GetById(int id);
        Task<IEnumerable<Author>> GetAll();

        Task<Author> Create(Author author);
        Author Update(Author author);
        Author Delete(Author author);

        Task<bool> IsValidAuthor(int id);

    }
}
