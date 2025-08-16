using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPIBankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransfersController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        /// <summary>
        /// Bước 1: Gửi OTP nếu đúng mật khẩu giao dịch
        /// </summary>
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtpForTransfer([FromBody] OtpTransferRequestDto request)
        {
            if (string.IsNullOrEmpty(request.FromAccountNumber) || string.IsNullOrEmpty(request.TransactionPassword))
            {
                return BadRequest(new CommonResponse<string>("error", "FromAccountNumber and TransactionPassword are required"));
            }

            var response = await _transferService.SendOtpForTransferAsync(request.FromAccountNumber, request.TransactionPassword);
            return Ok(response);
        }

        /// <summary>
        /// Bước 2: Xác nhận OTP và thực hiện chuyển khoản
        /// </summary>
        [HttpPost("confirm-transfer")]
        public async Task<IActionResult> ConfirmTransfer([FromBody] ConfirmTransferDto dto)
        {
            if (dto.TransferRequest == null || string.IsNullOrEmpty(dto.Otp))
            {
                return BadRequest(new CommonResponse<string>("error", "OTP and transfer data are required"));
            }

            var result = await _transferService.ConfirmTransferWithOtpAsync(dto.TransferRequest, dto.Otp);
            return Ok(result);
        }

        [HttpGet("transactions-admin")]
        public async Task<IActionResult> GetAllTransactionsAdmin()
        {
            try
            {
                var transferRequests = await _transferService.GetAllTransactionsAdminAsync();
                return Ok(new CommonResponse<List<TransactionResponseDto>>("success", "Transfer requests retrieved successfully", transferRequests));
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonResponse<string>("error", ex.Message));
            }
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = await _transferService.GetAllTransactionsAsync();
                return Ok(new CommonResponse<List<TransactionDto>>("success", "Transactions retrieved successfully", transactions));
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonResponse<string>("error", ex.Message));
            }
        }

        [HttpGet("validate-account/{toAccountNumber}")]
        public async Task<IActionResult> ValidateAccountNumber(
                                        string toAccountNumber,
                                        [FromQuery] string fromAccountNumber = null,
                                        [FromQuery] decimal? amount = null)
        {
            try
            {
                var result = await _transferService.ValidateAccountNumberAsync(toAccountNumber, fromAccountNumber, amount);
                return Ok(new CommonResponse<AccountValidationResponseDto>("success", result.Message, result));
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonResponse<string>("error", ex.Message));
            }
        }
    }
}
