using BPIBankSystem.API.Entities;

namespace BPIBankSystem.API.Services
{
    public interface IHelpService
    {
        Task<List<Help>> GetAllHelpsAsync();
        Task<List<CategoryHelp>> GetAllCategoriesAsync();
    }
}
