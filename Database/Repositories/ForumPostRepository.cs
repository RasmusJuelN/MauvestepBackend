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
    public class ForumPostRepository : Repository<ForumPost>, IForumPostRepository
    {
        public ForumPostRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ForumPost>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.ForumComments)
                    .ThenInclude(c => c.User)
                .Include(p => p.ForumComments)
                    .ThenInclude(c => c.CommentRatings)
                .Include(p => p.PostRatings)
                .ToListAsync();
        }

        public override async Task<ForumPost?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.ForumComments)
                    .ThenInclude(c => c.User)
                .Include(p => p.ForumComments)
                    .ThenInclude(c => c.CommentRatings)
                .Include(p => p.PostRatings)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<ForumPost>> GetPostsByThreadAsync(Guid threadId)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.ForumComments)
                    .ThenInclude(c => c.User)
                .Include(p => p.ForumComments)
                    .ThenInclude(c => c.CommentRatings)
                .Include(p => p.PostRatings)
                .Where(p => p.ForumThreadId == threadId)
                .ToListAsync();
        }

        public async Task<ForumPost?> GetPostWithCommentsAsync(Guid postId)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.ForumComments)
                    .ThenInclude(c => c.User)
                .Include(p => p.ForumComments)
                    .ThenInclude(c => c.CommentRatings)
                .Include(p => p.PostRatings)
                .FirstOrDefaultAsync(p => p.Id == postId);
        }

    }
}
