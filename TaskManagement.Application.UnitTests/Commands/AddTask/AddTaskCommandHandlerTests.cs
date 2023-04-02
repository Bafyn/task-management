using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TaskManagement.Application.Commands.AddTask;
using TaskManagement.Domain.Repositories;
using TaskManagement.Persistence.UnitOfWork;
using Xunit;

namespace TaskManagement.Application.Tests.Commands.AddTask;

public class AddTaskCommandHandlerTests
{
    private readonly Fixture _fixture = new();

    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly AddTaskCommandHandler _sut;

    public AddTaskCommandHandlerTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _sut = new AddTaskCommandHandler(_taskRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Should_CreateNewTask_When_ArgumentsAreValid()
    {
        // Arrange
        var command = _fixture.Create<AddTaskCommand>();
        var task = _fixture.Create<Domain.Entities.Task>();

        _mapperMock.Setup(m => m.Map<Domain.Entities.Task>(command)).Returns(task);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(task.TaskId);

        task.Status.Should().Be((int)Contracts.Enums.TaskStatus.NotStarted);
        task.CreatedOn.Should().BeWithin(TimeSpan.FromSeconds(5)).Before(DateTime.UtcNow);

        _taskRepositoryMock.Verify(r => r.Create(task), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.VerifyAll();
    }
}
