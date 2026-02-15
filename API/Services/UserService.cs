using API.DTOs;
using API.Interfaces;
using Database.Interfaces;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        private UserDto MapUserToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                LikesCount = user.LikesCount,
                PostCount = user.PostCount,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                CreatedAt = user.CreatedAt,
            };
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            // Extract user ID from JWT claims in the current request
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            // Console.WriteLine($"userIdClaim from JWT: {userIdClaim}");
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                throw new InvalidOperationException("User not authenticated or missing user ID claim");

            var userId = userIdClaim.Value;
            // Console.WriteLine($"Extracted userId from JWT: {userId}");

            // Parse the user ID and fetch from database
            if (!Guid.TryParse(userId, out var userGuid))
                throw new InvalidOperationException($"Invalid user ID format in token: {userId}");

            // Console.WriteLine($"Parsed GUID: {userGuid}");
            var user = await _userRepository.GetByIdAsync(userGuid);
            // Console.WriteLine($"User from DB: {(user == null ? "NULL" : user.Username)}");
            
            if (user == null)
                throw new InvalidOperationException($"User not found for ID: {userGuid}");

            return MapUserToDto(user);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapUserToDto(user);
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user == null ? null : MapUserToDto(user);
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return user == null ? null : MapUserToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapUserToDto).ToList();
        }

        
        // Registers a new user
        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            // Validate username doesn't already exist
            if (await _userRepository.UsernameExistsAsync(dto.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            // validates email
            if (await _userRepository.EmailExistsAsync(dto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            // Create new user with the password hashed
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password), // Hash password before storing
                CreatedAt = DateTime.UtcNow,
                Role = "User", // Default role for new users, I can set this to "Admin" if I want to create an admin user
                LikesCount = 0,
                PostCount = 0,
            };

            // Save user to database
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapUserToDto(user);
        }

        public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            // Check if the new username is unique 
            if (!string.IsNullOrEmpty(dto.Username) && user.Username != dto.Username && await _userRepository.UsernameExistsAsync(dto.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Check the email
            if (!string.IsNullOrEmpty(dto.Email) && user.Email != dto.Email && await _userRepository.EmailExistsAsync(dto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            // Update only provided fields from the request body, allowing to nly update some fields instead of all
            if (!string.IsNullOrEmpty(dto.Username))
                user.Username = dto.Username;
            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;
            if (dto.Bio != null)
                user.Bio = dto.Bio;
            if (dto.ProfilePictureUrl != null)
                user.ProfilePictureUrl = dto.ProfilePictureUrl;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapUserToDto(user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
            await _userRepository.SaveChangesAsync();
        }

        
        // Authenticates a user with username and password
        // Returns access token, refresh token, and user data if successful
      
        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            // Find user by username
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid username or password");
            }

            // Verify password
            if (!VerifyPassword(dto.Password, user.PasswordHash))
            {
                throw new InvalidOperationException("Invalid username or password");
            }

            // Generate tokens
            var accessToken = _jwtService.GenerateAccessToken(user.Id, user.Username, user.Role);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Store refresh token in database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return new LoginResponseDto
            {
                User = MapUserToDto(user),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = _jwtService.GetAccessTokenExpirationSeconds()
            };
        }

      
        // Refreshes an access token using a valid refresh token
        //Validates refresh token and generates new access token
       
        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(string refreshToken)
        {
            // Find user with this refresh token
            var user = await _userRepository.GetAllAsync();
            var userWithToken = user.FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (userWithToken == null)
            {
                throw new InvalidOperationException("Invalid refresh token");
            }

            // Check if refresh token is expired
            if (userWithToken.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Refresh token has expired");
            }

            // Generate new tokens
            var newAccessToken = _jwtService.GenerateAccessToken(userWithToken.Id, userWithToken.Username, userWithToken.Role);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // Update refresh token in database
            userWithToken.RefreshToken = newRefreshToken;
            userWithToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(userWithToken);
            await _userRepository.SaveChangesAsync();

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = _jwtService.GetAccessTokenExpirationSeconds()
            };
        }

       
    }
}
