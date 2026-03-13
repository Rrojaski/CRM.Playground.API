using CRM.Playground.CRM.Domain.Entities;
using CRM.Playground.CRM.Application.Validators;
using Xunit;

public class LeadValidatorTests
{
    private readonly LeadValidator _validator = new LeadValidator();

    [Fact]
    public void ValidLead_PassesValidation()
    {
        var lead = new Lead
        {
            Email = "test@test.com",
            FirstName = "John",
            LastName = "Doe",
            Phone = "1234567890",
            CompanyName = "TestCo",
            Source = "Website",
            Stage = "New",
            Notes = "Some notes"
        };
        var result = _validator.Validate(lead);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void InvalidLead_FailsValidation()
    {
        var lead = new Lead { Email = "", FirstName = new string('a', 101) };
        var result = _validator.Validate(lead);
        Assert.False(result.IsValid);
    }
}
