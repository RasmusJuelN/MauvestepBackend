using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class ForumThreadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int PostCount { get; set; }
        public List<ForumPostDto> Posts { get; set; } = new();
    }


    public class CreateThreadWithFirstPostDto
    {
        public string Title { get; set; } = null!;
        public Guid ForumCategoryId { get; set; }
        public string Content { get; set; } = null!;
    }

    public class UpdateForumThreadDto
    {
        public string Title { get; set; } = null!;
    }
}
