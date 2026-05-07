using Desafio.Dominio.Entidade;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Infraestrutura.Context
{
	public class AppDbContext : DbContext
	{
		public DbSet<Status> Status { get; set; }
		public DbSet<Tarefa> Tarefas { get; set; }

		public AppDbContext(DbContextOptions options) : base(options)
		{


		}

	}
}
