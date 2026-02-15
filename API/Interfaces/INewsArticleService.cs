using API.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface INewsArticleService
    {
        Task<NewsArticleDto?> GetArticleByIdAsync(Guid id);
        Task<IEnumerable<NewsArticleDto>> GetAllArticlesAsync();
        Task<IEnumerable<NewsArticleDto>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<NewsArticleDto> CreateArticleAsync(CreateNewsArticleDto dto);
        Task<NewsArticleDto> UpdateArticleAsync(Guid id, UpdateNewsArticleDto dto);
        Task DeleteArticleAsync(Guid id);
    }
}
