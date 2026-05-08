using Desafio.Entities;

namespace Desafio.Infrastructure.UnitOfWork
{
	public interface IUnitOfWork
	{
		IQueryable<Status> StatusLista { get; }
		IQueryable<Tarefa> TarefaLista { get; }
		void AdicionarTarefa(Tarefa tarefa);
		Task<Tarefa?> ObterTarefaPorId(int id, CancellationToken cancellationToken);
		void RemoverTarefa(Tarefa tarefa);
		Task<int> SalvarTarefa(CancellationToken cancellationToken);
	}
}
