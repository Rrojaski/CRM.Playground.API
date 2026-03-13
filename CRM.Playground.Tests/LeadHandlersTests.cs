using CRM.Playground.CRM.Application.Commands;
using CRM.Playground.CRM.Application.Queries;
using CRM.Playground.CRM.Application.Handlers;
using CRM.Playground.CRM.Domain.Entities;
using CRM.Playground.CRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class LeadHandlersTests
{
    private DbContextOptions<CrmDbContext> DbOptions => new DbContextOptionsBuilder<CrmDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    [Fact]
    public async Task CreateLeadCommandHandler_CreatesLead()
    {
        var db = new CrmDbContext(DbOptions);
        var handler = new CreateLeadCommandHandler(db);
        var command = new CreateLeadCommand { TenantId = Guid.NewGuid(), Email = "test@test.com" };
        var result = await handler.Handle(command, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(command.Email, result.Email);
    }

    [Fact]
    public async Task UpdateLeadCommandHandler_UpdatesLead()
    {
        var db = new CrmDbContext(DbOptions);
        var lead = new Lead { Id = Guid.NewGuid(), TenantId = Guid.NewGuid(), Email = "old@test.com", CreatedAt = DateTime.UtcNow };
        db.Leads.Add(lead);
        db.SaveChanges();
        var handler = new UpdateLeadCommandHandler(db);
        var command = new UpdateLeadCommand { Id = lead.Id, TenantId = lead.TenantId, Email = "new@test.com" };
        var result = await handler.Handle(command, CancellationToken.None);
        Assert.True(result);
        Assert.Equal("new@test.com", db.Leads.Find(lead.Id)?.Email);
    }

    [Fact]
    public async Task DeleteLeadCommandHandler_DeletesLead()
    {
        var db = new CrmDbContext(DbOptions);
        var lead = new Lead { Id = Guid.NewGuid(), TenantId = Guid.NewGuid(), Email = "delete@test.com", CreatedAt = DateTime.UtcNow };
        db.Leads.Add(lead);
        db.SaveChanges();
        var handler = new DeleteLeadCommandHandler(db);
        var command = new DeleteLeadCommand { Id = lead.Id };
        var result = await handler.Handle(command, CancellationToken.None);
        Assert.True(result);
        Assert.Null(db.Leads.Find(lead.Id));
    }

    [Fact]
    public async Task GetLeadByIdQueryHandler_ReturnsLead()
    {
        var db = new CrmDbContext(DbOptions);
        var lead = new Lead { Id = Guid.NewGuid(), TenantId = Guid.NewGuid(), Email = "find@test.com", CreatedAt = DateTime.UtcNow };
        db.Leads.Add(lead);
        db.SaveChanges();
        var handler = new GetLeadByIdQueryHandler(db);
        var query = new GetLeadByIdQuery { Id = lead.Id };
        var result = await handler.Handle(query, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(lead.Id, result.Id);
    }

    [Fact]
    public async Task GetAllLeadsQueryHandler_ReturnsLeads()
    {
        var db = new CrmDbContext(DbOptions);
        var lead1 = new Lead { Id = Guid.NewGuid(), TenantId = Guid.NewGuid(), Email = "a@test.com", CreatedAt = DateTime.UtcNow };
        var lead2 = new Lead { Id = Guid.NewGuid(), TenantId = lead1.TenantId, Email = "b@test.com", CreatedAt = DateTime.UtcNow };
        db.Leads.AddRange(lead1, lead2);
        db.SaveChanges();
        var handler = new GetAllLeadsQueryHandler(db);
        var query = new GetAllLeadsQuery { TenantId = lead1.TenantId };
        var result = await handler.Handle(query, CancellationToken.None);
        Assert.Equal(2, result.Count());
    }
}
