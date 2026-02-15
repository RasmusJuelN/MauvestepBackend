using System;

namespace API.DTOs
{
    public class NewsArticleDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? NewsImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateNewsArticleDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? NewsImageUrl { get; set; }
    }

    public class UpdateNewsArticleDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? NewsImageUrl { get; set; }
    }
}
