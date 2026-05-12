using Desafio.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Infrastructure.Persistence
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
