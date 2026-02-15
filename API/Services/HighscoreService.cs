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
    public class HighscoreService : IHighscoreService
    {
        private readonly IHighscoreRepository _highscoreRepository;
        private readonly IUserRepository _userRepository;

        public HighscoreService(IHighscoreRepository highscoreRepository, IUserRepository userRepository)
        {
            _highscoreRepository = highscoreRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<HighscoreDto>> GetAllHighscoresAsync()
        {
            var highscores = await _highscoreRepository.GetAllHighscoresAsync();
            return highscores.Select(MapToDto);
        }

        public async Task<HighscoreDto> CreateHighscoreAsync(CreateHighscoreDto dto, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var highscore = new Highscore
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BossName = dto.BossName,
                Score = dto.Score,
                CreatedAt = DateTime.UtcNow
            };

            await _highscoreRepository.AddAsync(highscore);
            await _highscoreRepository.SaveChangesAsync();

            highscore.User = user;
            return MapToDto(highscore);
        }

        private HighscoreDto MapToDto(Highscore highscore)
        {
            return new HighscoreDto
            {
                Id = highscore.Id,
                UserId = highscore.UserId,
                Username = highscore.User?.Username ?? "Unknown",
                BossName = highscore.BossName,
                Score = highscore.Score,
                CreatedAt = highscore.CreatedAt
            };
        }
    }
}
