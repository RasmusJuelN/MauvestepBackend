using API.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IForumThreadService
    {
        Task<IEnumerable<ForumThreadDto>> GetAllThreadsAsync();
        Task<ForumThreadDto?> GetThreadByIdAsync(Guid id);
        Task<IEnumerable<ForumThreadDto>> GetThreadsByCategoryAsync(Guid categoryId);
        Task<IEnumerable<ForumThreadDto>> GetThreadsByUserAsync(Guid userId);
        Task<IEnumerable<ForumPostDto>> GetThreadPostsAsync(Guid threadId, Guid? currentUserId = null);
        Task<ForumThreadDto> CreateThreadAsync(CreateThreadWithFirstPostDto dto, string firstPostContent, Guid userId);
        Task<ForumThreadDto> UpdateThreadAsync(Guid id, UpdateForumThreadDto dto);
        Task DeleteThreadAsync(Guid id);
    }
}
