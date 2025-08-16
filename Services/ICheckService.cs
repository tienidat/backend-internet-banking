using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;

namespace BPIBankSystem.API.Services
{
    public interface ICheckService
    {
        Task<List<CheckDto>> GetChecksByAccountIdAsync(int accountId);
        Task<List<CheckResponseDto>> GetAllChecksAsync();
        Task<CheckDto> GetCheckAsync(int id);
        Task<CheckDto> CreateCheckAsync(CreateCheckDto dto);
        Task<bool> UpdateCheckAsync(int id, UpdateCheckDto dto);
        Task<bool> DeleteCheckAsync(int id);
    }
}
