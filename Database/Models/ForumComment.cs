using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class ForumComment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("ForumPost")]
        public Guid ForumPostId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? EditedAt { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        // Navigation properties
        public virtual ForumPost ForumPost { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<CommentRating> CommentRatings { get; set; } = new List<CommentRating>();
    }
}
