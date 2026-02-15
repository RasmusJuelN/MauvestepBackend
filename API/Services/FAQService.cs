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
    public class FAQService : IFAQService
    {
        private readonly IFAQRepository _faqRepository;

        public FAQService(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        private FAQDto MapToDto(FAQ faq)
        {
            return new FAQDto
            {
                Id = faq.Id,
                Question = faq.Question,
                Answer = faq.Answer,
                Category = faq.Category
            };
        }

        public async Task<FAQDto?> GetFAQByIdAsync(Guid id)
        {
            var faq = await _faqRepository.GetByIdAsync(id);
            if (faq == null)
                return null;

            return MapToDto(faq);
        }

        public async Task<IEnumerable<FAQDto>> GetAllFAQsAsync()
        {
            var faqs = await _faqRepository.GetAllAsync();
            return faqs.Select(MapToDto);
        }

        public async Task<FAQDto> CreateFAQAsync(CreateFAQDto dto)
        {
            var faq = new FAQ
            {
                Id = Guid.NewGuid(),
                Question = dto.Question,
                Answer = dto.Answer,
                Category = dto.Category
            };

            await _faqRepository.AddAsync(faq);
            await _faqRepository.SaveChangesAsync();

            return MapToDto(faq);
        }

        public async Task<FAQDto> UpdateFAQAsync(Guid id, UpdateFAQDto dto)
        {
            var faq = await _faqRepository.GetByIdAsync(id);
            if (faq == null)
                throw new InvalidOperationException("FAQ not found");

            faq.Question = dto.Question;
            faq.Answer = dto.Answer;
            faq.Category = dto.Category;

            await _faqRepository.UpdateAsync(faq);
            await _faqRepository.SaveChangesAsync();

            return MapToDto(faq);
        }

        public async Task DeleteFAQAsync(Guid id)
        {
            var faq = await _faqRepository.GetByIdAsync(id);
            if (faq == null)
                throw new InvalidOperationException("FAQ not found");

            await _faqRepository.DeleteAsync(faq);
            await _faqRepository.SaveChangesAsync();
        }

       
    }
}
