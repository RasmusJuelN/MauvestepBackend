using API.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IForumPostService
    {
        Task<IEnumerable<ForumPostDto>> GetAllPostsAsync(Guid? currentUserId = null);
        Task<ForumPostDto?> GetPostByIdAsync(Guid id, Guid? currentUserId = null);
        Task<IEnumerable<ForumPostDto>> GetPostsByThreadAsync(Guid threadId, Guid? currentUserId = null);
        Task<IEnumerable<ForumCommentDto>> GetPostCommentsAsync(Guid postId, Guid? currentUserId = null);
        Task<ForumPostDto> CreatePostAsync(CreateForumPostDto dto, Guid userId);
        Task<ForumPostDto> UpdatePostAsync(Guid id, UpdateForumPostDto dto);
        Task DeletePostAsync(Guid id);
        Task<ForumPostDto> RatePostAsync(Guid postId, bool isLike, Guid userId);
    }
}
