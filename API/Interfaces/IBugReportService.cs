using API.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IBugReportService
    {
        Task<BugReportDto?> GetBugReportByIdAsync(Guid id);
        Task<IEnumerable<BugReportDto>> GetAllBugReportsAsync();
        Task<IEnumerable<BugReportDto>> GetBugReportsByUserAsync(Guid userId);
        Task<IEnumerable<BugReportDto>> GetBugReportsByStatusAsync(string status);
        Task<IEnumerable<BugReportDto>> GetBugReportsBySeverityAsync(string severity);
        Task<IEnumerable<BugReportDto>> GetUnresolvedBugReportsAsync();
        Task<IEnumerable<BugReportDto>> GetRecentBugReportsAsync(int count = 10);
        Task<BugReportDto> CreateBugReportAsync(CreateBugReportDto dto, Guid userId);
        Task<BugReportDto> UpdateBugReportAsync(Guid id, UpdateBugReportDto dto);
        Task DeleteBugReportAsync(Guid id);
    }
}
