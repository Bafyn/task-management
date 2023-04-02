namespace TaskManagement.Application.Services.Contracts;

public interface IServiceBusProducer
{
    void SendMessage<T>(T message) where T : class;
}
