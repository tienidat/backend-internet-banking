using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPIBankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly AppDbContext _context;

        public AddressController(IAddressService addressService, AppDbContext context)
        {
            _addressService = addressService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _addressService.GetAllAsync();
            return Ok(new CommonResponse<List<AddressResponseDto>>("success", "All addresses fetched", result));
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUser(int userId)
        {
            var result = await _addressService.GetAllByUserIdAsync(userId);
            return Ok(new CommonResponse<List<AddressDto>>("success", "Fetched", result));
        }

        [HttpGet("user/{userId}/current")]
        public async Task<IActionResult> GetCurrentAddress(int userId)
        {
            var result = await _addressService.GetCurrentAddressAsync(userId);
            return Ok(new CommonResponse<AddressDto>("success", "Current address", result));
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] AddressCreateDto dto)
        {
            try
            {
                var success = await _addressService.AddAddressAsync(dto);
                if (success)
                {
                    var newAddress = await _context.Addresses
                        .OrderByDescending(a => a.Id)
                        .FirstOrDefaultAsync(a => a.UserId == dto.UserId);
                    return Ok(new CommonResponse<int>
                    {
                        status = "success",
                        message = "Address added successfully.",
                        data = newAddress?.Id ?? 0
                    });
                }
                return BadRequest(new CommonResponse<string>
                {
                    status = "error",
                    message = "Failed to add address.",
                    data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonResponse<string>
                {
                    status = "error",
                    message = $"Failed to add address: {ex.Message}",
                    data = null
                });
            }
        }

        [HttpPut("{addressId}")]
        public async Task<IActionResult> UpdateAddress(int addressId, [FromBody] AddressUpdateDto dto)
        {
            var success = await _addressService.UpdateAddressAsync(addressId, dto);
            return Ok(new CommonResponse<string>("success", success ? "Updated" : "Not found", null));
        }

        [HttpPatch("{addressId}/mark-old")]
        public async Task<IActionResult> MarkAsOld(int addressId)
        {
            var success = await _addressService.MarkAddressAsOldAsync(addressId);
            return Ok(new CommonResponse<string>("success", success ? "Marked as old" : "Not found", null));
        }
    }
}


