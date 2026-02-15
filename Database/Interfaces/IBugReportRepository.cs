using Database.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Interfaces
{

    public interface IBugReportRepository : IRepository<BugReport>
    {

        Task<IEnumerable<BugReport>> GetBugReportsByUserAsync(Guid userId);
        Task<IEnumerable<BugReport>> GetBugReportsByStatusAsync(BugReportStatus status);
        Task<IEnumerable<BugReport>> GetOpenBugReportsAsync();
        Task<IEnumerable<BugReport>> GetRecentBugReportsAsync(int count = 10);
        Task<IEnumerable<BugReport>> GetBugReportsBySeverityAsync(BugReportSeverity severity);
    }
}
