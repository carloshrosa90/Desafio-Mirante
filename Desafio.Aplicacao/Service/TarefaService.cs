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

		public async Task<ResultadoServico<IEnumerable<Tarefa>>> ObterTodos()
		{
			try
			{
				var lista = await _unidadeTrabalho.TarefaLista.AsNoTracking().ToListAsync();
				return ResultadoServico<IEnumerable<Tarefa>>.ComSucesso(lista);
			}
			catch (Exception ex)
			{
				return ResultadoServico<IEnumerable<Tarefa>>.ComErro($"Erro ao listar tarefas: {ex.Message}");
			}
		}

		public async Task<ResultadoServico<IEnumerable<Tarefa>>> ObterPorFiltro(int? status, DateTime? dataVencimento)
		{
			try
			{
				var consulta = _unidadeTrabalho.TarefaLista.AsNoTracking().AsQueryable();

				if (status.HasValue)
					consulta = consulta.Where(t => t.int_status == status.Value);

				if (dataVencimento.HasValue)
					consulta = consulta.Where(t => t.dat_vencimento.Date == dataVencimento.Value.Date);

				var lista = await consulta.ToListAsync();
				return ResultadoServico<IEnumerable<Tarefa>>.ComSucesso(lista);
			}
			catch (Exception ex)
			{
				return ResultadoServico<IEnumerable<Tarefa>>.ComErro($"Erro ao filtrar tarefas: {ex.Message}");
			}
		}

		public async Task<ResultadoServico<Tarefa>> Incluir(Tarefa tarefa)
		{
			try
			{
				var statusExiste = await _unidadeTrabalho.StatusLista
					.AsNoTracking()
					.AnyAsync(s => s.int_id == tarefa.int_status);
				if (!statusExiste)
					return ResultadoServico<Tarefa>.ComErro("Status não encontrado. Informe um status existente.");

				_unidadeTrabalho.AdicionarTarefa(tarefa);
				await _unidadeTrabalho.SalvarTarefa();
				return ResultadoServico<Tarefa>.ComSucesso(tarefa);
			}
			catch (Exception ex)
			{
				return ResultadoServico<Tarefa>.ComErro($"Erro ao incluir tarefa: {ex.Message}");
			}
		}

		public async Task<ResultadoServico<Tarefa>> Alterar(int id, TarefaAtualizacaoDto dados)
		{
			try
			{
				var existente = await _unidadeTrabalho.ObterTarefaPorId(id);
				if (existente is null)
					return ResultadoServico<Tarefa>.ComErro("Tarefa não encontrada. Informe uma tarefa existente.");

				var statusExiste = await _unidadeTrabalho.StatusLista
					.AsNoTracking()
					.AnyAsync(s => s.int_id == dados.int_status);
				if (!statusExiste)
					return ResultadoServico<Tarefa>.ComErro("Status não encontrada. Informe um status existente.");

				existente.str_titulo = dados.str_titulo;
				existente.str_descricao = dados.str_descricao;
				existente.int_status = dados.int_status;
				existente.dat_vencimento = dados.dat_vencimento;

				await _unidadeTrabalho.SalvarTarefa();
				return ResultadoServico<Tarefa>.ComSucesso(existente);
			}
			catch (Exception ex)
			{
				return ResultadoServico<Tarefa>.ComErro($"Erro ao alterar tarefa: {ex.Message}");
			}
		}

		public async Task<ResultadoServico<bool>> Excluir(int id)
		{
			try
			{
				var existente = await _unidadeTrabalho.ObterTarefaPorId(id);
				if (existente is null)
					return ResultadoServico<bool>.ComErro("Tarefa não encontrada. Informe uma tarefa existente.");

				_unidadeTrabalho.RemoverTarefa(existente);
				await _unidadeTrabalho.SalvarTarefa();
				return ResultadoServico<bool>.ComSucesso(true);
			}
			catch (Exception ex)
			{
				return ResultadoServico<bool>.ComErro($"Erro ao excluir tarefa: {ex.Message}");
			}
		}
	}
}
