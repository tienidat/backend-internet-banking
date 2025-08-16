using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;

namespace BPIBankSystem.API.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<(UserResponseDto user, string token, string message)> LoginAsync(LoginDto dto);
        Task<List<UserResponseDto>> GetAllAsync();
        Task<string> UpdateAsync(string userId, UpdateUserDto dto);
        Task<string> DeleteAsync(string userId);
    }
}