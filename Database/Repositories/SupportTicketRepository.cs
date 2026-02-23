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
    public class SupportTicketRepository : Repository<SupportTicket>, ISupportTicketRepository
    {
        public SupportTicketRepository(AppDbContext context) : base(context)
        {
        }

       public override async Task<SupportTicket?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override async Task<IEnumerable<SupportTicket>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<SupportTicket>> GetTicketsByUserAsync(Guid userId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<SupportTicket>> GetTicketsByStatusAsync(SupportTicketStatus status)
        {
            return await _dbSet
                .Include(t => t.User)
                .Where(t => t.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<SupportTicket>> GetOpenTicketsAsync()
        {
            return await _dbSet
                .Include(t => t.User)
                .Where(t => t.Status == SupportTicketStatus.Open)
                .ToListAsync();
        }
    }
}
