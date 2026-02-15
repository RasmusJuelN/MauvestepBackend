using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(CreateUserDto dto)
        {
            try
            {
                // Validate input is not null
                if (string.IsNullOrWhiteSpace(dto.Username) || 
                    string.IsNullOrWhiteSpace(dto.Email) || 
                    string.IsNullOrWhiteSpace(dto.Password))
                {
                    return BadRequest(new { message = "Username, email, and password are required" });
                }

                // Service handles validation and password hashing
                var user = await _userService.CreateUserAsync(dto);

                return Ok(new 
                { 
                    success = true,
                    message = "Registration successful",
                    userId = user.Id 
                });
            }
            catch (InvalidOperationException ex)
            {
                // Catch duplicate username/email errors from service
                return BadRequest(new 
                { 
                    success = false,
                    message = ex.Message 
                });
            }
           
        }

        // handles login and token generation
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                {
                    return BadRequest(new { message = "Username and password are required" });
                }

                var result = await _userService.LoginAsync(dto);

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                // Invalid credentials
                return Unauthorized(new
                {
                    success = false,
                    message = ex.Message
                });
            }
           
        }

// token refresh
        [HttpPost("refreshtoken")]
        public async Task<ActionResult<RefreshTokenResponseDto>> RefreshToken(RefreshTokenDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RefreshToken))
                return BadRequest(new { message = "Refresh token is required" });

            try
            {
                var result = await _userService.RefreshTokenAsync(dto.RefreshToken);
                return Ok(new { success = true, data = result });
            }
            catch (InvalidOperationException)
            {
                return Unauthorized(new { message = "Invalid or expired token" });
            }
        }
    }
}
