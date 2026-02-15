using Database.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Interfaces
{

    public interface ISupportTicketRepository : IRepository<SupportTicket>
    {

        Task<IEnumerable<SupportTicket>> GetTicketsByUserAsync(Guid userId);
        Task<IEnumerable<SupportTicket>> GetTicketsByStatusAsync(SupportTicketStatus status);
        Task<IEnumerable<SupportTicket>> GetOpenTicketsAsync();
    }
}
