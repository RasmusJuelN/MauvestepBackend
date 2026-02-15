using API.DTOs;
using API.Interfaces;
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
    public class SupportTicketsController : ControllerBase
    {
        private readonly ISupportTicketService _supportTicketService;

        public SupportTicketsController(ISupportTicketService supportTicketService)
        {
            _supportTicketService = supportTicketService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupportTicketDto>>> GetAllTickets()
        {
            var tickets = await _supportTicketService.GetAllTicketsAsync();
            return Ok(tickets);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("open")]
        public async Task<ActionResult<IEnumerable<SupportTicketDto>>> GetOpenTickets()
        {
            var tickets = await _supportTicketService.GetOpenTicketsAsync();
            return Ok(tickets);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<SupportTicketDto>>> GetTicketsByStatus(string status)
        {
            var tickets = await _supportTicketService.GetTicketsByStatusAsync(status);
            return Ok(tickets);
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<SupportTicketDto>> GetTicketById(Guid id)
        {
            var ticket = await _supportTicketService.GetTicketByIdAsync(id);
            if (ticket == null)
                return NotFound(new { message = "Ticket not found" });

            // Allow access if user is admin or owns the ticket
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (!User.IsInRole("Admin") && ticket.UserId != userId)
                return Forbid();

            return Ok(ticket);
        }

  
        [Authorize]
        [HttpGet("my-tickets")]
        public async Task<ActionResult<IEnumerable<SupportTicketDto>>> GetMyTickets()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var tickets = await _supportTicketService.GetTicketsByUserAsync(userId);
            return Ok(tickets);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<SupportTicketDto>> CreateTicket(CreateSupportTicketDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var ticket = await _supportTicketService.CreateTicketAsync(dto, userId);
                return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<SupportTicketDto>> UpdateTicket(Guid id, UpdateSupportTicketDto dto)
        {
            try
            {
                var ticket = await _supportTicketService.UpdateTicketAsync(id, dto);
                return Ok(ticket);
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
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            try
            {
                await _supportTicketService.DeleteTicketAsync(id);
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
