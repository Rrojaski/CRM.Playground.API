using CRM.Playground.CRM.Domain.Entities;
using MediatR;

namespace CRM.Playground.CRM.Application.Queries;

public class GetAllLeadsQuery : IRequest<IEnumerable<Lead>>
{
    public Guid? TenantId { get; set; }
    public string? Stage { get; set; }
    public Guid? AssignedTo { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}
