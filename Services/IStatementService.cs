using BPIBankSystem.API.DTOs.Requests;

namespace BPIBankSystem.API.Services
{
    public interface IStatementService
    {
        Task<StatementResultDto> GetStatementAsync(int accountId, string type, int? month, int year);
    }
}

