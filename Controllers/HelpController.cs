using BPIBankSystem.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPIBankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpController : ControllerBase
    {
        private readonly IHelpService _helpService;

        public HelpController(IHelpService helpService)
        {
            _helpService = helpService;
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetAllHelps()
        {
            var helps = await _helpService.GetAllHelpsAsync();
            return Ok(new { status = "success", message = "Fetched", data = helps });
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _helpService.GetAllCategoriesAsync();
            return Ok(new { status = "success", message = "Fetched", data = categories });
        }
    }
}
