using AutoMapper;
using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPIBankSystem.API.Services.Impl
{
    public class SupportService : ISupportService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public SupportService(AppDbContext context, IEmailService emailService, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<List<SupportRequestResponse>> GetAllSupportRequestsAsync()
        {
            var supportRequests = await _context.SupportRequests
                .Include(sr => sr.User)
                .ToListAsync();
            return _mapper.Map<List<SupportRequestResponse>>(supportRequests);
        }

        public async Task<SupportRequest> CreateSupportRequestAsync(SupportRequestDto request)
        {
            var supportRequest = new SupportRequest
            {
                UserId = request.UserId,
                Email = request.Email,
                Question = request.Question,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.SupportRequests.Add(supportRequest);
            await _context.SaveChangesAsync();

            try
            {
                await _emailService.SendAsync(
                    request.Email,
                    "Yêu cầu hỗ trợ đã được nhận",
                    $"Cảm ơn bạn đã liên hệ! Câu hỏi của bạn:\n\n{request.Question}\n\nChúng tôi sẽ xử lý sớm nhất có thể."
                );

                await _emailService.SendAsync(
                    _configuration["EmailSettings:AdminEmail"] ?? "binhricardo@gmail.com",
                    "Yêu cầu hỗ trợ mới",
                    $"Yêu cầu mới từ {request.Email}:\n\n{request.Question}"
                );
            }
            catch (Exception)
            {
                Console.WriteLine("Lỗi gửi email, nhưng yêu cầu đã được lưu vào database.");
            }

            return supportRequest;
        }


        public async Task<bool> UpdateAnswerAsync(int id, string answer)
        {
            var request = await _context.SupportRequests.FindAsync(id);
            if (request == null) return false;

            request.Answer = answer;
            request.Status = "Resolved";
            request.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            try
            {
                await _emailService.SendAsync(
                    request.Email,
                    "Phản hồi yêu cầu hỗ trợ",
                    $"Bạn đã hỏi: {request.Question}\n\nCâu trả lời từ chúng tôi:\n{answer}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi gửi email phản hồi: " + ex.Message);

            }

            return true;
        }

    }

}
