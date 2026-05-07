using System.ComponentModel.DataAnnotations;

namespace Desafio.Dominio.Entidade
{
	public class Tarefa
	{
		[Key]
		public int int_id { get; set; }
		public string? str_titulo { get; set; }
		public bool str_descricao { get; set; }
		public int int_status { get; set; }
		public DateTime dat_vencimento { get; set; }
	}
}
