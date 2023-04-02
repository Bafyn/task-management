namespace TaskManagement.Application.Exceptions;

public class TaskNotFoundException : Exception
{
    public int TaskId { get; set; }

    public TaskNotFoundException(int taskId) : base("Task with the specified id wasn't found")
    {
        TaskId = taskId;
    }
}
