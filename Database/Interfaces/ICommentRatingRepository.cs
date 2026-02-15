using Database.Models;
using System;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    public interface ICommentRatingRepository : IRepository<CommentRating>
    {
        Task<CommentRating?> GetByCommentAndUserAsync(Guid commentId, Guid userId);
    }
}
