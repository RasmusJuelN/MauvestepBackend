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
    public class ForumThreadService : IForumThreadService
    {
        private readonly IForumThreadRepository _threadRepository;
        private readonly IForumPostRepository _postRepository;
        private readonly IForumCategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public ForumThreadService(
            IForumThreadRepository threadRepository,
            IForumPostRepository postRepository,
            IForumCategoryRepository categoryRepository,
            IUserRepository userRepository)
        {
            _threadRepository = threadRepository;
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        private ForumThreadDto MapToDto(ForumThread thread)
        {
            return new ForumThreadDto
            {
                Id = thread.Id,
                Title = thread.Title,
                CategoryId = thread.ForumCategoryId,
                UserId = thread.UserId,
                Username = thread.User?.Username ?? "Unknown",
                CreatedAt = thread.CreatedAt,
                PostCount = thread.ForumPosts?.Count ?? 0
            };
        }

        public async Task<IEnumerable<ForumThreadDto>> GetAllThreadsAsync()
        {
            var threads = await _threadRepository.GetAllAsync();
            return threads.Select(MapToDto);
        }

        public async Task<ForumThreadDto?> GetThreadByIdAsync(Guid id)
        {
            var thread = await _threadRepository.GetByIdAsync(id);
            return thread == null ? null : MapToDto(thread);
        }

        public async Task<IEnumerable<ForumThreadDto>> GetThreadsByCategoryAsync(Guid categoryId)
        {
            var threads = await _threadRepository.GetThreadsByCategoryAsync(categoryId);
            return threads.Select(MapToDto);
        }

        public async Task<IEnumerable<ForumThreadDto>> GetThreadsByUserAsync(Guid userId)
        {
            var threads = await _threadRepository.GetThreadsByUserAsync(userId);
            return threads.Select(MapToDto);
        }

        public async Task<IEnumerable<ForumPostDto>> GetThreadPostsAsync(Guid threadId, Guid? currentUserId = null)
        {
            var posts = await _postRepository.GetPostsByThreadAsync(threadId);
            return posts.Select(p =>
            {
                bool? userHasLiked = null;
                if (currentUserId.HasValue && p.PostRatings != null)
                {
                    var userRating = p.PostRatings.FirstOrDefault(r => r.UserId == currentUserId.Value);
                    if (userRating != null)
                    {
                        userHasLiked = userRating.IsLike;
                    }
                }

                return new ForumPostDto
                {
                    Id = p.Id,
                    ForumThreadId = p.ForumThreadId,
                    Content = p.Content,
                    UserId = p.UserId,
                    Username = p.User?.Username ?? "Unknown",
                    CreatedAt = p.CreatedAt,
                    EditedAt = p.EditedAt,
                    LikeCount = p.PostRatings?.Count(r => r.IsLike) ?? 0,
                    DislikeCount = p.PostRatings?.Count(r => !r.IsLike) ?? 0,
                    UserHasLiked = userHasLiked
                };
            });
        }

        public async Task<ForumThreadDto> CreateThreadAsync(CreateThreadWithFirstPostDto dto, string firstPostContent, Guid userId)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.ForumCategoryId);
            if (category == null)
                throw new InvalidOperationException("Category not found");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var thread = new ForumThread
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                ForumCategoryId = dto.ForumCategoryId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _threadRepository.AddAsync(thread);

            // Creates first post in the new thread
            var firstPost = new ForumPost
            {
                Id = Guid.NewGuid(),
                ForumThreadId = thread.Id,
                Content = firstPostContent,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _postRepository.AddAsync(firstPost);
            await _postRepository.SaveChangesAsync();

            var createdThread = await _threadRepository.GetByIdAsync(thread.Id);
            return MapToDto(createdThread!);
        }

        public async Task<ForumThreadDto> UpdateThreadAsync(Guid id, UpdateForumThreadDto dto)
        {
            var thread = await _threadRepository.GetByIdAsync(id);
            if (thread == null)
                throw new InvalidOperationException("Thread not found");

            thread.Title = dto.Title;
            await _threadRepository.UpdateAsync(thread);
            await _threadRepository.SaveChangesAsync();
            return MapToDto(thread);
        }

        public async Task DeleteThreadAsync(Guid id)
        {
            var thread = await _threadRepository.GetByIdAsync(id);
            if (thread == null)
                throw new InvalidOperationException("Thread not found");

            await _threadRepository.DeleteAsync(thread);
            await _threadRepository.SaveChangesAsync();
        }

        
    }
}
