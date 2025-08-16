using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;

namespace BPIBankSystem.API.Services
{
    public interface IAddressService
    {
        Task<List<AddressResponseDto>> GetAllAsync();
        Task<List<AddressDto>> GetAllByUserIdAsync(int userId);
        Task<AddressDto> GetCurrentAddressAsync(int userId);
        Task<bool> AddAddressAsync(AddressCreateDto dto);
        Task<bool> UpdateAddressAsync(int addressId, AddressUpdateDto dto);
        Task<bool> MarkAddressAsOldAsync(int addressId);
    }

}
