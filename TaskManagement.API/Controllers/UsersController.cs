using TaskManagement.API.DTOs;
using TaskManagement.API.Enums;
using TaskManagement.API.Extensions;
using TaskManagement.API.Interfaces;
using TaskManagement.API.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.API.Controllers;

public class UsersController(IUserService userService) : BaseApiController
{
    private readonly IUserService _userService = userService;

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var result = await _userService.GetUserById(Guid.Parse(userId));
        return result.ToProblemDetailsOrOk(this);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(int pageNumber = 1, int pageSize = 10)
    {
        var result = await _userService.GetUsers(pageNumber, pageSize);
        return result.ToProblemDetailsOrOk(this);
    }

    [TMAuthorize(Role.Admin)]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UserDto userDto)
    {
        var result = await _userService.UpdateUser(userDto);
        return result.ToProblemDetailsOrOk(this);
    }

    [TMAuthorize(Role.Admin)]
    [HttpPatch("updatePassword")]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordDto updatePasswordDto)
    {
        var result = await _userService.UpdatePassword(updatePasswordDto);
        return result.ToProblemDetailsOrOk(this);
    }

    [TMAuthorize(Role.Admin)]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await _userService.DeleteUser(Guid.Parse(userId));
        return result.ToProblemDetailsOrOk(this);
    }
}