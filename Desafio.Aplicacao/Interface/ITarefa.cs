using Desafio.Aplicacao.Comum;
using Desafio.Aplicacao.Dtos;
using Desafio.Entities;

namespace Desafio.Aplicacao.Interface
{
	public interface ITarefa
	{
		Task<ResultadoServico<IEnumerable<Tarefa>>> ObterTodos();
		Task<ResultadoServico<IEnumerable<Tarefa>>> ObterPorFiltro(int? status, DateTime? dataVencimento);
		Task<ResultadoServico<Tarefa>> Incluir(Tarefa tarefa);
		Task<ResultadoServico<Tarefa>> Alterar(int id, TarefaAtualizacaoDto dados);
		Task<ResultadoServico<bool>> Excluir(int id);
	}
}
