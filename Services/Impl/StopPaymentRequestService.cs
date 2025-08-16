using AutoMapper;
using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPIBankSystem.API.Services.Impl
{
    public class StopPaymentRequestService : IStopPaymentRequestService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StopPaymentRequestService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<StopPaymentRequestResponseDto>> GetAllStopPaymentRequestsAsync()
        {
            var requests = await _context.StopPaymentRequests
                .Include(r => r.User)
                .Include(r => r.Account) 
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            return _mapper.Map<List<StopPaymentRequestResponseDto>>(requests);
        }

        public async Task<List<StopPaymentRequestDto>> GetStopPaymentRequestsByUserIdAsync(int userId)
        {
            return await _context.StopPaymentRequests
                .Where(r => r.UserId == userId)
                .Select(r => new StopPaymentRequestDto
                {
                    Id = r.Id,
                    AccountId = r.AccountId,
                    UserId = r.UserId,
                    CheckNumber = r.CheckNumber,
                    ChequeDate=r.ChequeDate,
                    RequestDate = r.RequestDate,
                    Status = r.Status,
                    Reason = r.Reason,
                    Notes = r.Notes,
                    PayeeName = r.PayeeName
                }).ToListAsync();
        }

        public async Task<StopPaymentRequestDto> GetStopPaymentRequestAsync(int id)
        {
            var request = await _context.StopPaymentRequests
                .FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
                return null;

            return new StopPaymentRequestDto
            {
                Id = request.Id,
                AccountId = request.AccountId,
                UserId = request.UserId,
                CheckNumber = request.CheckNumber,
                ChequeDate = request.ChequeDate,
                RequestDate = request.RequestDate,
                Status = request.Status,
                Reason = request.Reason,
                Notes = request.Notes,
                PayeeName = request.PayeeName
            };
        }

        public async Task<CommonResponse<object>> CreateStopPaymentRequestAsync(CreateStopPaymentRequestDto dto)
        {
            var accountExists = await _context.Accounts.AnyAsync(a => a.Id == dto.AccountId);
            if (!accountExists)
                return new CommonResponse<object>("Failed", "Tài khoản không tồn tại.", null);

            var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
            if (!userExists)
                return new CommonResponse<object>("Failed", "Người dùng không tồn tại.", null);

            var checkExists = await _context.Checks.AnyAsync(c =>
                c.CheckNumber == dto.CheckNumber &&
                c.AccountId == dto.AccountId);
            if (!checkExists)
                return new CommonResponse<object>("Failed", "Số séc không hợp lệ hoặc khôngad thuộc về tài khoản.", null);

            var isDuplicateRequest = await _context.StopPaymentRequests.AnyAsync(r =>
                r.UserId == dto.UserId &&
                r.AccountId == dto.AccountId &&
                r.CheckNumber == dto.CheckNumber);
            if (isDuplicateRequest)
                return new CommonResponse<object>("Failed", "Yêu cầu ngừng thanh toán cho số séc này đã tồn tại.", null);

            var request = new StopPaymentRequests
            {
                AccountId = dto.AccountId,
                UserId = dto.UserId,
                CheckNumber = dto.CheckNumber,
                ChequeDate = dto.ChequeDate,
                Reason = dto.Reason,
                Notes = dto.Notes,
                PayeeName = dto.PayeeName
            };

            _context.StopPaymentRequests.Add(request);
            await _context.SaveChangesAsync();

            var response= new StopPaymentRequestDto
            {
                Id = request.Id,
                AccountId = request.AccountId,
                UserId = request.UserId,
                CheckNumber = request.CheckNumber,
                ChequeDate = request.ChequeDate,
                RequestDate = request.RequestDate,
                Status = request.Status,
                Reason = request.Reason,
                Notes = request.Notes,
                PayeeName = request.PayeeName
            };
            return new CommonResponse<object>("success", "Stop Payment successfully", response);    
        }


        public async Task<bool> UpdateStopPaymentRequestStatusAsync(int id, string newStatus)
        {
            var request = await _context.StopPaymentRequests.FindAsync(id);
            if (request == null)
                return false;

            request.Status = newStatus;

            if (newStatus.Equals("approved", StringComparison.OrdinalIgnoreCase))
            {
                var check = await _context.Checks
                    .FirstOrDefaultAsync(c => c.CheckNumber == request.CheckNumber && c.AccountId == request.AccountId);
                if (check == null)
                    return false;

                check.Status = "stopped";
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteStopPaymentRequestAsync(int id)
        {
            var request = await _context.StopPaymentRequests.FindAsync(id);
            if (request == null)
                return false;

            _context.StopPaymentRequests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
