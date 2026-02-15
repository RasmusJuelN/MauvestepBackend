using API.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface ISupportTicketService
    {
        Task<SupportTicketDto?> GetTicketByIdAsync(Guid id);
        Task<IEnumerable<SupportTicketDto>> GetAllTicketsAsync();
        Task<IEnumerable<SupportTicketDto>> GetTicketsByUserAsync(Guid userId);
        Task<IEnumerable<SupportTicketDto>> GetOpenTicketsAsync();
        Task<IEnumerable<SupportTicketDto>> GetTicketsByStatusAsync(string status);
        Task<SupportTicketDto> CreateTicketAsync(CreateSupportTicketDto dto, Guid userId);
        Task<SupportTicketDto> UpdateTicketAsync(Guid id, UpdateSupportTicketDto dto);
        Task DeleteTicketAsync(Guid id);
    }
}
