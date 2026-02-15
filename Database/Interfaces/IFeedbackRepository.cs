using Database.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Interfaces
{

    public interface IFeedbackRepository : IRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetFeedbackByCategoryAsync(string category);

        Task<IEnumerable<Feedback>> GetFeedbackByUserAsync(Guid userId);

    }
}
