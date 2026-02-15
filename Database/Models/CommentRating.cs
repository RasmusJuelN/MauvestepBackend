using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class CommentRating
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("ForumComment")]
        public Guid ForumCommentId { get; set; }

        [Required]
        public bool IsLike { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        // Navigation properties
        public virtual ForumComment ForumComment { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
