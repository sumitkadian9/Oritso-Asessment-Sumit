using FluentResults;
using TaskManagement.API.DTOs;

namespace TaskManagement.API.Interfaces;

public interface ITodoTaskService
{
    Task<Result<TodoTaskDto>> CreateTodoTask(TodoTaskDto todoTaskDto);
    Task<Result<TodoTaskDto>> GetTodoTaskById(Guid TodoTaskId);
    Task<Result<List<TodoTaskDto>>> GetTodoTasks(string searchParam, int pageNumber = 1, int pageSize = 10);
    Task<Result<int>> UpdateTodoTask(TodoTaskDto TodoTaskDto);
    Task<Result<int>> DeleteTodoTask(Guid TodoTaskId);
}