using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPIBankSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StopPaymentRequestsController : ControllerBase
    {
        private readonly IStopPaymentRequestService _stopPaymentRequestService;

        public StopPaymentRequestsController(IStopPaymentRequestService stopPaymentRequestService)
        {
            _stopPaymentRequestService = stopPaymentRequestService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllStopPaymentRequests()
        {
            var requests = await _stopPaymentRequestService.GetAllStopPaymentRequestsAsync();

            return Ok(new CommonResponse<List<StopPaymentRequestResponseDto>>
            {
                status = "success",
                message = "All stop payment requests retrieved successfully.",
                data = requests
            });
        }

        // GET: api/StopPaymentRequests/user/5
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetStopPaymentRequestsByUserId(int userId)
        {
            var requests = await _stopPaymentRequestService.GetStopPaymentRequestsByUserIdAsync(userId);

            return Ok(new CommonResponse<List<StopPaymentRequestDto>>
            {
                status = "success",
                message = $"Stop payment requests for user {userId} retrieved successfully.",
                data = requests
            });
        }

        // GET: api/StopPaymentRequests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStopPaymentRequest(int id)
        {
            var request = await _stopPaymentRequestService.GetStopPaymentRequestAsync(id);

            if (request == null)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Stop payment request not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<StopPaymentRequestDto>
            {
                status = "success",
                message = "Stop payment request retrieved successfully.",
                data = request
            });
        }

        // POST: api/StopPaymentRequests
        [HttpPost]
        public async Task<IActionResult> CreateStopPaymentRequest(CreateStopPaymentRequestDto dto)
        {
            var request = await _stopPaymentRequestService.CreateStopPaymentRequestAsync(dto);

            if (request == null)
            {
                return BadRequest(new CommonResponse<string>
                {
                    status = "error",
                    message = "Invalid AccountId or UserId.",
                    data = null
                });
            }

            return Ok(request);
        }

        // PUT: api/StopPaymentRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStopPaymentRequest(int id, string status)
        {
            var success = await _stopPaymentRequestService.UpdateStopPaymentRequestStatusAsync(id, status);

            if (!success)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Stop payment request not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<string>
            {
                status = "success",
                message = "Stop payment request updated successfully.",
                data = null
            });
        }

        // DELETE: api/StopPaymentRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStopPaymentRequest(int id)
        {
            var success = await _stopPaymentRequestService.DeleteStopPaymentRequestAsync(id);

            if (!success)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Stop payment request not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<string>
            {
                status = "success",
                message = "Stop payment request deleted successfully.",
                data = null
            });
        }
    }
}
