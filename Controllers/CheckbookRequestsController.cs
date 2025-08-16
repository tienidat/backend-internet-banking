using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPIBankSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckbookRequestsController : ControllerBase
    {
        private readonly ICheckbookRequestService _checkbookRequestService;

        public CheckbookRequestsController(ICheckbookRequestService checkbookRequestService)
        {
            _checkbookRequestService = checkbookRequestService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCheckbookRequests()
        {
            var requests = await _checkbookRequestService.GetAllRequestsAsync();

            return Ok(new CommonResponse<List<CheckbookRequestResponseDto>>
            {
                status = "success",
                message = "All checkbook requests retrieved successfully.",
                data = requests
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRequestsByUser(int userId)
        {
            var requests = await _checkbookRequestService.GetRequestsByUserAsync(userId);

            return Ok(new CommonResponse<List<CheckbookRequestDto>>
            {
                status = "success",
                message = "Checkbook requests for user retrieved successfully.",
                data = requests
            });
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetCheckbookRequest(int id)
        {
            var request = await _checkbookRequestService.GetCheckbookRequestAsync(id);

            if (request == null)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Checkbook request not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<CheckbookRequestDto>
            {
                status = "success",
                message = "Checkbook request retrieved successfully.",
                data = request
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckbookRequest(CreateCheckbookRequestDto dto)
        {
            var request = await _checkbookRequestService.CreateCheckbookRequestAsync(dto);

            if (request == null)
            {
                return BadRequest(new CommonResponse<string>
                {
                    status = "error",
                    message = "Invalid AccountId or UserId.",
                    data = null
                });
            }

            return Ok(new CommonResponse<CheckbookRequestDto>
            {
                status = "success",
                message = "Checkbook request created successfully.",
                data = request
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCheckbookRequest(int id, string status)
        {
            var success = await _checkbookRequestService.UpdateCheckbookRequestStatusAsync(id, status);

            if (!success)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Checkbook request not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<string>
            {
                status = "success",
                message = "Checkbook request updated successfully.",
                data = null
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCheckbookRequest(int id)
        {
            var success = await _checkbookRequestService.DeleteCheckbookRequestAsync(id);

            if (!success)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Checkbook request not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<string>
            {
                status = "success",
                message = "Checkbook request deleted successfully.",
                data = null
            });
        }
    }
}
