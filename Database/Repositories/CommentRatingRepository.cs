using Database.Context;
using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class CommentRatingRepository : Repository<CommentRating>, ICommentRatingRepository
    {
        public CommentRatingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<CommentRating?> GetByCommentAndUserAsync(Guid commentId, Guid userId)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.ForumCommentId == commentId && r.UserId == userId);
        }
    }
}
