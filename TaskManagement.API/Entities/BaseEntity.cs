namespace TaskManagement.API.Entities;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long CreatedOn { get; set; }
    public long LastUpdatedOn { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid LastUpdatedBy { get; set; }
}