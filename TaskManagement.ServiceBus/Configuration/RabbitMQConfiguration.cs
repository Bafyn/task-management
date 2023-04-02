namespace TaskManagement.ServiceBus.Configuration;

public class RabbitMQConfiguration
{
    public string Host { get; init; }

    public ushort Port { get; init; }

    public string Username { get; set; }

    public string Password { get; set; }
}
