using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class ForumPost
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("ForumThread")]
        public Guid ForumThreadId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? EditedAt { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        // Navigation properties
        public virtual ForumThread ForumThread { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ForumComment> ForumComments { get; set; } = new List<ForumComment>();
        public virtual ICollection<PostRating> PostRatings { get; set; } = new List<PostRating>();
    }
}
