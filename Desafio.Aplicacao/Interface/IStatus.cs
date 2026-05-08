using Desafio.Aplicacao.Comum;
using Desafio.Entities;

namespace Desafio.Aplicacao.Interface
{
	public interface IStatus
	{
		Task<ResultadoServico<IEnumerable<Status>>> ObterTodos(CancellationToken cancellationToken);
	}
}
