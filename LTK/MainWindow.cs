using System;
using Gtk;
using LTK;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;




public partial class MainWindow : Gtk.Window
{
	///private JogoLoterico[] lista_de_jogos;

	private Dictionary<string, List<string>> lista_de_jogos = null;

	private int [,] jogoResultado = null;


	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();

		InicializarDadosdoJogo ();
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnDefaultActivated(object sender, EventArgs e)
	{
		System.Diagnostics.Debug.Print("Teste");
	}

	protected void OnCmbJogoChanged (object sender, EventArgs e)
	{
		string jogoSelecionado = ((ComboBox)sender).ActiveText;

		cmbJogoAposta.Model = new ListStore (typeof(string));

		// Preencher o segundo combobox conforme o jogo selecionado.
		foreach (var jogoApostas in lista_de_jogos[jogoSelecionado].ToArray()) {
			cmbJogoAposta.AppendText (jogoApostas);
			System.Diagnostics.Debug.Print (jogoApostas);
		}
		cmbJogoAposta.Active = 0;
		//cmbJogoAposta.Redra

	}

	private void InicializarDadosdoJogo(){
		lista_de_jogos = new Dictionary<string, List<string>> ();

		lista_de_jogos.Add ("LOTOFACIL", new List<string> (){ "15", "16", "17", "18" });
		lista_de_jogos.Add ("LOTOMANIA", new List<string> (){ "50" });
		lista_de_jogos.Add ("QUINA", 
			new List<string> (){ "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" });
		lista_de_jogos.Add ("MEGASENA", 
			new List<string> (){"6", "7", "8", "9", "10", "11", "12", "13", "14", "15" });		
		lista_de_jogos.Add ("DUPLASENA", 
			new List<string> (){"6", "7", "8", "9", "10", "11", "12", "13", "14", "15" });
		
		var indice = 0;
		foreach(var strChave in lista_de_jogos.Keys){
			cmbJogo.InsertText(indice, strChave.ToString());
			indice++;
		}
		cmbJogo.Active = 0;

		for (var contador = 1; contador <= 100; contador++) {
			cmbJogoQuantidade.AppendText (contador.ToString ());
		}
		cmbJogoQuantidade.Active = 0;
	}

	protected void OnBtnGerarClicked (object sender, EventArgs e)
	{
		string jogoSelecionado = cmbJogo.ActiveText;

		// Vamos validar os dados.
		if (!lista_de_jogos.ContainsKey (jogoSelecionado))
			return;
	
		int jogoAposta = int.Parse (cmbJogoAposta.ActiveText);
		if (!lista_de_jogos [jogoSelecionado].Contains (jogoAposta.ToString ()))
			return;

		int jogoQuantidade = int.Parse (cmbJogoQuantidade.ActiveText);
		if (jogoQuantidade < 1 || jogoQuantidade > 100)
			jogoQuantidade = 1;	

		System.Diagnostics.Debug.Print ("asdf");

		if(jogoSelecionado == "LOTOMANIA"){
			GerarLotomania(jogoAposta, jogoQuantidade);
		}
	}


