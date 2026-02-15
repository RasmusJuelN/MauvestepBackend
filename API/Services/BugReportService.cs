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
    public class BugReportService : IBugReportService
    {
        private readonly IBugReportRepository _bugReportRepository;
        private readonly IUserRepository _userRepository;

        public BugReportService(IBugReportRepository bugReportRepository, IUserRepository userRepository)
        {
            _bugReportRepository = bugReportRepository;
            _userRepository = userRepository;
        }


        private BugReportDto MapToDto(BugReport bugReport)
        {
            return new BugReportDto
            {
                Id = bugReport.Id,
                UserId = bugReport.UserId,
                Username = bugReport.User?.Username ?? string.Empty,
                Title = bugReport.Title,
                Description = bugReport.Description,
                Category = bugReport.Category,
                Status = bugReport.Status,
                Severity = bugReport.Severity,
                CreatedAt = bugReport.CreatedAt,
                ResolvedAt = bugReport.ResolvedAt
            };
        }
        public async Task<BugReportDto?> GetBugReportByIdAsync(Guid id)
        {
            var bugReport = await _bugReportRepository.GetByIdAsync(id);
            if (bugReport == null)
                return null;

            return MapToDto(bugReport);
        }

        public async Task<IEnumerable<BugReportDto>> GetAllBugReportsAsync()
        {
            var bugReports = await _bugReportRepository.GetAllAsync();
            return bugReports.Select(MapToDto);
        }

        public async Task<IEnumerable<BugReportDto>> GetBugReportsByUserAsync(Guid userId)
        {
            var bugReports = await _bugReportRepository.GetBugReportsByUserAsync(userId);
            return bugReports.Select(MapToDto);
        }

        public async Task<IEnumerable<BugReportDto>> GetBugReportsByStatusAsync(string status)
        {
            if (!Enum.TryParse<BugReportStatus>(status, true, out var bugReportStatus))
                return Enumerable.Empty<BugReportDto>();

            var bugReports = await _bugReportRepository.GetBugReportsByStatusAsync(bugReportStatus);
            return bugReports.Select(MapToDto);
        }

        public async Task<IEnumerable<BugReportDto>> GetBugReportsBySeverityAsync(string severity)
        {
            if (!Enum.TryParse<BugReportSeverity>(severity, true, out var bugReportSeverity))
                return Enumerable.Empty<BugReportDto>();

            var bugReports = await _bugReportRepository.GetBugReportsBySeverityAsync(bugReportSeverity);
            return bugReports.Select(MapToDto);
        }

        public async Task<IEnumerable<BugReportDto>> GetUnresolvedBugReportsAsync()
        {
            var bugReports = await _bugReportRepository.GetOpenBugReportsAsync();
            return bugReports.Select(MapToDto);
        }

        public async Task<IEnumerable<BugReportDto>> GetRecentBugReportsAsync(int count = 10)
        {
            var bugReports = await _bugReportRepository.GetRecentBugReportsAsync(count);
            return bugReports.Select(MapToDto);
        }

        public async Task<BugReportDto> CreateBugReportAsync(CreateBugReportDto dto, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var bugReport = new BugReport
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                Severity = dto.Severity,
                Status = BugReportStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            await _bugReportRepository.AddAsync(bugReport);
            await _bugReportRepository.SaveChangesAsync();

            // Reload to get navigation properties
            bugReport = await _bugReportRepository.GetByIdAsync(bugReport.Id);
            return MapToDto(bugReport!);
        }

        public async Task<BugReportDto> UpdateBugReportAsync(Guid id, UpdateBugReportDto dto)
        {
            var bugReport = await _bugReportRepository.GetByIdAsync(id);
            if (bugReport == null)
                throw new InvalidOperationException("Bug report not found");

            bugReport.Status = dto.Status;
            bugReport.Severity = dto.Severity;

            if (dto.Status == BugReportStatus.Resolved && bugReport.ResolvedAt == null)
            {
                bugReport.ResolvedAt = DateTime.UtcNow;
            }

            await _bugReportRepository.UpdateAsync(bugReport);
            await _bugReportRepository.SaveChangesAsync();

            return MapToDto(bugReport);
        }

        public async Task DeleteBugReportAsync(Guid id)
        {
            var bugReport = await _bugReportRepository.GetByIdAsync(id);
            if (bugReport == null)
                throw new InvalidOperationException("Bug report not found");

            await _bugReportRepository.DeleteAsync(bugReport);
            await _bugReportRepository.SaveChangesAsync();
        }

        
    }
}
