using Desafio.Aplicacao.Comum;
using Desafio.Aplicacao.Interface;
using Desafio.Entities;
using Desafio.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Aplicacao.Service
{
	public class StatusService : IStatus
	{
		private readonly IUnitOfWork _unidadeTrabalho;

		public StatusService(IUnitOfWork unidadeTrabalho)
		{
			_unidadeTrabalho = unidadeTrabalho;
		}

		public async Task<ResultadoServico<IEnumerable<Status>>> ObterTodos()
		{
			try
			{
				var lista = await _unidadeTrabalho.StatusLista.AsNoTracking().ToListAsync();
				return ResultadoServico<IEnumerable<Status>>.ComSucesso(lista);
			}
			catch (Exception ex)
			{
				return ResultadoServico<IEnumerable<Status>>.ComErro($"Erro ao listar status: {ex.Message}");
			}
		}
	}
}
