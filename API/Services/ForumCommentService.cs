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
    public class ForumCommentService : IForumCommentService
    {
        private readonly IForumCommentRepository _commentRepository;
        private readonly IForumPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRatingRepository _commentRatingRepository;

        public ForumCommentService(
            IForumCommentRepository commentRepository,
            IForumPostRepository postRepository,
            IUserRepository userRepository,
            ICommentRatingRepository commentRatingRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _commentRatingRepository = commentRatingRepository;
        }

         private ForumCommentDto MapToDto(ForumComment comment, Guid? currentUserId = null)
        {
            bool? userHasLiked = null;
            if (currentUserId.HasValue && comment.CommentRatings != null)
            {
                var userRating = comment.CommentRatings.FirstOrDefault(r => r.UserId == currentUserId.Value);
                if (userRating != null)
                {
                    userHasLiked = userRating.IsLike;
                }
            }

            return new ForumCommentDto
            {
                Id = comment.Id,
                ForumPostId = comment.ForumPostId,
                Content = comment.Content,
                UserId = comment.UserId,
                Username = comment.User?.Username ?? "Unknown",
                CreatedAt = comment.CreatedAt,
                EditedAt = comment.EditedAt,
                LikeCount = comment.CommentRatings?.Count(r => r.IsLike) ?? 0,
                DislikeCount = comment.CommentRatings?.Count(r => !r.IsLike) ?? 0,
                UserHasLiked = userHasLiked
            };
        }

        public async Task<IEnumerable<ForumCommentDto>> GetAllCommentsAsync(Guid? currentUserId = null)
        {
            var comments = await _commentRepository.GetAllAsync();
            return comments.Select(c => MapToDto(c, currentUserId));
        }

        public async Task<ForumCommentDto?> GetCommentByIdAsync(Guid id, Guid? currentUserId = null)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return comment == null ? null : MapToDto(comment, currentUserId);
        }

        public async Task<IEnumerable<ForumCommentDto>> GetCommentsByPostAsync(Guid postId, Guid? currentUserId = null)
        {
            var comments = await _commentRepository.GetCommentsByPostAsync(postId);
            return comments.Select(c => MapToDto(c, currentUserId));
        }

        public async Task<IEnumerable<ForumCommentDto>> GetCommentsByUserAsync(Guid userId, Guid? currentUserId = null)
        {
            var comments = await _commentRepository.GetCommentsByUserAsync(userId);
            return comments.Select(c => MapToDto(c, currentUserId));
        }

        public async Task<ForumCommentDto> CreateCommentAsync(CreateForumCommentDto dto, Guid userId)
        {
            var post = await _postRepository.GetByIdAsync(dto.ForumPostId);
            if (post == null)
                throw new InvalidOperationException("Post not found");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var comment = new ForumComment
            {
                Id = Guid.NewGuid(),
                ForumPostId = dto.ForumPostId,
                Content = dto.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();
            
            var createdComment = await _commentRepository.GetByIdAsync(comment.Id);
            return MapToDto(createdComment!);
        }

        public async Task<ForumCommentDto> UpdateCommentAsync(Guid id, UpdateForumCommentDto dto)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                throw new InvalidOperationException("Comment not found");

            comment.Content = dto.Content;
            comment.EditedAt = DateTime.UtcNow;

            await _commentRepository.UpdateAsync(comment);
            await _commentRepository.SaveChangesAsync();
            return MapToDto(comment);
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                throw new InvalidOperationException("Comment not found");

            await _commentRepository.DeleteAsync(comment);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task<ForumCommentDto> RateCommentAsync(Guid commentId, bool isLike, Guid userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
                throw new InvalidOperationException("Comment not found");

            var existingRating = await _commentRatingRepository.GetByCommentAndUserAsync(commentId, userId);
            
            if (existingRating != null)
            {
                if (existingRating.IsLike == isLike)
                {
                    await _commentRatingRepository.DeleteAsync(existingRating);
                }
                else
                {

                    existingRating.IsLike = isLike;
                    await _commentRatingRepository.UpdateAsync(existingRating);
                }
            }
            else
            {
                var newRating = new CommentRating
                {
                    Id = Guid.NewGuid(),
                    ForumCommentId = commentId,
                    UserId = userId,
                    IsLike = isLike
                };
                await _commentRatingRepository.AddAsync(newRating);
            }

            await _commentRatingRepository.SaveChangesAsync();

            // Refetch to get updated counts
            var updatedComment = await _commentRepository.GetByIdAsync(commentId);
            return MapToDto(updatedComment!, userId);
        }

       
    }
}
