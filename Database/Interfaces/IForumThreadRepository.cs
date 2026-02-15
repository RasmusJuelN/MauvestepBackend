using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Interfaces
{

    public interface IForumThreadRepository : IRepository<ForumThread>
    {

        Task<IEnumerable<ForumThread>> GetThreadsByCategoryAsync(Guid categoryId);
        Task<IEnumerable<ForumThread>> GetThreadsByUserAsync(Guid userId);
        Task<ForumThread?> GetThreadWithPostsAsync(Guid threadId);
    }
}
