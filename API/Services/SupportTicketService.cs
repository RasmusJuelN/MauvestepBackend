using API.DTOs;
using API.Interfaces;
using Database.Interfaces;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class SupportTicketService : ISupportTicketService
    {
        private readonly ISupportTicketRepository _supportTicketRepository;
        private readonly IUserRepository _userRepository;

        public SupportTicketService(ISupportTicketRepository supportTicketRepository, IUserRepository userRepository)
        {
            _supportTicketRepository = supportTicketRepository;
            _userRepository = userRepository;
        }

        private SupportTicketDto MapToDto(SupportTicket ticket)
        {
            return new SupportTicketDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                Username = ticket.User?.Username ?? string.Empty,
                Name = ticket.Name,
                Email = ticket.Email,
                Subject = ticket.Subject,
                Message = ticket.Message,
                Status = ticket.Status,
                CreatedAt = ticket.CreatedAt,
                ResolvedAt = ticket.ResolvedAt
            };
        }
        
        public async Task<SupportTicketDto?> GetTicketByIdAsync(Guid id)
        {
            var ticket = await _supportTicketRepository.GetByIdAsync(id);
            if (ticket == null)
                return null;

            return MapToDto(ticket);
        }

        public async Task<IEnumerable<SupportTicketDto>> GetAllTicketsAsync()
        {
            var tickets = await _supportTicketRepository.GetAllAsync();
            return tickets.Select(MapToDto);
        }

        public async Task<IEnumerable<SupportTicketDto>> GetTicketsByUserAsync(Guid userId)
        {
            var tickets = await _supportTicketRepository.GetTicketsByUserAsync(userId);
            return tickets.Select(MapToDto);
        }

        public async Task<IEnumerable<SupportTicketDto>> GetOpenTicketsAsync()
        {
            var tickets = await _supportTicketRepository.GetOpenTicketsAsync();
            return tickets.Select(MapToDto);
        }

        public async Task<IEnumerable<SupportTicketDto>> GetTicketsByStatusAsync(string status)
        {
            if (!Enum.TryParse<SupportTicketStatus>(status, true, out var ticketStatus))
                return Enumerable.Empty<SupportTicketDto>();

            var tickets = await _supportTicketRepository.GetTicketsByStatusAsync(ticketStatus);
            return tickets.Select(MapToDto);
        }

        public async Task<SupportTicketDto> CreateTicketAsync(CreateSupportTicketDto dto, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var ticket = new SupportTicket
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = dto.Name,
                Email = dto.Email,
                Subject = dto.Subject,
                Message = dto.Message,
                Status = SupportTicketStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            await _supportTicketRepository.AddAsync(ticket);
            await _supportTicketRepository.SaveChangesAsync();

            // Reload to get navigation properties
            ticket = await _supportTicketRepository.GetByIdAsync(ticket.Id);
            return MapToDto(ticket!);
        }

        public async Task<SupportTicketDto> UpdateTicketAsync(Guid id, UpdateSupportTicketDto dto)
        {
            var ticket = await _supportTicketRepository.GetByIdAsync(id);
            if (ticket == null)
                throw new InvalidOperationException("Ticket not found");

            ticket.Status = dto.Status;
            ticket.ResolvedAt = dto.ResolvedAt;

            await _supportTicketRepository.UpdateAsync(ticket);
            await _supportTicketRepository.SaveChangesAsync();

            return MapToDto(ticket);
        }

        public async Task DeleteTicketAsync(Guid id)
        {

            var ticket = await _supportTicketRepository.GetByIdAsync(id);
            if (ticket == null)
                throw new InvalidOperationException("Ticket not found");

            await _supportTicketRepository.DeleteAsync(ticket);
            await _supportTicketRepository.SaveChangesAsync();
        }

        
    }
}
