using BPIBankSystem.API.Data;
using Microsoft.AspNetCore.Mvc;
using BPIBankSystem.API.Services;
using BPIBankSystem.API.DTOs.Requests;

namespace BPIBankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAllAsync();
            return Ok(new CommonResponse<IEnumerable<AccountDto>>("success", "Fetched all accounts", accounts));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var accounts = await _accountService.GetByUserIdAsync(userId);
            return Ok(new CommonResponse<IEnumerable<AccountDto>>("success", "Fetched user accounts", accounts));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null)
                return NotFound(new CommonResponse<AccountDto>("error", "Account not found", null));

            return Ok(new CommonResponse<AccountDto>("success", "Fetched account", account));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountDto dto)
        {
            try
            {
                var created = await _accountService.CreateAsync(dto);
                return Ok(new CommonResponse<AccountDto>("success", "Account created successfully", created));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new CommonResponse<string>("error", ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountDto dto)
        {
            try
            {
                var success = await _accountService.UpdateAsync(id, dto);
                if (!success)
                    return NotFound(new CommonResponse<string>("error", "Account not found"));

                return Ok(new CommonResponse<string>("success", "Account updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new CommonResponse<string>("error", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _accountService.DeleteAsync(id);
            if (!success)
                return NotFound(new CommonResponse<string>("error", "Account not found"));

            return Ok(new CommonResponse<string>("success", "Account deleted successfully"));
        }
    }

}

