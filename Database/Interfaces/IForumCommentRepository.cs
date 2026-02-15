using Database.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Interfaces
{

    public interface IForumCommentRepository : IRepository<ForumComment>
    {

        Task<IEnumerable<ForumComment>> GetCommentsByPostAsync(Guid postId);
        Task<IEnumerable<ForumComment>> GetCommentsByUserAsync(Guid userId);


    }
}
