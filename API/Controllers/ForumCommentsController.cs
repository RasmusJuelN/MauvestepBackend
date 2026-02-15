using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumCommentsController : ControllerBase
    {
        private readonly IForumCommentService _commentService;

        public ForumCommentsController(IForumCommentService commentService)
        {
            _commentService = commentService;
        }

        [Authorize]
        [HttpPost("{id}/rate")]
        public async Task<ActionResult<ForumCommentDto>> RateComment(Guid id, [FromBody] RateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var updatedComment = await _commentService.RateCommentAsync(id, dto.IsLike, userId);
                return Ok(updatedComment);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"InvalidOperationException in RateComment: {ex.Message}");
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ForumCommentDto>> CreateComment(CreateForumCommentDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var comment = await _commentService.CreateCommentAsync(dto, userId);
                return Ok(comment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ForumCommentDto>> UpdateComment(Guid id, UpdateForumCommentDto dto)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                if (comment == null)
                    return NotFound(new { message = "Comment not found" });

                // only owner or admin
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (!User.IsInRole("Admin") && comment.UserId != userId)
                    return Forbid();

                var updatedComment = await _commentService.UpdateCommentAsync(id, dto);
                return Ok(updatedComment);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                if (comment == null)
                    return NotFound(new { message = "Comment not found" });

                // only owner or admin
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (!User.IsInRole("Admin") && comment.UserId != userId)
                    return Forbid();

                await _commentService.DeleteCommentAsync(id);
  
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
