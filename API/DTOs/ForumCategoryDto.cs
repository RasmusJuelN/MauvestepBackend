using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class ForumCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int ThreadCount { get; set; }
        public List<ForumThreadDto> Threads { get; set; } = new();
    }

    public class CreateForumCategoryDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class UpdateForumCategoryDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
