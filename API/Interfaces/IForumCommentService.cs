using API.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IForumCommentService
    {
        Task<IEnumerable<ForumCommentDto>> GetAllCommentsAsync(Guid? currentUserId = null);
        Task<ForumCommentDto?> GetCommentByIdAsync(Guid id, Guid? currentUserId = null);
        Task<IEnumerable<ForumCommentDto>> GetCommentsByPostAsync(Guid postId, Guid? currentUserId = null);
        Task<IEnumerable<ForumCommentDto>> GetCommentsByUserAsync(Guid userId, Guid? currentUserId = null);
        Task<ForumCommentDto> CreateCommentAsync(CreateForumCommentDto dto, Guid userId);
        Task<ForumCommentDto> UpdateCommentAsync(Guid id, UpdateForumCommentDto dto);
        Task DeleteCommentAsync(Guid id);
        Task<ForumCommentDto> RateCommentAsync(Guid commentId, bool isLike, Guid userId);
    }
}
