using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; } = null!;


    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public string Role { get; set; } = "User";

    public string? Bio { get; set; } = null;
    
    public string? ProfilePictureUrl { get; set; } = null;

    public int LikesCount { get; set; } = 0;
    public int PostCount { get; set; } = 0;

    // JWT/Refresh Token properties
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navigation properties
    public virtual ICollection<Highscore> Highscores { get; set; } = new List<Highscore>();
    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    public virtual ICollection<ForumThread> ForumThreads { get; set; } = new List<ForumThread>();
    public virtual ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();
    public virtual ICollection<ForumComment> ForumComments { get; set; } = new List<ForumComment>();
    public virtual ICollection<CommentRating> CommentRatings { get; set; } = new List<CommentRating>();
    public virtual ICollection<PostRating> PostRatings { get; set; } = new List<PostRating>();
    public virtual ICollection<BugReport> BugReports { get; set; } = new List<BugReport>();
    public virtual ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
}
