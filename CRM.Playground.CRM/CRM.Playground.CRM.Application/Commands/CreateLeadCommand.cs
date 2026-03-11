using CRM.Playground.CRM.Domain.Entities;
using MediatR;

namespace CRM.Playground.CRM.Application.Commands;

public class CreateLeadCommand : IRequest<Lead>
{
    public Guid TenantId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public string? Source { get; set; }
    public string? Stage { get; set; }
    public Guid? AssignedTo { get; set; }
    public int? Score { get; set; }
    public decimal? ExpectedValue { get; set; }
    public string? Notes { get; set; }
    public DateTime? LastContactedAt { get; set; }
}
