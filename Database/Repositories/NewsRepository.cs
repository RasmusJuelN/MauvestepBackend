using Database.Context;
using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Repositories
{

    public class NewsRepository : Repository<NewsArticle>, INewsArticleRepository
    {
        public NewsRepository(AppDbContext context) : base(context)
        {
        }



        public async Task<IEnumerable<NewsArticle>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(a => a.CreatedAt >= startDate && a.CreatedAt <= endDate)
                .OrderByDescending(a => a.CreatedAt)
                .Include(a => a.User)
                .ToListAsync();
        }

        public override async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            return await _dbSet
                .OrderByDescending(a => a.CreatedAt)
                .Include(a => a.User)
                .ToListAsync();
        }

  
        public override async Task<NewsArticle?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
