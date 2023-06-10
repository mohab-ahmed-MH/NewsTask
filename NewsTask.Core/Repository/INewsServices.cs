using NewsTask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsTask.Core.Repository
{
    public interface INewsServices
    {

        Task<News> GetById(int id);
        Task<IEnumerable<News>> GetAll(int authorId = 0);

        Task<News> Create(News news);
        News Update(News news);
        List<News> UpdateNews(List<News> news);
        News Delete(News news);

        Task PublishToBePublished();

    }
}
