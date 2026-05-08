using Desafio.Entities;
using Desafio.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Apresentacao.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TarefaController : ControllerBase
{
	private readonly AppDbContext _context;

	public TarefaController(AppDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Tarefa>>> ObterTodos(CancellationToken cancellationToken)
	{
		return await _context.Tarefas.AsNoTracking().ToListAsync(cancellationToken);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<Tarefa>> ObterPorId(int id, CancellationToken cancellationToken)
	{
		var item = await _context.Tarefas.AsNoTracking().FirstOrDefaultAsync(t => t.int_id == id, cancellationToken);
		if (item is null)
			return NotFound();

		return item;
	}

	[HttpPost]
	public async Task<ActionResult<Tarefa>> Criar([FromBody] Tarefa tarefa, CancellationToken cancellationToken)
	{
		_context.Tarefas.Add(tarefa);
		await _context.SaveChangesAsync(cancellationToken);
		return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.int_id }, tarefa);
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Atualizar(int id, [FromBody] Tarefa tarefa, CancellationToken cancellationToken)
	{
		if (id != tarefa.int_id)
			return BadRequest();

		var existente = await _context.Tarefas.FindAsync(new object[] { id }, cancellationToken);
		if (existente is null)
			return NotFound();

		existente.str_titulo = tarefa.str_titulo;
		existente.str_descricao = tarefa.str_descricao;
		existente.int_status = tarefa.int_status;
		existente.dat_vencimento = tarefa.dat_vencimento;

		await _context.SaveChangesAsync(cancellationToken);
		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Excluir(int id, CancellationToken cancellationToken)
	{
		var existente = await _context.Tarefas.FindAsync(new object[] { id }, cancellationToken);
		if (existente is null)
			return NotFound();

		_context.Tarefas.Remove(existente);
		await _context.SaveChangesAsync(cancellationToken);
		return NoContent();
	}
}
