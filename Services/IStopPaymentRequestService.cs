using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;

namespace BPIBankSystem.API.Services
{
    public interface IStopPaymentRequestService
    {
        Task<StopPaymentRequestDto> GetStopPaymentRequestAsync(int id);
        Task<List<StopPaymentRequestResponseDto>> GetAllStopPaymentRequestsAsync();
        Task<List<StopPaymentRequestDto>> GetStopPaymentRequestsByUserIdAsync(int userId);
        Task<CommonResponse<object>> CreateStopPaymentRequestAsync(CreateStopPaymentRequestDto dto);
        Task<bool> UpdateStopPaymentRequestStatusAsync(int id, string newStatus);
        Task<bool> DeleteStopPaymentRequestAsync(int id);
    }
}
