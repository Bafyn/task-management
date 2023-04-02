namespace TaskManagement.Domain.Entities;

public class Task
{
    public int TaskId { get; set; }

    public string TaskName { get; set; }

    public string Description { get; set; }

    public int Status { get; set; }

    public string AssignedTo { get; set; }

    public TaskStatus TaskStatus { get; set; }


    public string CreatedBy { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
