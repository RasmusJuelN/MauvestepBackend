using System;
using Database.Models;

namespace API.DTOs
{
    public class SupportTicketDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
        public SupportTicketStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    public class CreateSupportTicketDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
    }

    public class UpdateSupportTicketDto
    {
        public SupportTicketStatus Status { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

}
