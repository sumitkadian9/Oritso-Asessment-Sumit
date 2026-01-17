using TaskManagement.API.Common;
using TaskManagement.API.Data;
using TaskManagement.API.DTOs;
using TaskManagement.API.Entities;
using TaskManagement.API.Errors;
using TaskManagement.API.Extensions;
using TaskManagement.API.Interfaces;
using TaskManagement.API.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.API.Services;

public class AccountService(AppDbContext dbContext, IUtilityService utilityService, ITokenService tokenService) : IAccountService
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IUtilityService _utilityService = utilityService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<Result<int>> Register(RegisterDto registerDto)
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        UserDto currentUser = _utilityService.GetCurrentUser();
        bool isDuplicateUser = await _dbContext.Users.Where(u => u.Email == registerDto.Email).AnyAsync();
        if(isDuplicateUser)
        {
            return Result.Fail(CustomErrors.Account.DuplicateUser);
        }

        PasswordHashWithSalt pw = PasswordHelper.GetPasswordWithSalt(registerDto.Password);
        
        User user = registerDto.ToEntity(pw, currentUser, timestamp);

        await _dbContext.Users.AddAsync(user);
        int res = await _dbContext.SaveChangesAsync();
        return res > 0 
            ? Result.Ok(res)
            : Result.Fail(CustomErrors.Account.RegistrationFailed);
    }

    public async Task<Result<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        User user = await _dbContext.Users.Where(u => u.Email == loginRequestDto.Email).FirstOrDefaultAsync();
        if(user is null)
        {
            return Result.Fail(CustomErrors.Account.UserNotFound);
        }

        if(!PasswordHelper.IsValid(loginRequestDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Result.Fail(CustomErrors.Account.InvalidCredentials);
        }

        var loginRes = new LoginResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            Token = _tokenService.CreateToken(user.Id)
        };

        return Result.Ok(loginRes);
    }
}