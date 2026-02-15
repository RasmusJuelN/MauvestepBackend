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
    public class ForumPostsController : ControllerBase
    {
        private readonly IForumPostService _postService;

        public ForumPostsController(IForumPostService postService)
        {
            _postService = postService;
        }

        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim != null ? Guid.Parse(userIdClaim) : null;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ForumPostDto>> GetPostById(Guid id)
        {
            var currentUserId = GetCurrentUserId();
            var post = await _postService.GetPostByIdAsync(id, currentUserId);
            if (post == null)
                return NotFound(new { message = "Post not found" });

            return Ok(post);
        }

 
        [HttpGet("{id}/comments")]
        public async Task<ActionResult<IEnumerable<ForumCommentDto>>> GetPostComments(Guid id)
        {
            var currentUserId = GetCurrentUserId();
            var comments = await _postService.GetPostCommentsAsync(id, currentUserId);
            return Ok(comments);
        }

        [Authorize]
        [HttpPost("{id}/rate")]
        public async Task<ActionResult<ForumPostDto>> RatePost(Guid id, [FromBody] RateDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            try
            {
                var updatedPost = await _postService.RatePostAsync(id, dto.IsLike, userId);
                return Ok(updatedPost);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

 
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ForumPostDto>> CreatePost(CreateForumPostDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var post = await _postService.CreatePostAsync(dto, userId);
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

 
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ForumPostDto>> UpdatePost(Guid id, UpdateForumPostDto dto)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(id);
                if (post == null)
                    return NotFound(new { message = "Post not found" });

                // only owner or admin
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (!User.IsInRole("Admin") && post.UserId != userId)
                    return Forbid();

                var updatedPost = await _postService.UpdatePostAsync(id, dto);
                return Ok(updatedPost);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(id);
                if (post == null)
                    return NotFound(new { message = "Post not found" });

                // only owner or admin
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (!User.IsInRole("Admin") && post.UserId != userId)
                    return Forbid();

                await _postService.DeletePostAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
