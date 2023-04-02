namespace TaskManagement.Messages.Commands;

public record UpdateTaskCommand(int Id, int Status, string UpdatedBy);