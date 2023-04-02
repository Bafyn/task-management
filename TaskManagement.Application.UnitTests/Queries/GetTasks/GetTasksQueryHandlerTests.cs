using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Queries.GetTasks;
using TaskManagement.Domain.Repositories;
using Xunit;

namespace TaskManagement.Application.Tests.Queries.GetTasks;

public class GetTasksQueryHandlerTests
{
    private readonly Fixture _fixture = new();

    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly GetTasksQueryHandler _sut;

    public GetTasksQueryHandlerTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _mapperMock = new Mock<IMapper>();

        _sut = new GetTasksQueryHandler(_taskRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Should_ReturnTasks_When_TasksExist()
    {
        // Arrange
        var query = _fixture.Create<GetTasksQuery>();
        var tasks = _fixture.CreateMany<Domain.Entities.Task>(3).ToList();
        var mappedTasks = _fixture.CreateMany<Contracts.Models.Task>(3).ToList();

        _taskRepositoryMock.Setup(r => r.GetAllTasksAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tasks);
        _mapperMock.Setup(m => m.Map<List<Contracts.Models.Task>>(tasks)).Returns(mappedTasks);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Tasks.Should().BeSameAs(mappedTasks);

        _taskRepositoryMock.VerifyAll();
        _mapperMock.VerifyAll();
    }
}
