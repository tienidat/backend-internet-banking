using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Entities;
using BPIBankSystem.API.Services;
using BPIBankSystem.API.Services.Impl;
using Microsoft.AspNetCore.Mvc;

namespace BPIBankSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly ISupportService _supportService;

        public SupportController(ISupportService supportService)
        {
            _supportService = supportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSupportRequestsAsync()
        {
            var requests = await _supportService.GetAllSupportRequestsAsync();

            return Ok(new CommonResponse<List<SupportRequestResponse>>
            {
                status = "success",
                message = "All stop payment requests retrieved successfully.",
                data = requests
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupportRequest([FromBody] SupportRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Question))
            {
                return BadRequest(new CommonResponse<string>
                {
                    status = "error",
                    message = "Email và câu hỏi là bắt buộc",
                    data = null
                });
            }

            var supportRequest = await _supportService.CreateSupportRequestAsync(request);

            return Ok(new CommonResponse<SupportRequest>
            {
                status = "success",
                message = "Yêu cầu hỗ trợ đã được gửi",
                data = supportRequest
            });
        }

        [HttpPut("answer/{id}")]
        public async Task<IActionResult> AnswerSupportRequest(int id, [FromBody] string answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                return BadRequest(new CommonResponse<string>
                {
                    status = "error",
                    message = "Câu trả lời không được để trống",
                    data = null
                });
            }

            var success = await _supportService.UpdateAnswerAsync(id, answer);
            if (!success)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Không tìm thấy yêu cầu hỗ trợ",
                    data = null
                });
            }

            return Ok(new CommonResponse<string>
            {
                status = "success",
                message = "Đã cập nhật câu trả lời và gửi email thành công",
                data = answer
            });
        }

    }
}
