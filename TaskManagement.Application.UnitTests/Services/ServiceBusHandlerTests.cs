using AutoFixture;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TaskManagement.Application.Consumers.Contracts;
using TaskManagement.Application.Resilience.Policies;
using TaskManagement.Application.Services;
using TaskManagement.Messages.Commands;
using Xunit;

namespace TaskManagement.Application.Tests.Services;

public class ServiceBusHandlerTests
{
    private readonly Fixture _fixture = new();

    private readonly Mock<IResiliencePolicyProvider> _policyProviderMock;
    private readonly Mock<IConnection> _rabbitMQConnectionMock;
    private readonly Mock<IModel> _channelMock;

    private readonly ServiceBusHandler _sut;

    public ServiceBusHandlerTests()
    {
        _rabbitMQConnectionMock = new Mock<IConnection>();
        _policyProviderMock = new Mock<IResiliencePolicyProvider>();
        _channelMock = new Mock<IModel>();

        _policyProviderMock.Setup(p => p.ServiceBusPublishPolicy.Execute(It.IsAny<Action>()))
            .Callback((Action action) => action());

        _rabbitMQConnectionMock.Setup(c => c.CreateModel()).Returns(_channelMock.Object);

        _sut = new ServiceBusHandler(_rabbitMQConnectionMock.Object, _policyProviderMock.Object);
    }

    [Fact]
    public void SendMessage_WhenMessageIsValid_PublishesMessage()
    {
        // Arrange
        var message = _fixture.Create<UpdateTaskCommand>();
        var expectedRoutingKey = nameof(UpdateTaskCommand);

        // Act
        _sut.SendMessage(message);

        // Assert
        _policyProviderMock.VerifyGet(p => p.ServiceBusPublishPolicy);
        _channelMock.Verify(
            c => c.BasicPublish(
                ServiceBus.Constants.ExchangeName,
                expectedRoutingKey,
                false,
                null,
                It.Is<ReadOnlyMemory<byte>>(body => CompareMessagePayload(message, body))),
            Times.Once);
    }

    [Fact]
    public void ReceiveMessage_ByDefault_SubscribesToMessages()
    {
        // Arrange
        var queueName = _fixture.Create<string>();
        var messageConsumer = new Mock<IMessageConsumer<UpdateTaskCommand>>();

        _channelMock
            .Setup(c => c.BasicConsume(
                queueName,
                true,
                string.Empty,
                false,
                false,
                null,
                It.Is<IBasicConsumer>(b => b is AsyncEventingBasicConsumer && (b as AsyncEventingBasicConsumer).Model == _channelMock.Object)))
            .Returns(_fixture.Create<string>());


        // Act
        _sut.ReceiveMessage(queueName, () => messageConsumer.Object);

        // Assert
        _channelMock.VerifyAll();
    }

    private static bool CompareMessagePayload(UpdateTaskCommand expected, ReadOnlyMemory<byte> actual)
    {
        var serializedEvent = Encoding.UTF8.GetString(actual.ToArray());
        var message = JsonSerializer.Deserialize<UpdateTaskCommand>(serializedEvent);

        return message.Id == expected.Id
            && message.Status == expected.Status
            && message.UpdatedBy == expected.UpdatedBy;
    }
}

