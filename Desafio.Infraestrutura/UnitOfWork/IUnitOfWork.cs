using Desafio.Entities;

namespace Desafio.Infrastructure.UnitOfWork
{
	public interface IUnitOfWork
	{
		IQueryable<Status> StatusLista { get; }
		IQueryable<Tarefa> TarefaLista { get; }
		void AdicionarTarefa(Tarefa tarefa);
		Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken);
	}
}
