// using TaskManagement.API.Attributes;
using TaskManagement.API.DTOs;
using TaskManagement.API.Enums;
using TaskManagement.API.Extensions;
using TaskManagement.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.API.Controllers;

public class AccountsController : BaseApiController
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _accountService.Register(registerDto);
        return result.ToProblemDetailsOrOk(this);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
    {
        var result = await _accountService.Login(loginRequestDto);
        return result.ToProblemDetailsOrOk(this);
    }
}