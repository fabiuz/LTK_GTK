using System;
namespace LTK
{
	/// <summary>
	/// Classe abstrata para manipular jogos lotéricos.
	/// </summary>
	public abstract class JogoLoterico
	{
		protected string Nome {get; }
		protected int bolaInicial { get; }
		protected int bolaFinal { get; }
		protected int bolasPorSorteio { get; }
		protected int[] bolasPorAposta { get; }
		private int[] todas_as_bolas { get; set; }

		/// <summary>
		/// Construtor para inicializar os dados.
		/// 
		/// </summary>
		/// <param name="nome">Nome do jogo.</param>
		/// <param name="bolaInicial">A menor bola do jogo.</param>
		/// <param name="bolaFinal">A última bola do jogo.</param>
		/// <param name="bolasPorSorteio">Indica a quantidade de bolas que é sorteada em um concurso.</param>
		/// <param name="bolasPorAposta">
		/// 	Um arranjo com a quantidade de bolas que pode ser apostadas.
		/// 	Por exemplo, na lotofacil, pode apostar com 15, 16, 17 ou 18 números.
		/// </param>
		public JogoLoterico(string nome, int bolaInicial, int bolaFinal, int bolasPorSorteio, int[] bolasPorAposta)
		{
			this.Nome = nome;
			this.bolaInicial = bolaInicial;
			this.bolaFinal = bolaFinal;
			this.bolasPorSorteio = bolasPorSorteio;
			this.bolasPorAposta = bolasPorAposta;

			// Vamos verificar os dados.
			if (this.bolaInicial > this.bolaFinal)
			{
				throw new System.ArgumentException("bolaInicial deve ser menor que bolaFinal");
			}

			if (bolasPorSorteio > (bolaFinal - bolaInicial))
			{
				throw new System.ArgumentException("Bolas por sorteio maior que a quantidade de bolas totais");
			}

			// Carrega números.
			int quantidade_de_Bolas = bolaInicial - bolaFinal + 1;
			todas_as_bolas = new int[quantidade_de_Bolas];

			var numeroInicial = bolaInicial;
			for (var iA = 0; iA < quantidade_de_Bolas; iA++)
			{
				todas_as_bolas[iA] = numeroInicial;
				numeroInicial++;
			}
		}
	}
}
