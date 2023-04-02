namespace TaskManagement.ServiceBus;

// These keys can be configured in app settings, but for the sake of simplicity they are part of this project
public class Constants
{
    public const string ExchangeName = "update-task-exchange";
    public const string QueueName = "update-task-queue";
    public const string RoutingKey = "*";
}
