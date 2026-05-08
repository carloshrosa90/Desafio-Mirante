using Desafio.Aplicacao.Comum;
using Desafio.Entities;

namespace Desafio.Aplicacao.Interface
{
	public interface ITarefa
	{
		Task<ResultadoServico<IEnumerable<Tarefa>>> ObterTodos(CancellationToken cancellationToken);
		Task<ResultadoServico<IEnumerable<Tarefa>>> ObterPorFiltro(int? status, DateTime? dataVencimento, CancellationToken cancellationToken);
		Task<ResultadoServico<Tarefa>> Incluir(Tarefa tarefa, CancellationToken cancellationToken);
	}
}
