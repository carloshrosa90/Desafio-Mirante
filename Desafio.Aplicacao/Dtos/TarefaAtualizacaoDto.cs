namespace Desafio.Aplicacao.Dtos
{
	public class TarefaAtualizacaoDto
	{
		public string? str_titulo { get; set; }
		public string? str_descricao { get; set; }
		public int int_status { get; set; }
		public DateTime dat_vencimento { get; set; }
	}
}
