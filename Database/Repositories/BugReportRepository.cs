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
    public class BugReportRepository : Repository<BugReport>, IBugReportRepository
    {
        public BugReportRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<BugReport?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public override async Task<IEnumerable<BugReport>> GetAllAsync()
        {
            return await _dbSet
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<BugReport>> GetBugReportsByUserAsync(Guid userId)
        {
            return await _dbSet
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BugReport>> GetBugReportsByStatusAsync(BugReportStatus status)
        {
            return await _dbSet
                .Include(b => b.User)
                .Where(b => b.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<BugReport>> GetBugReportsBySeverityAsync(BugReportSeverity severity)
        {
            return await _dbSet
                .Include(b => b.User)
                .Where(b => b.Severity == severity)
                .ToListAsync();
        }

        public async Task<IEnumerable<BugReport>> GetOpenBugReportsAsync()
        {
            return await _dbSet
                .Include(b => b.User)
                .Where(b => b.Status == BugReportStatus.Open)
                .ToListAsync();
        }

        public async Task<IEnumerable<BugReport>> GetRecentBugReportsAsync(int count = 10)
        {
            return await _dbSet
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}
