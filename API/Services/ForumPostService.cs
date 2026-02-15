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
    public class ForumPostService : IForumPostService
    {
        private readonly IForumPostRepository _postRepository;
        private readonly IForumThreadRepository _threadRepository;
        private readonly IForumCommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRatingRepository _postRatingRepository;

        public ForumPostService(
            IForumPostRepository postRepository,
            IForumThreadRepository threadRepository,
            IForumCommentRepository commentRepository,
            IUserRepository userRepository,
            IPostRatingRepository postRatingRepository)
        {
            _postRepository = postRepository;
            _threadRepository = threadRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _postRatingRepository = postRatingRepository;
        }
        
        private ForumPostDto MapToDto(ForumPost post, Guid? currentUserId = null)
        {
            // check if the current user has already liked a post and set the UserHasLiked value to send with DTO
            bool? userHasLiked = null;
            if (currentUserId.HasValue && post.PostRatings != null)
            {
                var userRating = post.PostRatings.FirstOrDefault(r => r.UserId == currentUserId.Value);
                if (userRating != null)
                {
                    userHasLiked = userRating.IsLike;
                }
            }

            return new ForumPostDto
            {
                Id = post.Id,
                ForumThreadId = post.ForumThreadId,
                Content = post.Content,
                UserId = post.UserId,
                Username = post.User?.Username ?? "Unknown",
                UserProfilePictureUrl = post.User?.ProfilePictureUrl,
                UserPostCount = post.User?.PostCount ?? 0,
                CreatedAt = post.CreatedAt,
                EditedAt = post.EditedAt,
                LikeCount = post.PostRatings?.Count(r => r.IsLike) ?? 0,
                DislikeCount = post.PostRatings?.Count(r => !r.IsLike) ?? 0,
                UserHasLiked = userHasLiked
            };
        }

        public async Task<IEnumerable<ForumPostDto>> GetAllPostsAsync(Guid? currentUserId = null)
        {
            var posts = await _postRepository.GetAllAsync();
            return posts.Select(p => MapToDto(p, currentUserId));
        }

        public async Task<ForumPostDto?> GetPostByIdAsync(Guid id, Guid? currentUserId = null)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return post == null ? null : MapToDto(post, currentUserId);
        }

        public async Task<IEnumerable<ForumPostDto>> GetPostsByThreadAsync(Guid threadId, Guid? currentUserId = null)
        {
            var posts = await _postRepository.GetPostsByThreadAsync(threadId);
            return posts.Select(p => MapToDto(p, currentUserId));
        }



        public async Task<IEnumerable<ForumCommentDto>> GetPostCommentsAsync(Guid postId, Guid? currentUserId = null)
        {
            var comments = await _commentRepository.GetCommentsByPostAsync(postId);
            return comments.Select(c =>
            {
                bool? userHasLiked = null;
                if (currentUserId.HasValue && c.CommentRatings != null)
                {
                    var userRating = c.CommentRatings.FirstOrDefault(r => r.UserId == currentUserId.Value);
                    if (userRating != null)
                    {
                        userHasLiked = userRating.IsLike;
                    }
                }

                return new ForumCommentDto
                {
                    Id = c.Id,
                    ForumPostId = c.ForumPostId,
                    Content = c.Content,
                    UserId = c.UserId,
                    Username = c.User?.Username ?? "Unknown",
                    CreatedAt = c.CreatedAt,
                    EditedAt = c.EditedAt,
                    LikeCount = c.CommentRatings?.Count(r => r.IsLike) ?? 0,
                    DislikeCount = c.CommentRatings?.Count(r => !r.IsLike) ?? 0,
                    UserHasLiked = userHasLiked
                };
            });
        }

        public async Task<ForumPostDto> CreatePostAsync(CreateForumPostDto dto, Guid userId)
        {
            var thread = await _threadRepository.GetByIdAsync(dto.ForumThreadId);
            if (thread == null)
                throw new InvalidOperationException("Thread not found");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var post = new ForumPost
            {
                Id = Guid.NewGuid(),
                ForumThreadId = dto.ForumThreadId,
                Content = dto.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();
            
            // Fetch the post with all navigation properties loaded
            var createdPost = await _postRepository.GetByIdAsync(post.Id);
            return MapToDto(createdPost!);
        }

        public async Task<ForumPostDto> UpdatePostAsync(Guid id, UpdateForumPostDto dto)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
                throw new InvalidOperationException("Post not found");

            post.Content = dto.Content;
            post.EditedAt = DateTime.UtcNow;

            await _postRepository.UpdateAsync(post);
            await _postRepository.SaveChangesAsync();
            return MapToDto(post);
        }

        public async Task DeletePostAsync(Guid id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
                throw new InvalidOperationException("Post not found");

            await _postRepository.DeleteAsync(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task<ForumPostDto> RatePostAsync(Guid postId, bool isLike, Guid userId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
                throw new InvalidOperationException("Post not found");

            var existingRating = await _postRatingRepository.GetByPostAndUserAsync(postId, userId);

            if (existingRating != null)
            {
                if (existingRating.IsLike == isLike)
                {
                    // Same rating - remove it (toggle off)
                    await _postRatingRepository.DeleteAsync(existingRating);
                }
                else
                {
                    // Different rating - update it
                    existingRating.IsLike = isLike;
                    await _postRatingRepository.UpdateAsync(existingRating);
                }
            }
            else
            {
                // No existing rating - create new
                var newRating = new PostRating
                {
                    Id = Guid.NewGuid(),
                    ForumPostId = postId,
                    UserId = userId,
                    IsLike = isLike
                };
                await _postRatingRepository.AddAsync(newRating);
            }

            await _postRatingRepository.SaveChangesAsync();
            
            // Fetch updated post with ratings
            var updatedPost = await _postRepository.GetByIdAsync(postId);
            return MapToDto(updatedPost!, userId);
        }
    }
}
