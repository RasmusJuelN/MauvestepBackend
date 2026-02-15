using Database.Models;
using System;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    public interface IPostRatingRepository : IRepository<PostRating>
    {
        Task<PostRating?> GetByPostAndUserAsync(Guid postId, Guid userId);
    }
}
