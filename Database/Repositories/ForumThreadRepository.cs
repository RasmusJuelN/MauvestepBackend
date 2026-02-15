using Database.Context;
using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class ForumThreadRepository : Repository<ForumThread>, IForumThreadRepository
    {
        public ForumThreadRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ForumThread>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.ForumPosts)
                .ToListAsync();
        }

        public override async Task<ForumThread?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.ForumPosts)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<ForumThread>> GetThreadsByCategoryAsync(Guid categoryId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.ForumPosts)
                .Where(t => t.ForumCategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ForumThread>> GetThreadsByUserAsync(Guid userId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.ForumPosts)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<ForumThread?> GetThreadWithPostsAsync(Guid threadId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.ForumPosts)
                .FirstOrDefaultAsync(t => t.Id == threadId);
        }
    }
}
