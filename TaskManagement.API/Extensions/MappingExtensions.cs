using TaskManagement.API.DTOs;
using TaskManagement.API.Entities;
using TaskManagement.API.Models;

namespace TaskManagement.API.Extensions;
public static class MappingExtensions
{
    public static User ToEntity(this RegisterDto dto, PasswordHashWithSalt password, UserDto currentUser, long timestamp)
    {
        return new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Role = Enums.Role.Member,
            PasswordHash = password.Hash,
            PasswordSalt = password.Salt
        }.SetMetaData(currentUser.Id, timestamp);
    }

    public static User UpdateFromDto(this User user, UserDto dto, UserDto currentUser, long timestamp)
    {
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.Role = dto.Role;
        user.SetMetaData(currentUser.Id, timestamp, Enums.EntityState.Modified);
        return user;
    }

    public static UserDto ToDto(this User entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Role = entity.Role
        };
    }
}