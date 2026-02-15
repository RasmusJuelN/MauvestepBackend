using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class ForumPostDto
    {
        public Guid Id { get; set; }
        public Guid ForumThreadId { get; set; }
        public string Content { get; set; } = null!;
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? UserProfilePictureUrl { get; set; }
        public int UserPostCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool? UserHasLiked { get; set; } 
        public List<ForumCommentDto> Comments { get; set; } = new();
    }

    public class CreateForumPostDto
    {
        public Guid ForumThreadId { get; set; }
        public string Content { get; set; } = null!;
    }

    public class UpdateForumPostDto
    {
        public string Content { get; set; } = null!;
    }
}
