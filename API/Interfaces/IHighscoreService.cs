using API.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IHighscoreService
    {
        Task<IEnumerable<HighscoreDto>> GetAllHighscoresAsync();
        Task<HighscoreDto> CreateHighscoreAsync(CreateHighscoreDto dto, Guid userId);
    }
}
