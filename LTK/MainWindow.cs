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

	//private int [,] jogoResultado = null;


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

		for (var contador = 1; contador <= 5000; contador++) {
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
		if (Math.Sign(jogoQuantidade) == -1)
		{
			jogoQuantidade = Math.Abs(jogoQuantidade);
		}
		if (jogoQuantidade == 0)
		{
			jogoQuantidade = 1;
		}

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
		// Vamos criar um arranjo, que guarda todas as bolas.
		// Não precisamos validar as variáveis pois são foram realizadas.
		// No arranjo bolas sorteadas, os valores são:
		// 0 -> zero, se a bola ainda não foi sorteada.
		// 1 -> um, se a bola foi sorteada.
		// 2 -> dois, se a bola é do último concurso.
		int[,] bolas_Sorteadas = new int[jogoQuantidade, 100];

		int indiceJogo = 0;

		// Guarda as distâncias possíveis que o sucessor do número
		// anterior pode se distanciar.
		List<int> distanciadores = new List<int> ();

		// Inicializa o gerador de números aleatórios, com uma semente.
		Random geradorAleatorio = new Random((new Random()).Next(1000000));

		// Números do último jogo:
		List<int> bolasUltimoJogo = new List<int>(){
			1, 8, 17, 19, 29,
			31, 37, 40, 42, 43,
			45, 47, 50, 53, 62,
			66, 70, 80, 94, 96};
		
		while (indiceJogo < jogoQuantidade) {
			// Sempre zera o distanciador, quando começa um novo jogo.
			distanciadores.Clear ();

			// Vamos indicar quais bolas não podem sair ao jogar numero
			// aleatoriamente.
			for (var uA = 0; uA <= 99; uA++)
			{
				bolas_Sorteadas[indiceJogo, uA] = 0;
			}


			foreach (var bolaUltimoConcurso in bolasUltimoJogo)
			{
				bolas_Sorteadas[indiceJogo, bolaUltimoConcurso] = 2;
			}

			// Vamos adicionar o número na lista, indiceNaoSorteado,
			// Então, na lista, cada valor corresponde ao índice no arranjo
			// bolas_Sorteadas.
			List<int> indiceNaoSorteado = new List<int> ();
			for (var uA = 0; uA <= 99; uA++) {
				if (bolas_Sorteadas [indiceJogo, uA] == 0) {
					indiceNaoSorteado.Add (uA);
				}
			}

			// Pega o primeiro número.
			int indiceAnterior = geradorAleatorio.Next (0, 5);
			int qtBolasSorteadas = 1;
			bolas_Sorteadas [indiceJogo, indiceNaoSorteado [indiceAnterior]] = 1;

			System.Diagnostics.Debug.Print("Número Inicial: " + indiceAnterior + "\r\n");

			while (qtBolasSorteadas != jogoAposta) {
				if (distanciadores.Count == 0) {
					distanciadores.AddRange (new int[]{ 1, 2, 3, 4, 5, 6, 7  });
				}

				// Sortea o índice, em seguida, pega o valor que está armazenado neste índice
				// e adiciona este índice ao valor do índice anterior.
				//int indiceSorteado = geradorAleatorio.Next (0, distanciadores.Count);
				int indiceDistanciador = geradorAleatorio.Next (0, distanciadores.Count);

				string strTexto = "";
				strTexto += "qtBolasSorteadas: " + qtBolasSorteadas + "\r\n";
				strTexto += "Indice Distanciador sorteado: " + indiceDistanciador.ToString () + "\r\n";
				strTexto += "Valor no distanciador sorteado: " + distanciadores[indiceDistanciador] + "\r\n";
				strTexto += "Indice Anterior: " + indiceAnterior + "\r\n";

				int indiceProximo = distanciadores [indiceDistanciador] + indiceAnterior;
				distanciadores.RemoveAt (indiceDistanciador);

				strTexto += "Indice Próximo: " + indiceProximo.ToString () + "\r\n";
			
				if (indiceProximo > indiceNaoSorteado.Count-1) {
					indiceAnterior = 0;
					distanciadores.Clear ();
					indiceNaoSorteado.Clear ();
					for (var uA = 0; uA <= 99; uA++) {
						if (bolas_Sorteadas [indiceJogo, uA] == 0) {
							indiceNaoSorteado.Add (uA);
						}
					}
				} else {
					// O arranjo 'indiceNaoSorteado' armazena os valores que são 
					// os índices do arranjo 'bolas_Sorteadas[,]', tais valores
					// correspondentes a valor do tipo bool com o valor falso.
					int numeroSorteado = indiceNaoSorteado [indiceProximo];
					strTexto += "Numero sorteado: " + numeroSorteado;
					strTexto += "\r\n=============================================\r\n";

					bolas_Sorteadas [indiceJogo, numeroSorteado] = 1;
					qtBolasSorteadas++;
					indiceAnterior = indiceProximo;
				}

				System.Diagnostics.Debug.Print (strTexto);
			}
			indiceJogo++;
		}

		// Agora, vamos criar o modelo pra o treeview.
		System.Type[] gTipos = new System.Type[jogoAposta + 2];

		for (var uA = 0; uA < gTipos.Length; uA++) {
			gTipos [uA] = typeof(string);
		}
		TreeStore treeStore = new TreeStore (gTipos);

		DateTime dataHoraAgora = DateTime.Now;
		string arquivoNome = "lotomania_" + dataHoraAgora.ToString("yyyy_MM_dd_HHmmss") + ".txt";



		System.IO.StreamWriter objArquivo = new System.IO.StreamWriter(arquivoNome);



		StringBuilder strLinhas = new StringBuilder();

		// Vamos colocar um cabeçalho.
		strLinhas.Append("#CONCURSO;JOGO_TIPO");
		for (var indiceBola = 1; indiceBola <= 50; indiceBola++)
		{
			strLinhas.Append(";");
			strLinhas.Append("B");
			strLinhas.Append(indiceBola);
		}
		// Indica final de linha.
		strLinhas.AppendLine();

		int contadorRegistros = 0;
		for (var uA = 0; uA < jogoQuantidade; uA += 2) {
			string[] bolasSelecionadas = new string[jogoAposta + 2];
			string[] bolasNaoSelecionadas = new string[jogoAposta + 2];
			bolasSelecionadas[0] = contadorRegistros.ToString();
			bolasNaoSelecionadas[0] = (++contadorRegistros).ToString() + " [" + bolasSelecionadas[0] + "]";
			bolasSelecionadas[1] = "LOTOMANIA";
			bolasNaoSelecionadas[1] = "LOTOMANIA";

			contadorRegistros++;

			int indiceBolaSelecionada = 2;
			int indiceBolaNaoSelecionada = 2;

			for (var uB = 0; uB <= 99; uB++) {
				if (bolas_Sorteadas [uA, uB] == 1) {
					bolasSelecionadas[indiceBolaSelecionada] = uB.ToString();
					indiceBolaSelecionada++;
				} else {
					bolasNaoSelecionadas[indiceBolaNaoSelecionada] = uB.ToString();
					indiceBolaNaoSelecionada++;
				}
			}

			for (var uC = 0; uC < jogoAposta + 2; uC++)
			{
				// Cada campo será separado, por um ';', entretanto, não iremos
				// colocar está informação no final senão vai ficar um caractere ';'
				// inútil, então quando no loop o índice é diferente de '0', quer dizer, que
				// já tem uma campo então devemos inserir o caractere ';'. 
				if (uC != 0)
				{
					strLinhas.Append(";");
				}

				strLinhas.Append(bolasSelecionadas[uC]);
			}
			// Insere uma nova linha.
			strLinhas.AppendLine();


			for (var uC = 0; uC < jogoAposta + 2; uC++)
			{
				// Cada campo será separado, por um ';', entretanto, não iremos
				// colocar está informação no final senão vai ficar um caractere ';'
				// inútil, então quando no loop o índice é diferente de '0', quer dizer, que
				// já tem uma campo então devemos inserir o caractere ';'. 
				if (uC != 0)
				{
					strLinhas.Append(";");
				}

				strLinhas.Append(bolasNaoSelecionadas[uC]);
			}
			// Insere uma nova linha.
			strLinhas.AppendLine();


			treeStore.AppendValues (bolasSelecionadas);
			treeStore.AppendValues (bolasNaoSelecionadas);
		}

		// Grava os dados.
		objArquivo.Write(strLinhas.ToString());
		objArquivo.Flush();
		objArquivo.Close();
		objArquivo = null;

		// Anexar o modelo.
		tvResultado.Model = treeStore;

		tvResultado.EnableTreeLines = true;


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
			tvResultado.AppendColumn (new TreeViewColumn ("B" + (indice -1).ToString (), 
				new CellRendererText(), "text", indice			
			));
		}
		tvResultado.ColumnsAutosize ();




	}

	protected void OnCheckbutton1Toggled(object sender, EventArgs e)
	{
	}

	protected void OnChkVinteUltimosNumerosToggled(object sender, EventArgs e)
	{
		if (chkVinteUltimosNumeros.State == StateType.Selected)
		{
			tbLotomania.Visible = true;
		}
		else
		{
			tbLotomania.Visible = false;
		}
	}
}
