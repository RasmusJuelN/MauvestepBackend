using API.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IForumCategoryService
    {
        Task<IEnumerable<ForumCategoryDto>> GetAllCategoriesAsync();
        Task<ForumCategoryDto?> GetCategoryByIdAsync(Guid id);
        Task<IEnumerable<ForumThreadDto>> GetCategoryThreadsAsync(Guid categoryId);
        Task<ForumCategoryDto> CreateCategoryAsync(CreateForumCategoryDto dto);
        Task<ForumCategoryDto> UpdateCategoryAsync(Guid id, UpdateForumCategoryDto dto);
        Task DeleteCategoryAsync(Guid id);
    }
}
