using API.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto);
        Task DeleteUserAsync(Guid id);
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<RefreshTokenResponseDto> RefreshTokenAsync(string refreshToken);
        Task<UserDto> GetCurrentUserAsync();
    }
}
