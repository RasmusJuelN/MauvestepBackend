using Database.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Interfaces
{

    public interface INewsArticleRepository : IRepository<NewsArticle>
    {

        Task<IEnumerable<NewsArticle>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
