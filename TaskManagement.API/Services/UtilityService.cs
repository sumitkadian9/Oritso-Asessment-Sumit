using TaskManagement.API.DTOs;
using TaskManagement.API.Interfaces;

namespace TaskManagement.API.Services;

public class UtilityService(IHttpContextAccessor httpContextAccessor) : IUtilityService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public UserDto GetCurrentUser()
    {
        return (UserDto)_httpContextAccessor?.HttpContext?.Items["User"];
    }

}