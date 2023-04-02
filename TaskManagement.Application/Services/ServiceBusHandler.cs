using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TaskManagement.Application.Consumers.Contracts;
using TaskManagement.Application.Resilience.Policies;
using TaskManagement.Application.Services.Contracts;

namespace TaskManagement.Application.Services;

public class ServiceBusHandler : IServiceBusProducer, IServiceBusConsumer, IDisposable
{
    private bool _disposed;

    private readonly IModel _channel;
    private readonly IResiliencePolicyProvider _policyProvider;

    public ServiceBusHandler(IConnection rabbitMQConnection, IResiliencePolicyProvider policyProvider)
    {
        _channel = rabbitMQConnection.CreateModel();
        _policyProvider = policyProvider;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // NOTE: dispose managed state (managed objects)
            }

            _channel.Dispose();
            _disposed = true;
        }
    }

    ~ServiceBusHandler()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void SendMessage<T>(T message) where T : class
    {
        var serializedMessage = JsonSerializer.Serialize(message);
        var messageBytes = Encoding.UTF8.GetBytes(serializedMessage);
        var routingKey = typeof(T).Name;

        _policyProvider.ServiceBusPublishPolicy.Execute(() =>
        {
            _channel.BasicPublish(ServiceBus.Constants.ExchangeName, routingKey, null, messageBytes);
        });
    }

    public void ReceiveMessage<TMessage>(string queueName, Func<IMessageConsumer<TMessage>> messageConsumerFactory)
        where TMessage : class
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += ConsumeAsync;

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        async Task ConsumeAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            // Create a new consumer for each message.
            var messageConsumer = messageConsumerFactory();
            var body = eventArgs.Body.ToArray();
            var serializedEvent = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<TMessage>(serializedEvent);

            await _policyProvider.ServiceBusConsumePolicy.ExecuteAsync(() => messageConsumer.ConsumeAsync(message));
        }
    }
}
