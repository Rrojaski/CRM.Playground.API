using CRM.Playground.CRM.Domain.Entities;
using CRM.Playground.CRM.Infrastructure.Persistence;
using CRM.Playground.CRM.Application.Commands;
using CRM.Playground.CRM.Application.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Playground.CRM.Application.Handlers;

public class CreateLeadCommandHandler : IRequestHandler<CreateLeadCommand, Lead>
{
    private readonly CrmDbContext _db;
    public CreateLeadCommandHandler(CrmDbContext db) => _db = db;
    public async Task<Lead> Handle(CreateLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            CompanyName = request.CompanyName,
            Source = request.Source,
            Stage = request.Stage,
            AssignedTo = request.AssignedTo,
            Score = request.Score,
            ExpectedValue = request.ExpectedValue,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow,
            LastContactedAt = request.LastContactedAt
        };
        _db.Leads.Add(lead);
        await _db.SaveChangesAsync(cancellationToken);
        return lead;
    }
}

public class UpdateLeadCommandHandler : IRequestHandler<UpdateLeadCommand, bool>
{
    private readonly CrmDbContext _db;
    public UpdateLeadCommandHandler(CrmDbContext db) => _db = db;
    public async Task<bool> Handle(UpdateLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = await _db.Leads.FindAsync(new object[] { request.Id }, cancellationToken);
        if (lead == null) return false;
        lead.TenantId = request.TenantId;
        lead.Email = request.Email;
        lead.FirstName = request.FirstName;
        lead.LastName = request.LastName;
        lead.Phone = request.Phone;
        lead.CompanyName = request.CompanyName;
        lead.Source = request.Source;
        lead.Stage = request.Stage;
        lead.AssignedTo = request.AssignedTo;
        lead.Score = request.Score;
        lead.ExpectedValue = request.ExpectedValue;
        lead.Notes = request.Notes;
        lead.LastContactedAt = request.LastContactedAt;
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeleteLeadCommandHandler : IRequestHandler<DeleteLeadCommand, bool>
{
    private readonly CrmDbContext _db;
    public DeleteLeadCommandHandler(CrmDbContext db) => _db = db;
    public async Task<bool> Handle(DeleteLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = await _db.Leads.FindAsync(new object[] { request.Id }, cancellationToken);
        if (lead == null) return false;
        _db.Leads.Remove(lead);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class GetLeadByIdQueryHandler : IRequestHandler<GetLeadByIdQuery, Lead?>
{
    private readonly CrmDbContext _db;
    public GetLeadByIdQueryHandler(CrmDbContext db) => _db = db;
    public async Task<Lead?> Handle(GetLeadByIdQuery request, CancellationToken cancellationToken)
    {
        return await _db.Leads.FindAsync(new object[] { request.Id }, cancellationToken);
    }
}

public class GetAllLeadsQueryHandler : IRequestHandler<GetAllLeadsQuery, IEnumerable<Lead>>
{
    private readonly CrmDbContext _db;
    public GetAllLeadsQueryHandler(CrmDbContext db) => _db = db;
    public async Task<IEnumerable<Lead>> Handle(GetAllLeadsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Leads.AsQueryable();
        if (request.TenantId.HasValue)
            query = query.Where(x => x.TenantId == request.TenantId.Value);
        if (!string.IsNullOrEmpty(request.Stage))
            query = query.Where(x => x.Stage == request.Stage);
        if (request.AssignedTo.HasValue)
            query = query.Where(x => x.AssignedTo == request.AssignedTo);
        if (request.CreatedFrom.HasValue)
            query = query.Where(x => x.CreatedAt >= request.CreatedFrom.Value);
        if (request.CreatedTo.HasValue)
            query = query.Where(x => x.CreatedAt <= request.CreatedTo.Value);
        return await query.ToListAsync(cancellationToken);
    }
}
