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
    public class ChecksController : ControllerBase
    {
        private readonly ICheckService _checkService;

        public ChecksController(ICheckService checkService)
        {
            _checkService = checkService;
        }


        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetChecksByAccount(int accountId)
        {
            var checks = await _checkService.GetChecksByAccountIdAsync(accountId);
            return Ok(new CommonResponse<List<CheckDto>>
            {
                status = "success",
                message = "Checks for account retrieved.",
                data = checks
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChecks()
        {
            var checks = await _checkService.GetAllChecksAsync();
            return Ok(new CommonResponse<List<CheckResponseDto>>
            {
                status = "success",
                message = "All checks retrieved.",
                data = checks
            });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCheck(int id)
        {
            var check = await _checkService.GetCheckAsync(id);

            if (check == null)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Check not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<CheckDto>
            {
                status = "success",
                message = "Check retrieved successfully.",
                data = check
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheck(CreateCheckDto dto)
        {
            var check = await _checkService.CreateCheckAsync(dto);

            if (check == null)
            {
                return BadRequest(new CommonResponse<string>
                {
                    status = "error",
                    message = "Invalid AccountId.",
                    data = null
                });
            }

            return Ok(new CommonResponse<CheckDto>
            {
                status = "success",
                message = "Check created successfully.",
                data = check
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCheck(int id, UpdateCheckDto dto)
        {
            var success = await _checkService.UpdateCheckAsync(id, dto);

            if (!success)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Check not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<string>
            {
                status = "success",
                message = "Check updated successfully.",
                data = null
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCheck(int id)
        {
            var success = await _checkService.DeleteCheckAsync(id);

            if (!success)
            {
                return NotFound(new CommonResponse<string>
                {
                    status = "error",
                    message = "Check not found.",
                    data = null
                });
            }

            return Ok(new CommonResponse<string>
            {
                status = "success",
                message = "Check deleted successfully.",
                data = null
            });
        }
    }
}
