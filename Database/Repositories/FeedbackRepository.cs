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
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) : base(context)
        {
        }

        // Override to include User navigation property
        public override async Task<Feedback?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        // Override to include User navigation property
        public override async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await _dbSet
                .Include(f => f.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackByUserAsync(Guid userId)
        {
            return await _dbSet
                .Include(f => f.User)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackByCategoryAsync(string category)
        {
            return await _dbSet
                .Include(f => f.User)
                .Where(f => f.Category == category)
                .ToListAsync();
        }

    }
}
