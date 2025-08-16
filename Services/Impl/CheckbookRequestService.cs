using AutoMapper;
using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPIBankSystem.API.Services.Impl
{
    public class CheckbookRequestService : ICheckbookRequestService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public CheckbookRequestService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<CheckbookRequestResponseDto>> GetAllRequestsAsync()
        {
            var requests = await _context.CheckbookRequests
                .Include(r => r.User) 
                .Include(r => r.Account) 
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            return _mapper.Map<List<CheckbookRequestResponseDto>>(requests);
        }

        public async Task<List<CheckbookRequestDto>> GetRequestsByUserAsync(int userId)
        {
            return await _context.CheckbookRequests
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RequestDate)
                .Select(r => new CheckbookRequestDto
                {
                    Id = r.Id,
                    AccountId = r.AccountId,
                    UserId = r.UserId,
                    RequestDate = r.RequestDate,
                    Status = r.Status,
                    CheckbookNumber = r.CheckbookNumber,
                    Notes = r.Notes
                }).ToListAsync();
        }


        public async Task<CheckbookRequestDto> GetCheckbookRequestAsync(int id)
        {
            var request = await _context.CheckbookRequests
                .FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
                return null;

            return new CheckbookRequestDto
            {
                Id = request.Id,
                AccountId = request.AccountId,
                UserId = request.UserId,
                RequestDate = request.RequestDate,
                Status = request.Status,
                CheckbookNumber = request.CheckbookNumber,
                Notes = request.Notes
            };
        }

        public async Task<CheckbookRequestDto> CreateCheckbookRequestAsync(CreateCheckbookRequestDto dto)
        {
            if (!await _context.Accounts.AnyAsync(a => a.Id == dto.AccountId) ||
                !await _context.Users.AnyAsync(u => u.Id == dto.UserId))
            {
                return null;
            }

            var request = new CheckbookRequests
            {
                AccountId = dto.AccountId,
                UserId = dto.UserId,
                CheckbookNumber = dto.CheckbookNumber,
                Notes = dto.Notes
            };

            _context.CheckbookRequests.Add(request);
            await _context.SaveChangesAsync();

            return new CheckbookRequestDto
            {
                Id = request.Id,
                AccountId = request.AccountId,
                UserId = request.UserId,
                RequestDate = request.RequestDate,
                Status = request.Status,
                CheckbookNumber = request.CheckbookNumber,
                Notes = request.Notes
            };
        }

        public async Task<bool> UpdateCheckbookRequestStatusAsync(int id, string status)
        {
            var request = await _context.CheckbookRequests.FindAsync(id);
            if (request == null)
                return false;

            if (!string.IsNullOrEmpty(status))
                request.Status = status;


            if (status == "approved")
            {
                if (!int.TryParse(request.CheckbookNumber, out int numberOfChecks))
                    numberOfChecks = 20;

                var lastCheck = await _context.Checks
                    .Where(c => c.AccountId == request.AccountId)
                    .OrderByDescending(c => c.CheckNumber)
                    .FirstOrDefaultAsync();

                int startNumber = lastCheck != null && int.TryParse(lastCheck.CheckNumber, out int lastNum)
                    ? lastNum + 1
                    : 100001;

                for (int i = 0; i < numberOfChecks; i++)
                {
                    var check = new Checks
                    {
                        CheckNumber = (startNumber + i).ToString(),
                        AccountId = request.AccountId,
                        IssueDate = DateTime.UtcNow,
                        Status = "active"
                    };
                    _context.Checks.Add(check);
                }
            }

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteCheckbookRequestAsync(int id)
        {
            var request = await _context.CheckbookRequests.FindAsync(id);
            if (request == null)
                return false;

            _context.CheckbookRequests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
