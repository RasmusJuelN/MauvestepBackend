using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class PostRating
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("ForumPost")]
        public Guid ForumPostId { get; set; }

        [Required]
        public bool IsLike { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        // Navigation properties
        public virtual ForumPost ForumPost { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
