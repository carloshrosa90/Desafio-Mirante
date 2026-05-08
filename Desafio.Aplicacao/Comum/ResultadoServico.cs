namespace Desafio.Aplicacao.Comum
{
	public sealed class ResultadoServico<T>
	{
		public bool Sucesso { get; init; }
		public string Mensagem { get; init; } = string.Empty;
		public T? Dados { get; init; }

		public static ResultadoServico<T> ComSucesso(T dados) =>
			new() { Sucesso = true, Mensagem = string.Empty, Dados = dados };

		public static ResultadoServico<T> ComErro(string mensagem) =>
			new() { Sucesso = false, Mensagem = mensagem, Dados = default };
	}
}
