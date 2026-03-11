using CRM.Playground.CRM.Domain.Entities;
using MediatR;

namespace CRM.Playground.CRM.Application.Queries;

public class GetAllLeadsQuery : IRequest<IEnumerable<Lead>>
{
    public Guid? TenantId { get; set; }
}
