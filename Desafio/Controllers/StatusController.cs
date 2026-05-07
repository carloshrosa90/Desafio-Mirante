using Desafio.Entities;
using Desafio.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
	private readonly AppDbContext _context;

	public StatusController(AppDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Status>>> ObterTodos(CancellationToken cancellationToken)
	{
		return await _context.Status.AsNoTracking().ToListAsync(cancellationToken);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<Status>> ObterPorId(int id, CancellationToken cancellationToken)
	{
		var item = await _context.Status.AsNoTracking().FirstOrDefaultAsync(s => s.int_id == id, cancellationToken);
		if (item is null)
			return NotFound();

		return item;
	}

	[HttpPost]
	public async Task<ActionResult<Status>> Criar([FromBody] Status status, CancellationToken cancellationToken)
	{
		_context.Status.Add(status);
		await _context.SaveChangesAsync(cancellationToken);
		return CreatedAtAction(nameof(ObterPorId), new { id = status.int_id }, status);
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Atualizar(int id, [FromBody] Status status, CancellationToken cancellationToken)
	{
		if (id != status.int_id)
			return BadRequest();

		var existente = await _context.Status.FindAsync(new object[] { id }, cancellationToken);
		if (existente is null)
			return NotFound();

		existente.str_nome = status.str_nome;
		existente.sta_ativo = status.sta_ativo;

		await _context.SaveChangesAsync(cancellationToken);
		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Excluir(int id, CancellationToken cancellationToken)
	{
		var existente = await _context.Status.FindAsync(new object[] { id }, cancellationToken);
		if (existente is null)
			return NotFound();

		_context.Status.Remove(existente);
		await _context.SaveChangesAsync(cancellationToken);
		return NoContent();
	}
}
