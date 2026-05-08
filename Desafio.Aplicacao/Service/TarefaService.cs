using Desafio.Aplicacao.Comum;
using Desafio.Aplicacao.Interface;
using Desafio.Entities;
using Desafio.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Aplicacao.Service
{
	public class TarefaService : ITarefa
	{
		private readonly IUnitOfWork _unidadeTrabalho;

		public TarefaService(IUnitOfWork unidadeTrabalho)
		{
			_unidadeTrabalho = unidadeTrabalho;
		}

		public async Task<ResultadoServico<IEnumerable<Tarefa>>> ObterTodos(CancellationToken cancellationToken)
		{
			try
			{
				var lista = await _unidadeTrabalho.TarefaLista.AsNoTracking().ToListAsync(cancellationToken);
				return ResultadoServico<IEnumerable<Tarefa>>.ComSucesso(lista);
			}
			catch (Exception ex)
			{
				return ResultadoServico<IEnumerable<Tarefa>>.ComErro($"Erro ao listar tarefas: {ex.Message}");
			}
		}

		public async Task<ResultadoServico<IEnumerable<Tarefa>>> ObterPorFiltro(int? status, DateTime? dataVencimento, CancellationToken cancellationToken)
		{
			try
			{
				var consulta = _unidadeTrabalho.TarefaLista.AsNoTracking().AsQueryable();

				if (status.HasValue)
					consulta = consulta.Where(t => t.int_status == status.Value);

				if (dataVencimento.HasValue)
					consulta = consulta.Where(t => t.dat_vencimento.Date == dataVencimento.Value.Date);

				var lista = await consulta.ToListAsync(cancellationToken);
				return ResultadoServico<IEnumerable<Tarefa>>.ComSucesso(lista);
			}
			catch (Exception ex)
			{
				return ResultadoServico<IEnumerable<Tarefa>>.ComErro($"Erro ao filtrar tarefas: {ex.Message}");
			}
		}

		public async Task<ResultadoServico<Tarefa>> Incluir(Tarefa tarefa, CancellationToken cancellationToken)
		{
			try
			{
				_unidadeTrabalho.AdicionarTarefa(tarefa);
				await _unidadeTrabalho.SalvarAlteracoesAsync(cancellationToken);
				return ResultadoServico<Tarefa>.ComSucesso(tarefa);
			}
			catch (Exception ex)
			{
				return ResultadoServico<Tarefa>.ComErro($"Erro ao incluir tarefa: {ex.Message}");
			}
		}
	}
}
