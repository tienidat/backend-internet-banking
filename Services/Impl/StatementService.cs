using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs;
using BPIBankSystem.API.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace BPIBankSystem.API.Services.Impl
{
    public class StatementService : IStatementService
    {
        private readonly AppDbContext _context;

        public StatementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StatementResultDto> GetStatementAsync(int accountId, string type, int? month, int year)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                return null;


            var allTransactions = _context.Transactions
                .Where(t =>
                    (t.FromAccountId == accountId || t.ToAccountId == accountId) &&
                    t.CreatedAt.Year == year);

            // Nếu chọn dạng monthly thì lọc thêm theo tháng
            if (type == "monthly" && month.HasValue)
            {
                allTransactions = allTransactions.Where(t => t.CreatedAt.Month == month.Value);
            }

            var transactionList = await allTransactions.OrderBy(t => t.CreatedAt).ToListAsync();

            decimal totalCredits = transactionList
                .Where(t => t.ToAccountId == accountId)
                .Sum(t => t.Amount);

            decimal totalDebits = transactionList
                .Where(t => t.FromAccountId == accountId)
                .Sum(t => t.Amount);

            decimal closingBalance = account.Balance;
            decimal openingBalance = closingBalance + totalDebits - totalCredits;

            return new StatementResultDto
            {
                AccountNumber = account.AccountNumber,
                AccountName = account.AccountName,
                OpeningBalance = openingBalance,
                ClosingBalance = closingBalance,
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                Transactions = transactionList.Select(t => new TransactionDto
                {
                    Id = t.Id,
                    TransferRequestId = t.TransferRequestId,
                    FromAccountId = t.FromAccountId,
                    ToAccountId = t.ToAccountId,
                    Amount = t.Amount,
                    Description = t.Description,
                    CreatedAt = t.CreatedAt,
                    Reference = t.Reference
                }).ToList()
            };
        }
    }
}
