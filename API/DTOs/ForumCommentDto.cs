using System;

namespace API.DTOs
{
    public class ForumCommentDto
    {
        public Guid Id { get; set; }
        public Guid ForumPostId { get; set; }
        public string Content { get; set; } = null!;
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool? UserHasLiked { get; set; } 
    }

    public class CreateForumCommentDto
    {
        public Guid ForumPostId { get; set; }
        public string Content { get; set; } = null!;
    }

    public class UpdateForumCommentDto
    {
        public string Content { get; set; } = null!;
    }
}
