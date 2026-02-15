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
    public class ForumCategoryService : IForumCategoryService
    {
        private readonly IForumCategoryRepository _categoryRepository;

        public ForumCategoryService(IForumCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        private ForumCategoryDto MapToDto(ForumCategory category)
        {
            return new ForumCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                ThreadCount = category.ForumThreads?.Count ?? 0
            };
        }

        public async Task<IEnumerable<ForumCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(MapToDto);
        }

        public async Task<ForumCategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null ? null : MapToDto(category);
        }

        public async Task<IEnumerable<ForumThreadDto>> GetCategoryThreadsAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetCategoryWithThreadsAsync(categoryId);
            if (category == null)
                throw new InvalidOperationException("Category not found");

            return category.ForumThreads.Select(t => new ForumThreadDto
            {
                Id = t.Id,
                Title = t.Title,
                CategoryId = t.ForumCategoryId,
                UserId = t.UserId,
                Username = t.User?.Username ?? "Unknown",
                CreatedAt = t.CreatedAt,
                PostCount = t.ForumPosts.Count
            });
        }

        public async Task<ForumCategoryDto> CreateCategoryAsync(CreateForumCategoryDto dto)
        {
            var category = new ForumCategory
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
            return MapToDto(category);
        }

        public async Task<ForumCategoryDto> UpdateCategoryAsync(Guid id, UpdateForumCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new InvalidOperationException("Category not found");

            category.Name = dto.Name;
            category.Description = dto.Description;

            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();
            return MapToDto(category);
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new InvalidOperationException("Category not found");

            await _categoryRepository.DeleteAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        
    }
}
