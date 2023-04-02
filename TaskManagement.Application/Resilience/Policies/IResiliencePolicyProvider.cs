using Polly;

namespace TaskManagement.Application.Resilience.Policies;

public interface IResiliencePolicyProvider
{
    ISyncPolicy ServiceBusPublishPolicy { get; }

    IAsyncPolicy ServiceBusConsumePolicy { get; }
}
