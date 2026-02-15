using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HighscoresController : ControllerBase
    {
        private readonly IHighscoreService _highscoreService;

        public HighscoresController(IHighscoreService highscoreService)
        {
            _highscoreService = highscoreService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HighscoreDto>>> GetAllHighscores()
        {
            var scores = await _highscoreService.GetAllHighscoresAsync();
            return Ok(scores);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<HighscoreDto>> CreateHighscore(CreateHighscoreDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var score = await _highscoreService.CreateHighscoreAsync(dto, userId);
            return CreatedAtAction(nameof(GetAllHighscores), new { }, score);
        }
    }
}
