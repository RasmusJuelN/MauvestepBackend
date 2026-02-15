using API.DTOs;
using Database.Interfaces;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{

    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsRepository;
        private readonly IUserRepository _userRepository;

        public NewsArticleService(INewsArticleRepository newsRepository, IUserRepository userRepository)
        {
            _newsRepository = newsRepository;
            _userRepository = userRepository;
        }


        private static NewsArticleDto MapToDto(NewsArticle article)
        {
            return new NewsArticleDto
            {
                Id = article.Id,
                UserId = article.UserId,
                Username = article.User?.Username ?? "Unknown",
                Title = article.Title,
                Content = article.Content,
                NewsImageUrl = article.NewsImageUrl,
                CreatedAt = article.CreatedAt
            };
        }


        public async Task<IEnumerable<NewsArticleDto>> GetAllArticlesAsync()
        {
            var articles = await _newsRepository.GetAllAsync();
            return articles.Select(MapToDto).ToList();
        }


        public async Task<NewsArticleDto?> GetArticleByIdAsync(Guid id)
        {
            var article = await _newsRepository.GetByIdAsync(id);
            return article == null ? null : MapToDto(article);
        }


        public async Task<IEnumerable<NewsArticleDto>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var articles = await _newsRepository.GetArticlesByDateRangeAsync(startDate, endDate);
            return articles.Select(MapToDto).ToList();
        }

 
        public async Task<NewsArticleDto> CreateArticleAsync(CreateNewsArticleDto dto)
        {
            // TODO: Get current user from context/claims

            var adminUser = await _userRepository.GetByUsernameAsync("admin");
            if (adminUser == null)
            {
                throw new InvalidOperationException("Admin user not found");
            }

            var article = new NewsArticle
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Content = dto.Content,
                NewsImageUrl = dto.NewsImageUrl,
                UserId = adminUser.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _newsRepository.AddAsync(article);
            await _newsRepository.SaveChangesAsync();

            // Reload to get the User navigation property populated
            var createdArticle = await _newsRepository.GetByIdAsync(article.Id);
            return MapToDto(createdArticle!);
        }


        public async Task<NewsArticleDto> UpdateArticleAsync(Guid id, UpdateNewsArticleDto dto)
        {
            var article = await _newsRepository.GetByIdAsync(id);
            if (article == null)
            {
                throw new KeyNotFoundException($"Article with ID {id} not found");
            }

            article.Title = dto.Title;
            article.Content = dto.Content;
            article.NewsImageUrl = dto.NewsImageUrl;

            await _newsRepository.UpdateAsync(article);
            await _newsRepository.SaveChangesAsync();

            // Reload to ensure we have latest data with navigation properties
            var updatedArticle = await _newsRepository.GetByIdAsync(id);
            return MapToDto(updatedArticle!);
        }

        public async Task DeleteArticleAsync(Guid id)
        {
            var article = await _newsRepository.GetByIdAsync(id);
            if (article == null)
            {
                throw new KeyNotFoundException($"Article with ID {id} not found");
            }

            await _newsRepository.DeleteAsync(article);
            await _newsRepository.SaveChangesAsync();
        }

    }
}
