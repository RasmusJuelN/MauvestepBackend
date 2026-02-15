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
    public class ForumThreadsController : ControllerBase
    {
        private readonly IForumThreadService _threadService;

        public ForumThreadsController(IForumThreadService threadService)
        {
            _threadService = threadService;
        }

        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim != null ? Guid.Parse(userIdClaim) : null;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ForumThreadDto>> GetThreadById(Guid id)
        {
            var thread = await _threadService.GetThreadByIdAsync(id);
            if (thread == null)
                return NotFound(new { message = "Thread not found" });

            return Ok(thread);
        }

        [HttpGet("{id}/posts")]
        public async Task<ActionResult<IEnumerable<ForumPostDto>>> GetThreadPosts(Guid id)
        {
            var currentUserId = GetCurrentUserId();
            var posts = await _threadService.GetThreadPostsAsync(id, currentUserId);
            return Ok(posts);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ForumThreadDto>> CreateThread(CreateThreadWithFirstPostDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var threadDto = new CreateThreadWithFirstPostDto 
                {
                    Title = dto.Title,
                    ForumCategoryId = dto.ForumCategoryId
                };

                var thread = await _threadService.CreateThreadAsync(threadDto, dto.Content, userId);
                return CreatedAtAction(nameof(GetThreadById), new { id = thread.Id }, thread);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ForumThreadDto>> UpdateThread(Guid id, UpdateForumThreadDto dto)
        {
            try
            {
                var thread = await _threadService.GetThreadByIdAsync(id);
                if (thread == null)
                    return NotFound(new { message = "Thread not found" });

                // only owner or admin
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (!User.IsInRole("Admin") && thread.UserId != userId)
                    return Forbid();

                var updatedThread = await _threadService.UpdateThreadAsync(id, dto);
                return Ok(updatedThread);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThread(Guid id)
        {
            try
            {
                var thread = await _threadService.GetThreadByIdAsync(id);
                if (thread == null)
                    return NotFound(new { message = "Thread not found" });

                //  only owner or admin
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (!User.IsInRole("Admin") && thread.UserId != userId)
                    return Forbid();

                await _threadService.DeleteThreadAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

    
}
