using CRM.Playground.CRM.Domain.Entities;
using FluentValidation;

namespace CRM.Playground.CRM.Application.Validators;

public class LeadValidator : AbstractValidator<Lead>
{
    public LeadValidator()
    {
        RuleFor(x => x.Email).NotEmpty().MaximumLength(256);
        RuleFor(x => x.FirstName).MaximumLength(100);
        RuleFor(x => x.LastName).MaximumLength(100);
        RuleFor(x => x.Phone).MaximumLength(50);
        RuleFor(x => x.CompanyName).MaximumLength(200);
        RuleFor(x => x.Source).MaximumLength(100);
        RuleFor(x => x.Stage).MaximumLength(50);
        RuleFor(x => x.Notes).MaximumLength(4000);
    }
}
