using Database.Context;
using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class ForumCategoryRepository : Repository<ForumCategory>, IForumCategoryRepository
    {
        public ForumCategoryRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ForumCategory>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.ForumThreads)
                    .ThenInclude(t => t.User)
                .Include(c => c.ForumThreads)
                    .ThenInclude(t => t.ForumPosts)
                .ToListAsync();
        }

        public async Task<ForumCategory?> GetByNameAsync(string name)
        {
            return await FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<ForumCategory?> GetCategoryWithThreadsAsync(Guid categoryId)
        {
            return await _dbSet
                .Include(c => c.ForumThreads)
                    .ThenInclude(t => t.User)
                .Include(c => c.ForumThreads)
                    .ThenInclude(t => t.ForumPosts)
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }
    }
}
