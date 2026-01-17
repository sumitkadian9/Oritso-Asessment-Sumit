using TaskManagement.API.Common;
using TaskManagement.API.Data;
using TaskManagement.API.DTOs;
using TaskManagement.API.Entities;
using TaskManagement.API.Enums;
using TaskManagement.API.Errors;
using TaskManagement.API.Extensions;
using TaskManagement.API.Interfaces;
using TaskManagement.API.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.API.Services;

public class UserService(AppDbContext dbContext, IUtilityService utilityService) : IUserService
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IUtilityService _utilityService = utilityService;

    public async Task<Result<UserDto>> GetUserById(Guid userId)
    {
        var user = await _dbContext.Users.ById(userId).Select(u => u.ToDto()).FirstOrDefaultAsync();
        if(user is null)
        {
            return Result.Fail(CustomErrors.Account.UserNotFound);
        }

        return Result.Ok(user);
    }
    
    public async Task<Result<List<UserDto>>> GetUsers(int pageNumber = 1, int pageSize = 10)
    {
        var users = await _dbContext.Users.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(u => u.ToDto()).ToListAsync();
        return Result.Ok(users);
    }

    public async Task<Result<int>> UpdateUser(UserDto userDto)
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        UserDto currentUser = _utilityService.GetCurrentUser();
        User user = await _dbContext.Users.ById(userDto.Id).FirstOrDefaultAsync();
        if(user is null)
        {
            return Result.Fail(CustomErrors.Account.UserNotFound);
        }
        
        user.UpdateFromDto(userDto, currentUser, timestamp);
        
        int res = await _dbContext.SaveChangesAsync();
        return res > 0 
            ? Result.Ok(res)
            : Result.Fail(CustomErrors.Account.UserUpdateFailed);
    }
        
    public async Task<Result<int>> UpdatePassword(UpdatePasswordDto updatePasswordDto)
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        UserDto currentUser = _utilityService.GetCurrentUser();
        User user = await _dbContext.Users.ById(updatePasswordDto.UserId).FirstOrDefaultAsync();
        if(user is null)
        {
            return Result.Fail(CustomErrors.Account.UserNotFound);
        }

        PasswordHashWithSalt pw = PasswordHelper.GetPasswordWithSalt(updatePasswordDto.Password);

        user.PasswordHash = pw.Hash;
        user.PasswordSalt = pw.Salt;

        user.SetMetaData(currentUser?.Id ?? updatePasswordDto.UserId, timestamp, Enums.EntityState.Modified);
        
        int res = await _dbContext.SaveChangesAsync();
        return res > 0 
            ? Result.Ok(res)
            : Result.Fail(CustomErrors.Account.PasswordUpdateFailed);
    }
    
    public async Task<Result<int>> DeleteUser(Guid userId)
    {
        UserDto currentUser = _utilityService.GetCurrentUser();
        User user = await _dbContext.Users.ById(userId).FirstOrDefaultAsync();
        if(user is null)
        {
            return Result.Fail(CustomErrors.Account.UserNotFound);
        }

        if(user.Id == currentUser.Id)
        {
            return Result.Fail(CustomErrors.Account.UserNotDeletable);
        }

        if(user.Role == Role.Admin)
        {
            return Result.Fail(CustomErrors.Account.UserNotDeletable);
        }

        _dbContext.Remove(user);

        int res = await _dbContext.SaveChangesAsync();
        return res > 0 
            ? Result.Ok(res)
            : Result.Fail(CustomErrors.Account.UserDeletionFailed);
    }
}