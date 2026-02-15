using System;
using Database.Models;

namespace API.DTOs
{
    public class BugReportDto
    {
        public Guid Id { get; set; }
        public Guid UserId   { get; set; }
        public string Username { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public BugReportStatus Status { get; set; }
        public BugReportSeverity Severity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    public class CreateBugReportDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public BugReportSeverity Severity { get; set; } = BugReportSeverity.Low;
    }

    public class UpdateBugReportDto
    {
        public BugReportStatus Status { get; set; }
        public BugReportSeverity Severity { get; set; }
    }
}
