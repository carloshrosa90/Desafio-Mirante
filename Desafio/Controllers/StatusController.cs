using Desafio.Aplicacao.Interface;
using Desafio.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Desafio.Apresentacao.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
	private readonly IStatus _statusService;

	public StatusController(IStatus statusService)
	{
		_statusService = statusService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Status>>> ObterTodos(CancellationToken cancellationToken)
	{
		var resultado = await _statusService.ObterTodos(cancellationToken);
		if (!resultado.Sucesso)
			return BadRequest(resultado.Mensagem);
		return Ok(resultado.Dados);
	}
}
