using Polly;
using RabbitMQ.Client.Exceptions;
using TaskManagement.Application.Exceptions;

namespace TaskManagement.Application.Resilience.Policies;

public class ResiliencePolicyProvider : IResiliencePolicyProvider
{
    public ISyncPolicy ServiceBusPublishPolicy { get; } = Policy
        .Handle<RabbitMQClientException>()
        .WaitAndRetry(3, num => TimeSpan.FromMilliseconds(num * 200));

    public IAsyncPolicy ServiceBusConsumePolicy { get; } = Policy
        .Handle<Exception>(ex => ex is not TaskNotFoundException)
        .RetryAsync(3);
}
