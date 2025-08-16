using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;

namespace BPIBankSystem.API.Services
{
    public interface ICheckbookRequestService
    {
        Task<List<CheckbookRequestResponseDto>> GetAllRequestsAsync();
        Task<List<CheckbookRequestDto>> GetRequestsByUserAsync(int userId);
        Task<CheckbookRequestDto> GetCheckbookRequestAsync(int id);
        Task<CheckbookRequestDto> CreateCheckbookRequestAsync(CreateCheckbookRequestDto dto);
        Task<bool> UpdateCheckbookRequestStatusAsync(int id, string status);
        Task<bool> DeleteCheckbookRequestAsync(int id);
    }
}
