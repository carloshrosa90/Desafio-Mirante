using Desafio.Aplicacao.Dtos;
using Desafio.Aplicacao.Interface;
using Desafio.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Desafio.Apresentacao.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TarefaController : ControllerBase
{
	private readonly ITarefa _tarefaService;

	public TarefaController(ITarefa tarefaService)
	{
		_tarefaService = tarefaService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Tarefa>>> ObterTodos(CancellationToken cancellationToken)
	{
		var resultado = await _tarefaService.ObterTodos(cancellationToken);
		if (!resultado.Sucesso)
			return BadRequest(resultado.Mensagem);
		return Ok(resultado.Dados);
	}

	[HttpGet("filtro")]
	public async Task<ActionResult<IEnumerable<Tarefa>>> ObterPorFiltro(
		[FromQuery] int? status,
		[FromQuery]
		[SwaggerParameter(Description = "Formato da data: yyyy-MM-dd (ex.: 2026-05-08).")]
		DateTime? dataVencimento,
		CancellationToken cancellationToken)
	{
		var resultado = await _tarefaService.ObterPorFiltro(status, dataVencimento, cancellationToken);
		if (!resultado.Sucesso)
			return BadRequest(resultado.Mensagem);
		return Ok(resultado.Dados);
	}

	[HttpPost]
	public async Task<ActionResult<Tarefa>> Incluir([FromBody] Tarefa tarefa, CancellationToken cancellationToken)
	{
		var resultado = await _tarefaService.Incluir(tarefa, cancellationToken);
		if (!resultado.Sucesso || resultado.Dados is null)
			return BadRequest(resultado.Mensagem);
		var criada = resultado.Dados;
		return Created($"/api/tarefa/{criada.int_id}", criada);
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Alterar(int id, [FromBody] TarefaAtualizacaoDto dados, CancellationToken cancellationToken)
	{
		var resultado = await _tarefaService.Alterar(id, dados, cancellationToken);
		if (!resultado.Sucesso)
		{
			if (resultado.Mensagem == "Tarefa nao encontrada.")
				return NotFound(resultado.Mensagem);
			return BadRequest(resultado.Mensagem);
		}
		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Excluir(int id, CancellationToken cancellationToken)
	{
		var resultado = await _tarefaService.Excluir(id, cancellationToken);
		if (!resultado.Sucesso)
		{
			if (resultado.Mensagem == "Tarefa nao encontrada.")
				return NotFound(resultado.Mensagem);
			return BadRequest(resultado.Mensagem);
		}
		return NoContent();
	}
}
