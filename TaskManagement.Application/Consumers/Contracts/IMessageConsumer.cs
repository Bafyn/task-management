namespace TaskManagement.Application.Consumers.Contracts;

public interface IMessageConsumer<T> where T : class
{
    public Task ConsumeAsync(T message);
}
