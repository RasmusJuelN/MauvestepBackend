using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public enum BugReportStatus
    {
        Open = 1,
        InProgress = 2,
        Resolved = 3,
        Closed = 4
    }

    public enum BugReportSeverity
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }
    public class BugReport
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string Category { get; set; } = null!;

        [Required]
        public BugReportStatus Status { get; set; } = BugReportStatus.Open;

        [Required]
        public BugReportSeverity Severity { get; set; } = BugReportSeverity.Low;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ResolvedAt { get; set; }

        // Navigation property
        public virtual User User { get; set; } = null!;
    }
}
