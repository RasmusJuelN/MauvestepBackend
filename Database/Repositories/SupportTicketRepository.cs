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

        public async Task<IEnumerable<SupportTicket>> GetTicketsByUserAsync(Guid userId)
        {
            return await FindAsync(t => t.UserId == userId);
        }

        public async Task<IEnumerable<SupportTicket>> GetTicketsByStatusAsync(SupportTicketStatus status)
        {
            return await FindAsync(t => t.Status == status);
        }

        public async Task<IEnumerable<SupportTicket>> GetOpenTicketsAsync()
        {
            return await FindAsync(t => t.Status == SupportTicketStatus.Open);
        }
    }
}
