using API.DTOs;
using API.Interfaces;
using Database.Interfaces;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUserRepository userRepository)
        {
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
        }

        private FeedbackDto MapToDto(Feedback feedback)
        {
            return new FeedbackDto
            {
                Id = feedback.Id,
                UserId = feedback.UserId,
                Username = feedback.User?.Username ?? string.Empty,
                Title = feedback.Title,
                Content = feedback.Content,
                Category = feedback.Category,
                Rating = feedback.Rating,
                CreatedAt = feedback.CreatedAt
            };
        }

        public async Task<FeedbackDto?> GetFeedbackByIdAsync(Guid id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
                return null;

            return MapToDto(feedback);
        }

        public async Task<IEnumerable<FeedbackDto>> GetAllFeedbackAsync()
        {
            var feedbacks = await _feedbackRepository.GetAllAsync();
            return feedbacks.Select(MapToDto);
        }


        public async Task<IEnumerable<FeedbackDto>> GetFeedbackByCategoryAsync(string category)
        {
            var feedbacks = await _feedbackRepository.GetFeedbackByCategoryAsync(category);
            return feedbacks.Select(MapToDto);
        }

        public async Task<IEnumerable<FeedbackDto>> GetFeedbackByUserAsync(Guid userId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbackByUserAsync(userId);
            return feedbacks.Select(MapToDto);
        }

        public async Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto dto, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = dto.Title,
                Content = dto.Content,
                Category = dto.Category,
                Rating = dto.Rating,
                CreatedAt = DateTime.UtcNow
            };

            await _feedbackRepository.AddAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();

            // Reload to get navigation properties
            feedback = await _feedbackRepository.GetByIdAsync(feedback.Id);
            return MapToDto(feedback!);
        }

        public async Task<FeedbackDto> UpdateFeedbackAsync(Guid id, UpdateFeedbackDto dto)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
                throw new InvalidOperationException("Feedback not found");

            feedback.Title = dto.Title;
            feedback.Content = dto.Content;
            feedback.Category = dto.Category;
            feedback.Rating = dto.Rating;

            await _feedbackRepository.UpdateAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();

            return MapToDto(feedback);
        }

        public async Task DeleteFeedbackAsync(Guid id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
                throw new InvalidOperationException("Feedback not found");

            await _feedbackRepository.DeleteAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();
        }

        
    }
}
