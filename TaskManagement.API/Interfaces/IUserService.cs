using FluentResults;
using TaskManagement.API.DTOs;

namespace TaskManagement.API.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetUserById(Guid userId);
    Task<Result<List<UserDto>>> GetUsers(int pageNumber = 1, int pageSize = 10);
    Task<Result<int>> UpdateUser(UserDto userDto);
    Task<Result<int>> UpdatePassword (UpdatePasswordDto updatePasswordDto);
    Task<Result<int>> DeleteUser(Guid userId);
}