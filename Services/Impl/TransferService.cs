using AutoMapper;
using System.Data;
using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BPIBankSystem.API.Services.Impl
{
    public class TransferService : ITransferService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<Account> _passwordHasher;
        private readonly ILogger<TransferService> _logger;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _memoryCache;

        public TransferService(AppDbContext dbContext, IMapper mapper, ILogger<TransferService> logger,
            IEmailService emailService, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<Account>();
            _logger = logger;
            _emailService = emailService;
            _memoryCache = memoryCache;
        }

        public async Task<CommonResponse<object>> SendOtpForTransferAsync(string fromAccountNumber, string transactionPassword)
        {
            var account = await _dbContext.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.AccountNumber == fromAccountNumber);
            if (account == null || account.User == null)
                return new CommonResponse<object>("Failed", "Account or associated user not found", null);

            var verificationResult = _passwordHasher.VerifyHashedPassword(account, account.TransactionPassword, transactionPassword);
            if (verificationResult != PasswordVerificationResult.Success)
                return new CommonResponse<object>("Rejected", "Incorrect transaction password", null);

            string otp = new Random().Next(100000, 999999).ToString();
            _memoryCache.Set($"otp:{account.Id}", otp, TimeSpan.FromMinutes(5));

            await _emailService.SendAsync(account.User.Email, "Your OTP for Transfer", $"Your OTP is: {otp}");

            return new CommonResponse<object>("Pending", "OTP sent to email", null);
        }

        public async Task<CommonResponse<object>> ConfirmTransferWithOtpAsync(TransferRequestDto transferRequestDto, string inputOtp)
        {
            var fromAccount = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == transferRequestDto.FromAccountNumber);
            if (fromAccount == null)
                return new CommonResponse<object>("Rejected", "From account not found", null);

            if (!_memoryCache.TryGetValue($"otp:{fromAccount.Id}", out string cachedOtp) || cachedOtp != inputOtp)
            {
                return new CommonResponse<object>("Rejected", "Invalid or expired OTP", null);
            }

            _memoryCache.Remove($"otp:{fromAccount.Id}");
            return await TransferCoreAsync(transferRequestDto);
        }

        private async Task<CommonResponse<object>> TransferCoreAsync(TransferRequestDto transferRequestDto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var fromAccount = await _dbContext.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == transferRequestDto.FromAccountNumber);
                var toAccount = await _dbContext.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == transferRequestDto.ToAccountNumber);

                if (fromAccount == null || toAccount == null)
                    return new CommonResponse<object>("Rejected", "Invalid account(s)", null);

                if (fromAccount.Balance < transferRequestDto.Amount)
                    return new CommonResponse<object>("Rejected", "Insufficient balance", null);

                var newTransferRequest = new TransferRequest
                {
                    FromAccountId = fromAccount.Id,
                    ToAccountId = toAccount.Id,
                    Amount = transferRequestDto.Amount,
                    Description = transferRequestDto.Description,
                    Status = "Completed",
                    CreatedAt = DateTime.UtcNow,
                    ErrorMessage = ""
                };
                _dbContext.TransferRequests.Add(newTransferRequest);
                await _dbContext.SaveChangesAsync();

                fromAccount.Balance -= transferRequestDto.Amount;
                toAccount.Balance += transferRequestDto.Amount;

                var reference = $"TRF-{DateTime.UtcNow.Ticks}";
                var newTransaction = new Transaction
                {
                    TransferRequestId = newTransferRequest.Id,
                    FromAccountId = fromAccount.Id,
                    ToAccountId = toAccount.Id,
                    Amount = transferRequestDto.Amount,
                    Description = transferRequestDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    Reference = reference
                };
                _dbContext.Transactions.Add(newTransaction);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new CommonResponse<object>("Completed", "Transfer completed successfully", new { reference });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var error = ex.InnerException?.Message ?? ex.Message;

                _logger.LogError(ex, "Transfer failed: {Message}", error);

                return new CommonResponse<object>("Rejected", error, null);
            }
        }

        public async Task<List<TransactionResponseDto>> GetAllTransactionsAdminAsync()
        {
            var transactions = await _dbContext.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .ToListAsync();

            return _mapper.Map<List<TransactionResponseDto>>(transactions);
        }

        public async Task<List<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _dbContext.Transactions.ToListAsync();
            return _mapper.Map<List<TransactionDto>>(transactions);
        }


        public async Task<AccountValidationResponseDto> ValidateAccountNumberAsync(string toAccountNumber, string fromAccountNumber = null, decimal? amount = null)
        {
            var toAccount = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == toAccountNumber);
            var response = new AccountValidationResponseDto
            {
                IsValid = toAccount != null,
                AccountName = toAccount?.AccountName,
                Message = toAccount == null ? "Recipient account not found" : "Recipient account found",
                IsBalanceSufficient = true
            };

            if (!string.IsNullOrEmpty(fromAccountNumber) && amount.HasValue)
            {
                var fromAccount = await _dbContext.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == fromAccountNumber);
                if (fromAccount == null || fromAccount.Balance < amount)
                {
                    response.IsValid = false;
                    response.IsBalanceSufficient = false;
                    response.Message = "Insufficient balance";
                }
            }

            return response;
        }
    }
}