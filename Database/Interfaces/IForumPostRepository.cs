using Database.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Interfaces
{

    public interface IForumPostRepository : IRepository<ForumPost>
    {

        Task<IEnumerable<ForumPost>> GetPostsByThreadAsync(Guid threadId);
        Task<ForumPost?> GetPostWithCommentsAsync(Guid postId);

 

    }
}
