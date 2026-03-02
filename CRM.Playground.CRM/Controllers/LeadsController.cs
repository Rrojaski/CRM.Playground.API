using CRM.Playground.CRM.Domain.Entities;
using CRM.Playground.CRM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Playground.CRM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    private readonly CrmDbContext _db;

    public LeadsController(CrmDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lead>>> GetAll()
    {
        return Ok(await _db.Leads.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Lead>> GetById(Guid id)
    {
        var lead = await _db.Leads.FindAsync(id);
        return lead == null ? NotFound() : Ok(lead);
    }

    [HttpPost]
    public async Task<ActionResult<Lead>> Create(Lead lead)
    {
        lead.Id = Guid.NewGuid();
        lead.CreatedAt = DateTime.UtcNow;
        _db.Leads.Add(lead);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = lead.Id }, lead);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Lead lead)
    {
        if (id != lead.Id) return BadRequest();
        var exists = await _db.Leads.AnyAsync(x => x.Id == id);
        if (!exists) return NotFound();
        _db.Entry(lead).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var lead = await _db.Leads.FindAsync(id);
        if (lead == null) return NotFound();
        _db.Leads.Remove(lead);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
