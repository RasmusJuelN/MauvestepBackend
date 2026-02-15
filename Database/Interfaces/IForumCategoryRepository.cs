using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Interfaces
{

    public interface IForumCategoryRepository : IRepository<ForumCategory>
    {

        Task<ForumCategory?> GetByNameAsync(string name);
        Task<ForumCategory?> GetCategoryWithThreadsAsync(Guid categoryId);
    }
}
