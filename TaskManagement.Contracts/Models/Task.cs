namespace TaskManagement.Contracts.Models;

public class Task
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Enums.TaskStatus Status { get; set; }

    public string AssignedTo { get; set; }


    public string CreatedBy { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }
}
