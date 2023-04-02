using TaskManagement.Application.Consumers.Contracts;
using TaskManagement.Application.Services.Contracts;

namespace TaskManagement.API.HostedServices;

public class ServiceBusJob : BackgroundService
{
    private readonly List<IServiceBusConsumer> _serviceBusConsumers;
    private readonly IServiceProvider _serviceProvider;

    public ServiceBusJob(IServiceProvider serviceProvider)
    {
        _serviceBusConsumers = new List<IServiceBusConsumer>();
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        RegisterConsumer<Messages.Commands.UpdateTaskCommand>(ServiceBus.Constants.QueueName);

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var consumer in _serviceBusConsumers.Where(c => c is IDisposable))
        {
            (consumer as IDisposable).Dispose();
        }

        return base.StopAsync(cancellationToken);
    }

    private void RegisterConsumer<TMessage>(string queueName) where TMessage : class
    {
        var consumer = _serviceProvider.GetService<IServiceBusConsumer>();
        var messageConsumerFactory = () => _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IMessageConsumer<TMessage>>();

        consumer.ReceiveMessage(queueName, messageConsumerFactory);

        _serviceBusConsumers.Add(consumer);
    }
}
