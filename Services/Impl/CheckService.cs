using AutoMapper;
using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using Microsoft.EntityFrameworkCore;

namespace BPIBankSystem.API.Services.Impl
{
    public class CheckService : ICheckService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public CheckService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CheckDto>> GetChecksByAccountIdAsync(int accountId)
        {
            return await _context.Checks
                .Where(c => c.AccountId == accountId)
                .OrderByDescending(c => c.IssueDate)
                .Select(c => new CheckDto
                {
                    Id = c.Id,
                    CheckNumber = c.CheckNumber,
                    AccountId = c.AccountId,
                    IssueDate = c.IssueDate,
                    Status = c.Status,
                    CancellationReason = c.CancellationReason
                })
                .ToListAsync();
        }

        public async Task<List<CheckResponseDto>> GetAllChecksAsync()
        {
            var checks = await _context.Checks
                .Include(c => c.Account) 
                .OrderByDescending(c => c.IssueDate)
                .ToListAsync();

            return _mapper.Map<List<CheckResponseDto>>(checks);
        }


        public async Task<CheckDto> GetCheckAsync(int id)
        {
            var check = await _context.Checks
                .FirstOrDefaultAsync(c => c.Id == id);
            if (check == null)
                return null;

            return new CheckDto
            {
                Id = check.Id,
                CheckNumber = check.CheckNumber,
                AccountId = check.AccountId,

                IssueDate = check.IssueDate,
                Status = check.Status,
                CancellationReason = check.CancellationReason
            };
        }

        public async Task<CheckDto> CreateCheckAsync(CreateCheckDto dto)
        {
            if (!await _context.Accounts.AnyAsync(a => a.Id == dto.AccountId))
            {
                return null;
            }

            var check = new Checks
            {
                CheckNumber = dto.CheckNumber,
                AccountId = dto.AccountId,

                CancellationReason = dto.CancellationReason
            };

            _context.Checks.Add(check);
            await _context.SaveChangesAsync();

            return new CheckDto
            {
                Id = check.Id,
                CheckNumber = check.CheckNumber,
                AccountId = check.AccountId,
                IssueDate = check.IssueDate,
                Status = check.Status,
                CancellationReason = check.CancellationReason
            };
        }

        public async Task<bool> UpdateCheckAsync(int id, UpdateCheckDto dto)
        {
            var check = await _context.Checks.FindAsync(id);
            if (check == null)
                return false;

            check.Status = string.IsNullOrEmpty(dto.Status) ? check.Status : dto.Status;
            check.CancellationReason = string.IsNullOrEmpty(dto.CancellationReason) ? check.CancellationReason : dto.CancellationReason;

            _context.Entry(check).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCheckAsync(int id)
        {
            var check = await _context.Checks.FindAsync(id);
            if (check == null)
                return false;

            _context.Checks.Remove(check);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
