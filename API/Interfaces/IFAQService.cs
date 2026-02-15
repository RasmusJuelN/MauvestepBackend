using API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IFAQService
    {
        Task<FAQDto?> GetFAQByIdAsync(Guid id);
        Task<IEnumerable<FAQDto>> GetAllFAQsAsync();
        Task<FAQDto> CreateFAQAsync(CreateFAQDto dto);
        Task<FAQDto> UpdateFAQAsync(Guid id, UpdateFAQDto dto);
        Task DeleteFAQAsync(Guid id);
    }
}
