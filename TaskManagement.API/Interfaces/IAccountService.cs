using TaskManagement.API.DTOs;
using FluentResults;

namespace TaskManagement.API.Interfaces;

public interface IAccountService
{
    Task<Result<int>> Register (RegisterDto registerDto);
    Task<Result<LoginResponseDto>> Login (LoginRequestDto loginRequestDto);
}