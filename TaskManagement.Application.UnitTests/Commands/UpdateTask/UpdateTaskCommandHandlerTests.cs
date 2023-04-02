using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TaskManagement.Application.Commands.UpdateTask;
using TaskManagement.Application.Services.Contracts;
using Xunit;

namespace TaskManagement.Application.Tests.Commands.UpdateTask;

public class UpdateTaskCommandHandlerTests
{
    private readonly Fixture _fixture = new();

    private readonly Mock<IServiceBusProducer> _serviceBusProducerMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly UpdateTaskCommandHandler _sut;

    public UpdateTaskCommandHandlerTests()
    {
        _serviceBusProducerMock = new Mock<IServiceBusProducer>();
        _mapperMock = new Mock<IMapper>();

        _sut = new UpdateTaskCommandHandler(_serviceBusProducerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Should_SendMessage_When_Invoked()
    {
        // Arrange
        var command = _fixture.Create<UpdateTaskCommand>();
        var expectedMessage = _fixture.Create<Messages.Commands.UpdateTaskCommand>();

        _mapperMock.Setup(m => m.Map<Messages.Commands.UpdateTaskCommand>(command)).Returns(expectedMessage);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        _serviceBusProducerMock.Verify(p => p.SendMessage(expectedMessage), Times.Once);
        _mapperMock.VerifyAll();
    }
}
