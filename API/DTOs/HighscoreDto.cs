using System;

namespace API.DTOs
{
    public class HighscoreDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string BossName { get; set; } = null!;
        public int Score { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateHighscoreDto
    {
        public string BossName { get; set; } = null!;
        public int Score { get; set; }
    }
}
