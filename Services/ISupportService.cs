using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Entities;

namespace BPIBankSystem.API.Services
{
    public interface ISupportService
    {
        Task<List<SupportRequestResponse>> GetAllSupportRequestsAsync();
        Task<SupportRequest> CreateSupportRequestAsync(SupportRequestDto request);
        Task<bool> UpdateAnswerAsync(int id, string answer);

    }
}
