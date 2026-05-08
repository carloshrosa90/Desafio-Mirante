using Desafio.Aplicacao.Comum;
using Desafio.Aplicacao.Dtos;
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
				await _unidadeTrabalho.SalvarTarefa(cancellationToken);
				return ResultadoServico<Tarefa>.ComSucesso(tarefa);
			}
			catch (Exception ex)
			{
				return ResultadoServico<Tarefa>.ComErro($"Erro ao incluir tarefa: {ex.Message}");
			}
		}

		public async Task<ResultadoServico<Tarefa>> Alterar(int id, TarefaAtualizacaoDto dados, CancellationToken cancellationToken)
		{
			try
			{
				var existente = await _unidadeTrabalho.ObterTarefaPorId(id, cancellationToken);
				if (existente is null)
					return ResultadoServico<Tarefa>.ComErro("Tarefa nao encontrada.");

				existente.str_titulo = dados.str_titulo;
				existente.str_descricao = dados.str_descricao;
				existente.int_status = dados.int_status;
				existente.dat_vencimento = dados.dat_vencimento;

				await _unidadeTrabalho.SalvarTarefa(cancellationToken);
				return ResultadoServico<Tarefa>.ComSucesso(existente);
			}
			catch (Exception ex)
			{
				return ResultadoServico<Tarefa>.ComErro($"Erro ao alterar tarefa: {ex.Message}");
			}
		}

		public async Task<ResultadoServico<bool>> Excluir(int id, CancellationToken cancellationToken)
		{
			try
			{
				var existente = await _unidadeTrabalho.ObterTarefaPorId(id, cancellationToken);
				if (existente is null)
					return ResultadoServico<bool>.ComErro("Tarefa nao encontrada.");

				_unidadeTrabalho.RemoverTarefa(existente);
				await _unidadeTrabalho.SalvarTarefa(cancellationToken);
				return ResultadoServico<bool>.ComSucesso(true);
			}
			catch (Exception ex)
			{
				return ResultadoServico<bool>.ComErro($"Erro ao excluir tarefa: {ex.Message}");
			}
		}
	}
}
