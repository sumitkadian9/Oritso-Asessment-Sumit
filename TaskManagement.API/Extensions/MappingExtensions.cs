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

    public static TodoTask ToEntity(this TodoTaskDto dto, UserDto currentUser, long timestamp)
    {
        return new TodoTask
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Status = dto.Status,
            Remarks = dto.Remarks,
            UserId = currentUser.Id
        }.SetMetaData(currentUser.Id, timestamp);
    }

    public static TodoTask UpdateFromDto(this TodoTask todoTask, TodoTaskDto dto, UserDto currentUser, long timestamp)
    {
        todoTask.Title = dto.Title;
        todoTask.Description = dto.Description;
        todoTask.DueDate = dto.DueDate;
        todoTask.Status = dto.Status;
        todoTask.Remarks = dto.Remarks;
        todoTask.SetMetaData(currentUser.Id, timestamp, Enums.EntityState.Modified);
        return todoTask;
    }

    public static TodoTaskDto ToDto(this TodoTask entity)
    {
        return new TodoTaskDto
        {
            Id = entity.Id,
            Description = entity.Description,
            DueDate = entity.DueDate,
            Status = entity.Status,
            Remarks = entity.Remarks,
            CreatedOn = entity.CreatedOn,
            LastUpdatedOn = entity.LastUpdatedOn,
            User = entity.User != null ? new UserMinDto
            {
                Id = entity.UserId,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName
            } : null
        };
    }
}