using CRM.Playground.CRM.Controllers;
using CRM.Playground.CRM.Application.Commands;
using CRM.Playground.CRM.Application.Queries;
using CRM.Playground.CRM.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class LeadsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly LeadsController _controller;

    public LeadsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new LeadsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllLeadsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Lead>());
        var result = await _controller.GetAll(null);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WhenLeadExists()
    {
        var lead = new Lead { Id = Guid.NewGuid() };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetLeadByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(lead);
        var result = await _controller.GetById(lead.Id);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenLeadDoesNotExist()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetLeadByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Lead?)null);
        var result = await _controller.GetById(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedResult_WhenValid()
    {
        var command = new CreateLeadCommand { TenantId = Guid.NewGuid(), Email = "test@test.com" };
        var lead = new Lead { Id = Guid.NewGuid(), TenantId = command.TenantId, Email = command.Email };
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(lead);
        var result = await _controller.Create(command);
        Assert.IsType<CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenValid()
    {
        var command = new UpdateLeadCommand { Id = Guid.NewGuid(), TenantId = Guid.NewGuid(), Email = "test@test.com" };
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var result = await _controller.Update(command.Id, command);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenInvalid()
    {
        var command = new UpdateLeadCommand { Id = Guid.NewGuid(), TenantId = Guid.NewGuid(), Email = "test@test.com" };
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var result = await _controller.Update(command.Id, command);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenValid()
    {
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteLeadCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var result = await _controller.Delete(id);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenInvalid()
    {
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteLeadCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var result = await _controller.Delete(id);
        Assert.IsType<NotFoundResult>(result);
    }
}
