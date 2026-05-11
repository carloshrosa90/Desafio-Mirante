using Desafio.Entities;
using Desafio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Infrastructure.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _contexto;

		public UnitOfWork(AppDbContext contexto)
		{
			_contexto = contexto;
		}

		public IQueryable<Status> StatusLista => _contexto.Status.AsQueryable();
		public IQueryable<Tarefa> TarefaLista => _contexto.Tarefas.AsQueryable();

		public void AdicionarTarefa(Tarefa tarefa)
		{
			_contexto.Tarefas.Add(tarefa);
		}

		public async Task<Tarefa?> ObterTarefaPorId(int id)
		{
			return await _contexto.Tarefas.FindAsync(id);
		}

		public void RemoverTarefa(Tarefa tarefa)
		{
			_contexto.Tarefas.Remove(tarefa);
		}

		public async Task<int> SalvarTarefa()
		{
			return await _contexto.SaveChangesAsync();
		}
	}
}