	/// <summary>
	/// Gerar aleatória vários números para a lotomania.
	/// </summary>
	/// <param name="jogoAposta">Jogo aposta.</param>
	/// <param name="jogoQuantidade">Jogo quantidade.</param>
	private void GerarLotomania(int jogoAposta, int jogoQuantidade){

		// Nós iremos armazenar as bolas geradas em um treeview.
		//jogoAposta = 3;
		System.Type[] gTipos = new System.Type[jogoAposta + 2];

		// As colunas que refere-se à bola.
		for (var indice = 0; indice < gTipos.Length; indice++) {
			gTipos [indice] = typeof(string);
		}

		TreeStore treeStore = new TreeStore (gTipos);

		// Inicializa o gerador de números aleatórios;
		Random numeroAleatorio = new Random();

		// Armazenar se a bola já saiu ou não.
		bool[] todasBolas = new bool[100];

		// Dado o primeiro número sorteado aleatoriamente,
		// o próximo número será a soma do número anterior +
		// o valor do número sorteado aleatoriamente da lista.
		List<int> distanciadores = new List<int> ();

		// Indica a quantidade de jogos já localizados.
		int qtJogos = 0;

		while (qtJogos < jogoQuantidade) {
			// Zera os valores.
			todasBolas = new bool[100];
			distanciadores = new List<int> ();
			int[] bolasSorteadas = new int[jogoAposta];

			// Pega o valor inicial.
			int numeroAnterior = numeroAleatorio.Next(0, 3);

			// Indica a quantidade de bola já sorteada.
			int indiceBolaSorteada = 0;
			bolasSorteadas [indiceBolaSorteada] = numeroAnterior;

			// Aqui, iremos sortear, o número pegando-o de uma lista de distanciadores
			// e somaremos com o número anterior.
			// Se excedermos o limite do número, iremos pegar qualquer número não sorteado

			while (indiceBolaSorteada < jogoAposta) {
				// Primeiro, verifica se a lista está vazia, se sim, preenche-a novamente.
				if (distanciadores.Count == 0) {
					distanciadores.AddRange (new int[]{ 1, 2, 3 });
				}

				// Sortea o número da seguinte forma,
				// Pega o índice aleatoriamente, da lista de distanciadores, em seguida,
				// recuperar o valor que está na posição do índice sorteada, em seguida,
				// soma-se ao número anterior, já encontrado.
				// Em seguida, verifica se o número já foi sorteado, se foi retorna o processo.
				int indice = numeroAleatorio.Next (distanciadores.Count);
				int numeroTemporario = distanciadores [indice];
				numeroTemporario += numeroAnterior;

				// Agora, se o número excedeu 99, iremos preencher, os números restantes.
				if (numeroTemporario > 99) {

					while (indiceBolaSorteada < jogoAposta) {
						indice = numeroAleatorio.Next (0, 99);

						// Se bola sorteada continue do início do loop.
						if (todasBolas [indice]) {
							continue;
						}

						todasBolas [indice] = true;
						indiceBolaSorteada++;
					}
					break;
				}

				if (todasBolas [numeroTemporario]) {
					continue;
				}

				// Remove o número sorteado.
				distanciadores.RemoveAt (indice);
				todasBolas [numeroTemporario] = true;

				numeroAnterior = numeroTemporario;
				indiceBolaSorteada++;
				bolasSorteadas [indiceBolaSorteada] = numeroAnterior;
			}

			string[] strBolasSorteadas = new string[jogoAposta + 2];
			string[] strBolasNaoSorteadas = new string[jogoAposta + 2];

			// 
			indiceBolaSorteada = 2;
			int indiceBolaNaoSorteada = 2;

			// Vamos percorrer todas as bolas, e colocar cada número
			// na sua posição específica.
			for (var uA = 0; uA <= 99; uA++) {
				if (todasBolas [uA]) {
					strBolasSorteadas [indiceBolaSorteada] = uA.ToString ();
					indiceBolaSorteada++;
				} else {
					strBolasNaoSorteadas [indiceBolaNaoSorteada] = uA.ToString ();
					indiceBolaNaoSorteada++;
				}
			}

			// As colunas 0 e 1, corresponde, respectivamente a 
			// posição do jogo e o nome do jogo.
			strBolasSorteadas[0] = (qtJogos + 1).ToString();
			strBolasNaoSorteadas [0] = (qtJogos + 2).ToString () + "[" + strBolasSorteadas [0].ToString() + "]";

			strBolasSorteadas [1] = "LOTOMANIA";
			strBolasNaoSorteadas [1] = "LOTOMANIA";

			treeStore.AppendValues (strBolasSorteadas);
			treeStore.AppendValues (strBolasNaoSorteadas);
			qtJogos += 2;
		}

		tvResultado.EnableTreeLines = true;
		tvResultado.Model = treeStore;

		// Vamos apagar a coluna anterior.
		while (tvResultado.Columns.Length != 0) {
			tvResultado.RemoveColumn (tvResultado.Columns [0]);
		}

		// Vamos adicionar as colunas.
		tvResultado.AppendColumn(new TreeViewColumn("#Concurso", new CellRendererText(), "text", 0));
		tvResultado.AppendColumn (new TreeViewColumn ("JOGO_TIPO", new CellRendererText (), "text", 1));

		// Vamos adicionar as colunas das bolas.
		for (var indice = 2; indice < jogoAposta + 2; indice++) {			
			tvResultado.AppendColumn (new TreeViewColumn ("B" + (indice -1).ToString (), new CellRendererText(), "text", indice));
		}
		tvResultado.ColumnsAutosize ();

	}
}
