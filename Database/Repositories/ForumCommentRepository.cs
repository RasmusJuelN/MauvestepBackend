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
    public class ForumCommentRepository : Repository<ForumComment>, IForumCommentRepository
    {
        public ForumCommentRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ForumComment>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.User)
                .Include(c => c.CommentRatings)
                .ToListAsync();
        }

        public override async Task<ForumComment?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(c => c.User)
                .Include(c => c.CommentRatings)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<ForumComment>> GetCommentsByPostAsync(Guid postId)
        {
            return await _dbSet
                .Include(c => c.User)
                .Include(c => c.CommentRatings)
                .Where(c => c.ForumPostId == postId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ForumComment>> GetCommentsByUserAsync(Guid userId)
        {
            return await _dbSet
                .Include(c => c.User)
                .Include(c => c.CommentRatings)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
    }
}

