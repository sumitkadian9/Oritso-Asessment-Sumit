using FluentResults;
using TaskManagement.API.Data;
using TaskManagement.API.DTOs;
using TaskManagement.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Errors;
using TaskManagement.API.Extensions;
using TaskManagement.API.Entities;

namespace TaskManagement.API.Services;

public class TodoTaskService(AppDbContext dbContext, IUtilityService utilityService) : ITodoTaskService
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IUtilityService _utilityService = utilityService;

    public async Task<Result<TodoTaskDto>> CreateTodoTask(TodoTaskDto todoTaskDto)
    {
        var currentUser = _utilityService.GetCurrentUser();
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        TodoTask todoTask = todoTaskDto.ToEntity(currentUser, timestamp);
        await _dbContext.TodoTasks.AddAsync(todoTask);
        int res = await _dbContext.SaveChangesAsync();

        if (res <= 0)
        {
            return Result.Fail(CustomErrors.TodoTask.TodoTaskCreationFailed);
        } 

        var createdTask = await _dbContext.TodoTasks
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == todoTask.Id);

        return Result.Ok(createdTask!.ToDto());
    }
    
    public async Task<Result<TodoTaskDto>> GetTodoTaskById(Guid todoTaskId)
    {
        var currentUser = _utilityService.GetCurrentUser();
        
        //Filtering by current user. A user can only see his own tasks
        var task = await _dbContext.TodoTasks.AsNoTracking()
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.Id == todoTaskId && t.UserId == currentUser.Id);

        if (task == null)
        {
            return Result.Fail(CustomErrors.TodoTask.TodoTaskNotFound);
        } 

        return Result.Ok(task.ToDto());
    }

    public async Task<Result<List<TodoTaskDto>>> GetTodoTasks(string searchParam, int pageNumber = 1, int pageSize = 10)
    {
        var currentUser = _utilityService.GetCurrentUser();
        
        var query = _dbContext.TodoTasks.AsNoTracking()
                    .Include(t => t.User)
                    .Where(t => t.UserId == currentUser.Id)
                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchParam))
        {
            query = query.Where(t => 
                    t.Title.Contains(searchParam) || 
                    t.Description.Contains(searchParam));
        }

        var tasks = await query
                    .OrderByDescending(t => t.CreatedOn)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        return Result.Ok(tasks.Select(t => t.ToDto()).ToList());
    }

    public async Task<Result<int>> UpdateTodoTask(TodoTaskDto todoTaskDto)
    {
        var currentUser = _utilityService.GetCurrentUser();
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var task = await _dbContext.TodoTasks
                    .FirstOrDefaultAsync(t => t.Id == todoTaskDto.Id && t.UserId == currentUser.Id);

        if (task == null) 
        {
            return Result.Fail(CustomErrors.TodoTask.TodoTaskNotFound);
        }

        task.UpdateFromDto(todoTaskDto, currentUser, timestamp);

        int res = await _dbContext.SaveChangesAsync();
        return res > 0 ? Result.Ok(res) : Result.Fail(CustomErrors.TodoTask.TodoTaskUpdateFailed);
    }

    public async Task<Result<int>> DeleteTodoTask(Guid todoTaskId)
    {
        var currentUser = _utilityService.GetCurrentUser();

        var task = await _dbContext.TodoTasks
                    .FirstOrDefaultAsync(t => t.Id == todoTaskId && t.UserId == currentUser.Id);

        if (task == null) 
        {
            return Result.Fail(CustomErrors.TodoTask.TodoTaskNotFound);
        }

        _dbContext.TodoTasks.Remove(task);
        int res = await _dbContext.SaveChangesAsync();
        
        return res > 0 ? Result.Ok(res) : Result.Fail(CustomErrors.TodoTask.TodoTaskDeletionFailed);
    }
}