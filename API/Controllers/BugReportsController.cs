using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BugReportsController : ControllerBase
    {
        private readonly IBugReportService _bugReportService;

        public BugReportsController(IBugReportService bugReportService)
        {
            _bugReportService = bugReportService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BugReportDto>>> GetAllBugReports()
        {
            var bugReports = await _bugReportService.GetAllBugReportsAsync();
            return Ok(bugReports);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<BugReportDto>>> GetBugReportsByStatus(string status)
        {
            var bugReports = await _bugReportService.GetBugReportsByStatusAsync(status);
            return Ok(bugReports);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("severity/{severity}")]
        public async Task<ActionResult<IEnumerable<BugReportDto>>> GetBugReportsBySeverity(string severity)
        {
            var bugReports = await _bugReportService.GetBugReportsBySeverityAsync(severity);
            return Ok(bugReports);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("unresolved")]
        public async Task<ActionResult<IEnumerable<BugReportDto>>> GetUnresolvedBugReports()
        {
            var bugReports = await _bugReportService.GetUnresolvedBugReportsAsync();
            return Ok(bugReports);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<BugReportDto>> GetBugReportById(Guid id)
        {
            var bugReport = await _bugReportService.GetBugReportByIdAsync(id);
            if (bugReport == null)
                return NotFound(new { message = "Bug report not found" });

            // Allow access if user is admin or owns the bug report
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (!User.IsInRole("Admin") && bugReport.UserId != userId)
                return Forbid();

            return Ok(bugReport);
        }

        [Authorize]
        [HttpGet("my-reports")]
        public async Task<ActionResult<IEnumerable<BugReportDto>>> GetMyBugReports()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var bugReports = await _bugReportService.GetBugReportsByUserAsync(userId);
            return Ok(bugReports);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BugReportDto>> CreateBugReport(CreateBugReportDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var bugReport = await _bugReportService.CreateBugReportAsync(dto, userId);
                return CreatedAtAction(nameof(GetBugReportById), new { id = bugReport.Id }, bugReport);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<BugReportDto>> UpdateBugReport(Guid id, UpdateBugReportDto dto)
        {
            try
            {
                var bugReport = await _bugReportService.UpdateBugReportAsync(id, dto);
                return Ok(bugReport);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBugReport(Guid id)
        {
            try
            {
                await _bugReportService.DeleteBugReportAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
