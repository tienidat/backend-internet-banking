using BPIBankSystem.API.DTOs.Requests;

namespace BPIBankSystem.API.Services
{
    public interface IAccountService
    {
        Task<List<AccountDto>> GetAllAsync();
        Task<List<AccountDto>> GetByUserIdAsync(int userId);
        Task<AccountDto?> GetByIdAsync(int id);
        Task<AccountDto> CreateAsync(AccountDto accountDto);
        Task<bool> UpdateAsync(int id, AccountDto accountDto);
        Task<bool> DeleteAsync(int id);
    }
}
