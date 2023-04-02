using TaskManagement.Application.Consumers.Contracts;

namespace TaskManagement.Application.Services.Contracts;

public interface IServiceBusConsumer
{
    void ReceiveMessage<TMessage>(string queueName, Func<IMessageConsumer<TMessage>> messageConsumerFactory) where TMessage : class;
}
