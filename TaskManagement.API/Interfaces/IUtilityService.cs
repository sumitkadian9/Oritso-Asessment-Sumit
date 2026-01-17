using TaskManagement.API.DTOs;

namespace TaskManagement.API.Interfaces;

public interface IUtilityService
{
    UserDto GetCurrentUser();
}