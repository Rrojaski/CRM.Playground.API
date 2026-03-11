using CRM.Playground.CRM.Domain.Entities;
using MediatR;

namespace CRM.Playground.CRM.Application.Queries;

public class GetLeadByIdQuery : IRequest<Lead?>
{
    public Guid Id { get; set; }
}
