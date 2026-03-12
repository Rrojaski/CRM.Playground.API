using MediatR;

namespace CRM.Playground.CRM.Application.Commands;

public class DeleteLeadCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
