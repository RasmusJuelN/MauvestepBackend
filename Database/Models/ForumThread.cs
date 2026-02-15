using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class ForumThread
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("ForumCategory")]
        public Guid ForumCategoryId { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        // Navigation properties
        public virtual ForumCategory ForumCategory { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();
    }
}
