using Microsoft.AspNetCore.Mvc;
using BPIBankSystem.API.Services;

using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;

[Route("api/[controller]")]
[ApiController]
public class StatementController : ControllerBase
{
    private readonly IStatementService _statementService;

    public StatementController(IStatementService statementService)
    {
        _statementService = statementService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatement(int accountId, string type, int? month, int year)
    {
        var statement = await _statementService.GetStatementAsync(accountId, type, month, year);

        if (statement == null)
        {
            return NotFound(new CommonResponse<string>
            {
                status = "error",
                message = "Account not found.",
                data = null
            });
        }

        return Ok(new CommonResponse<StatementResultDto>
        {
            status = "success",
            message = "Statement retrieved successfully.",
            data = statement
        });
    }

}
