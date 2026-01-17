using TaskManagement.API.Enums;

namespace TaskManagement.API.Entities;

public class TodoTask : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly DueDate { get; set; }
    public TaskCompletionStatus Status { get; set;}
    public string Remarks { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}