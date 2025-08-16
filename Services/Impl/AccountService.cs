using BPIBankSystem.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;

namespace BPIBankSystem.API.Services.Impl
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountDto>> GetAllAsync()
        {
            var accounts = await _context.Accounts.ToListAsync();

            return accounts.Select(a => new AccountDto
            {
                AccountId = a.Id,
                AccountNumber = a.AccountNumber,
                AccountName = a.AccountName,
                Balance = a.Balance,
                UserId = a.UserId
            }).ToList();
        }

        public async Task<List<AccountDto>> GetByUserIdAsync(int userId)
        {
            var accounts = await _context.Accounts
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return accounts.Select(a => new AccountDto
            {
                AccountId = a.Id,
                AccountNumber = a.AccountNumber,
                AccountName = a.AccountName,
                Balance = a.Balance,
                UserId = a.UserId
            }).ToList();
        }

        public async Task<AccountDto?> GetByIdAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return null;

            return new AccountDto
            {
                AccountId = account.Id,
                AccountNumber = account.AccountNumber,
                AccountName = account.AccountName,
                Balance = account.Balance,
                UserId = account.UserId
            };
        }

        public async Task<AccountDto> CreateAsync(AccountDto accountDto)
        {
            // Kiểm tra số tài khoản đã tồn tại
            var exists = await _context.Accounts
                .AnyAsync(a => a.AccountNumber == accountDto.AccountNumber);

            if (exists)
                throw new ArgumentException("Account number already exists.");

            // Kiểm tra password chỉ chứa số
            if (!Regex.IsMatch(accountDto.TransactionPassword, @"^\d+$"))
                throw new ArgumentException("Transaction password must contain only digits.");

            var passwordHasher = new PasswordHasher<Account>();

            var account = new Account
            {
                AccountNumber = accountDto.AccountNumber,
                AccountName = accountDto.AccountName,
                Balance = accountDto.Balance,
                UserId = accountDto.UserId
            };

            account.TransactionPassword = passwordHasher.HashPassword(account, accountDto.TransactionPassword);

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            accountDto.AccountId = account.Id;
            return accountDto;
        }

        public async Task<bool> UpdateAsync(int id, AccountDto accountDto)
        {
            if (!Regex.IsMatch(accountDto.TransactionPassword, @"^\d+$"))
                throw new ArgumentException("Transaction password must contain only digits.");

            var existing = await _context.Accounts.FindAsync(id);
            if (existing == null) return false;

            var passwordHasher = new PasswordHasher<Account>();

            existing.AccountNumber = accountDto.AccountNumber;
            existing.AccountName = accountDto.AccountName;
            existing.Balance = accountDto.Balance;
            existing.UserId = accountDto.UserId;
            existing.TransactionPassword = passwordHasher.HashPassword(existing, accountDto.TransactionPassword);

            _context.Accounts.Update(existing);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return false;

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}