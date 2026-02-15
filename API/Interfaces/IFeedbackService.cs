using API.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IFeedbackService
    {
        Task<FeedbackDto?> GetFeedbackByIdAsync(Guid id);
        Task<IEnumerable<FeedbackDto>> GetAllFeedbackAsync();
        Task<IEnumerable<FeedbackDto>> GetFeedbackByUserAsync(Guid userId);
        Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto dto, Guid userId);
        Task<FeedbackDto> UpdateFeedbackAsync(Guid id, UpdateFeedbackDto dto);
        Task DeleteFeedbackAsync(Guid id);
        Task<IEnumerable<FeedbackDto>> GetFeedbackByCategoryAsync(string category);
    }
}
