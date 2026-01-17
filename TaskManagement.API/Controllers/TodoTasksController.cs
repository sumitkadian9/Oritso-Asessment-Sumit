using TaskManagement.API.DTOs;
using TaskManagement.API.Enums;
using TaskManagement.API.Extensions;
using TaskManagement.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Attributes;

namespace TaskManagement.API.Controllers;

public class TodoTasksController(ITodoTaskService todoTaskService) : BaseApiController
{
    private readonly ITodoTaskService _todoTaskService = todoTaskService;

    [TMAuthorize]
    [HttpPost]
    public async Task<IActionResult> CreateTodoTask(TodoTaskDto todoTaskDto)
    {
        var result = await _todoTaskService.CreateTodoTask(todoTaskDto);
        return result.ToProblemDetailsOrOk(this);
    }
    
    [TMAuthorize]
    [HttpGet("{todoTaskId}")]
    public async Task<IActionResult> GetTodoTaskById(string todoTaskId)
    {
        var result = await _todoTaskService.GetTodoTaskById(Guid.Parse(todoTaskId));
        return result.ToProblemDetailsOrOk(this);
    }

    [TMAuthorize]
    [HttpGet]
    public async Task<IActionResult> GetTodoTasks(string searchParam, int pageNumber = 1, int pageSize = 10)
    {
        var result = await _todoTaskService.GetTodoTasks(searchParam, pageNumber, pageSize);
        return result.ToProblemDetailsOrOk(this);
    }

    [TMAuthorize]
    [HttpPut]
    public async Task<IActionResult> UpdateTodoTask(TodoTaskDto todoTaskDto)
    {
        var result = await _todoTaskService.UpdateTodoTask(todoTaskDto);
        return result.ToProblemDetailsOrOk(this);
    }

    [TMAuthorize]
    [HttpDelete("{todoTaskId}")]
    public async Task<IActionResult> DeleteTodoTask(string todoTaskId)
    {
        var result = await _todoTaskService.DeleteTodoTask(Guid.Parse(todoTaskId));
        return result.ToProblemDetailsOrOk(this);
    }
}