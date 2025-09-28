using EmployeeManagement.Application.DataTransferObjects.Identity;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;

public class AccountController : ApiBaseController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost("Register")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterEmployee([FromBody] RegisterEmployeeDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _accountService.RegisterEmployeeAsync(dto, cancellationToken);
        return FromResult(result);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await _accountService.LoginAsync(dto, cancellationToken);
        return FromResult(result);
    }
}