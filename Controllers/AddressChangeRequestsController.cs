using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPIBankSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressChangeRequestsController : ControllerBase
    {
        private readonly IAddressChangeRequestService _addressChangeRequestService;

        public AddressChangeRequestsController(IAddressChangeRequestService addressChangeRequestService)
        {
            _addressChangeRequestService = addressChangeRequestService;
        }


        [HttpGet]

        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _addressChangeRequestService.GetAllAsync();
            return Ok(new CommonResponse<List<AddressChangeRequestResponseDto>>
            {
                status = "success",
                message = "All address change requests retrieved.",
                data = requests
            });
        }

        [HttpGet("user/{userId}")]

        public async Task<IActionResult> GetRequestsByUserId(int userId)
        {
            var requests = await _addressChangeRequestService.GetRequestsByUserIdAsync(userId);
            return Ok(new CommonResponse<List<AddressChangeRequestDto>>
            {
                status = "success",
                message = "Address change requests for user retrieved.",
                data = requests
            });
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressChangeRequest(int id)
        {
            var request = await _addressChangeRequestService.GetAddressChangeRequestAsync(id);

            if (request == null)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Address change request not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<AddressChangeRequestDto>
            {
                status = "success",
                message = "Address change request retrieved successfully.",
                data = request
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddressChangeRequest(CreateAddressChangeRequestDto dto)
        {
            var request = await _addressChangeRequestService.CreateAddressChangeRequestAsync(dto);

            if (request == null)
            {
                return BadRequest(new CommonResponse<string>
                {
                    status = "error",
                    message = "Invalid UserId or NewAddressId.",
                    data = null
                });
            }

            return Ok(new CommonResponse<AddressChangeRequestDto>
            {
                status = "success",
                message = "Address change request created successfully.",
                data = request
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAddressChangeRequest(int id, string status)
        {
            var success = await _addressChangeRequestService.UpdateAddressChangeRequestStatusAsync(id, status);

            if (!success)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Address change request not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<string>
            {
                status = "success",
                message = "Address change request updated successfully.",
                data = null
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddressChangeRequest(int id)
        {
            var success = await _addressChangeRequestService.DeleteAddressChangeRequestAsync(id);

            if (!success)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Address change request not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<string>
            {
                status = "success",
                message = "Address change request deleted successfully.",
                data = null
            });
        }
    }

}
