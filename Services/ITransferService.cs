using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;

namespace BPIBankSystem.API.Services
{
    public interface ITransferService
    {
        Task<CommonResponse<object>> SendOtpForTransferAsync(string fromAccountNumber, string transactionPassword);
        Task<CommonResponse<object>> ConfirmTransferWithOtpAsync(TransferRequestDto transferRequestDto, string inputOtp);
        Task<List<TransactionResponseDto>> GetAllTransactionsAdminAsync();
        Task<List<TransactionDto>> GetAllTransactionsAsync();
        Task<AccountValidationResponseDto> ValidateAccountNumberAsync(string toAccountNumber, string fromAccountNumber = null, decimal? amount = null);
    }
}

