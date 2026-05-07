using System.ComponentModel.DataAnnotations;

namespace Desafio.Dominio.Entidade
{
	public class Status
	{
		[Key]
		public int int_id { get; set; }
		public string? str_nome { get; set; }
		public bool sta_ativo { get; set; }
	}
}
