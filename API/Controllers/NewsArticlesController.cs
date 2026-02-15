using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsArticlesController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsArticleDto>>> GetAllArticles()
        {
            var articles = await _newsArticleService.GetAllArticlesAsync();
            return Ok(articles);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<NewsArticleDto>> GetArticleById(Guid id)
        {
            var article = await _newsArticleService.GetArticleByIdAsync(id);
            if (article == null)
                return NotFound(new { message = "Article not found" });

            return Ok(article);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<NewsArticleDto>> CreateArticle(CreateNewsArticleDto dto)
        {
            var article = await _newsArticleService.CreateArticleAsync(dto);
            return CreatedAtAction(nameof(GetArticleById), new { id = article.Id }, article);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(Guid id, UpdateNewsArticleDto dto)
        {
            var article = await _newsArticleService.UpdateArticleAsync(id, dto);
            return Ok(article);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            await _newsArticleService.DeleteArticleAsync(id);
            return NoContent();
        }
    }
}
