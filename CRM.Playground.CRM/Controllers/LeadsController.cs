using CRM.Playground.CRM.Domain.Entities;
using CRM.Playground.CRM.Application.Commands;
using CRM.Playground.CRM.Application.Queries;
using CRM.Playground.CRM.Application.Validators;
using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Playground.CRM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly LeadValidator _validator;

    public LeadsController(IMediator mediator)
    {
        _mediator = mediator;
        _validator = new LeadValidator();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lead>>> GetAll([
        FromQuery] Guid? tenantId,
        [FromQuery] string? stage,
        [FromQuery] Guid? assignedTo,
        [FromQuery] DateTime? createdFrom,
        [FromQuery] DateTime? createdTo)
    {
        var leads = await _mediator.Send(new GetAllLeadsQuery
        {
            TenantId = tenantId,
            Stage = stage,
            AssignedTo = assignedTo,
            CreatedFrom = createdFrom,
            CreatedTo = createdTo
        });
        return Ok(leads);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Lead>> GetById(Guid id)
    {
        var lead = await _mediator.Send(new GetLeadByIdQuery { Id = id });
        return lead == null ? NotFound() : Ok(lead);
    }

    [HttpPost]
    public async Task<ActionResult<Lead>> Create([FromBody] CreateLeadCommand command)
    {
        var lead = new Lead
        {
            TenantId = command.TenantId,
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Phone = command.Phone,
            CompanyName = command.CompanyName,
            Source = command.Source,
            Stage = command.Stage,
            AssignedTo = command.AssignedTo,
            Score = command.Score,
            ExpectedValue = command.ExpectedValue,
            Notes = command.Notes,
            LastContactedAt = command.LastContactedAt
        };
        var validationResult = _validator.Validate(lead);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLeadCommand command)
    {
        if (id != command.Id) return BadRequest();
        var lead = new Lead
        {
            Id = command.Id,
            TenantId = command.TenantId,
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Phone = command.Phone,
            CompanyName = command.CompanyName,
            Source = command.Source,
            Stage = command.Stage,
            AssignedTo = command.AssignedTo,
            Score = command.Score,
            ExpectedValue = command.ExpectedValue,
            Notes = command.Notes,
            LastContactedAt = command.LastContactedAt
        };
        var validationResult = _validator.Validate(lead);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteLeadCommand { Id = id });
        if (!result) return NotFound();
        return NoContent();
    }
}
