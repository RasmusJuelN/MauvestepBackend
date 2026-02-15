using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Highscore> Highscores => Set<Highscore>();
        public DbSet<NewsArticle> NewsArticles => Set<NewsArticle>();
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<Ability> Abilities => Set<Ability>();
        public DbSet<ForumCategory> ForumCategories => Set<ForumCategory>();
        public DbSet<ForumThread> ForumThreads => Set<ForumThread>();
        public DbSet<ForumPost> ForumPosts => Set<ForumPost>();
        public DbSet<ForumComment> ForumComments => Set<ForumComment>();
        public DbSet<CommentRating> CommentRatings => Set<CommentRating>();
        public DbSet<PostRating> PostRatings => Set<PostRating>();
        public DbSet<FAQ> FAQs => Set<FAQ>();
        public DbSet<BugReport> BugReports => Set<BugReport>();
        public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
        public DbSet<Feedback> Feedbacks => Set<Feedback>();

    }

}
