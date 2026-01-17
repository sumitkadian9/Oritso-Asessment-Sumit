using TaskManagement.API.Enums;

namespace TaskManagement.API.DTOs;

public class TodoTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly DueDate { get; set; }
    public TaskCompletionStatus Status { get; set;}
    public string Remarks { get; set; }
    public UserMinDto User { get; set; }
    public long CreatedOn { get; set; }
    public long LastUpdatedOn { get; set; }
}