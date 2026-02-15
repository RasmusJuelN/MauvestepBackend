using Database.Context;
using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class PostRatingRepository : Repository<PostRating>, IPostRatingRepository
    {
        public PostRatingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PostRating?> GetByPostAndUserAsync(Guid postId, Guid userId)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.ForumPostId == postId && r.UserId == userId);
        }
    }
}
