using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;

namespace BPIBankSystem.API.Services
{
    public interface IAddressChangeRequestService
    {
        Task<List<AddressChangeRequestResponseDto>> GetAllAsync();
        Task<List<AddressChangeRequestDto>> GetRequestsByUserIdAsync(int userId);
        Task<AddressChangeRequestDto> GetAddressChangeRequestAsync(int id);
        Task<AddressChangeRequestDto> CreateAddressChangeRequestAsync(CreateAddressChangeRequestDto dto);
        Task<bool> UpdateAddressChangeRequestStatusAsync(int id, string status);
        Task<bool> DeleteAddressChangeRequestAsync(int id);
    }
}
