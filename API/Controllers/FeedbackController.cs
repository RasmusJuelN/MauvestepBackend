using API.DTOs;
using API.Interfaces;
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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetAllFeedback()
        {
            var feedback = await _feedbackService.GetAllFeedbackAsync();
            return Ok(feedback);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackDto>> GetFeedbackById(Guid id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            if (feedback == null)
                return NotFound(new { message = "Feedback not found" });

            return Ok(feedback);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetFeedbackByCategory(string category)
        {
            var feedback = await _feedbackService.GetFeedbackByCategoryAsync(category);
            return Ok(feedback);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FeedbackDto>> CreateFeedback(CreateFeedbackDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var feedback = await _feedbackService.CreateFeedbackAsync(dto, userId);
                return CreatedAtAction(nameof(GetFeedbackById), new { id = feedback.Id }, feedback);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<FeedbackDto>> UpdateFeedback(Guid id, UpdateFeedbackDto dto)
        {
            try
            {
                var feedback = await _feedbackService.UpdateFeedbackAsync(id, dto);
                return Ok(feedback);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            try
            {
                await _feedbackService.DeleteFeedbackAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
