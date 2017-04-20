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
	private const int LIMITE_DE_JOGOS = 100;

	private Dictionary<string, List<string>> lista_de_jogos = null;

	//private int [,] jogoResultado = null;


	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();

		InicializarDadosdoJogo();
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

	protected void OnCmbJogoChanged(object sender, EventArgs e)
	{
		string jogoSelecionado = ((ComboBox)sender).ActiveText;

		cmbJogoAposta.Model = new ListStore(typeof(string));

		// Preencher o segundo combobox conforme o jogo selecionado.
		foreach (var jogoApostas in lista_de_jogos[jogoSelecionado].ToArray())
		{
			cmbJogoAposta.AppendText(jogoApostas);
			System.Diagnostics.Debug.Print(jogoApostas);
		}
		cmbJogoAposta.Active = 0;
		//cmbJogoAposta.Redra

	}

	private void InicializarDadosdoJogo()
	{
		lista_de_jogos = new Dictionary<string, List<string>>();

		lista_de_jogos.Add("LOTOFACIL", new List<string>() { "15", "16", "17", "18" });
		lista_de_jogos.Add("LOTOMANIA", new List<string>() { "50" });
		lista_de_jogos.Add("QUINA",
			new List<string>() { "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" });
		lista_de_jogos.Add("MEGASENA",
			new List<string>() { "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" });
		lista_de_jogos.Add("DUPLASENA",
			new List<string>() { "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" });

		var indice = 0;
		foreach (var strChave in lista_de_jogos.Keys)
		{
			cmbJogo.InsertText(indice, strChave.ToString());
			indice++;
		}
		cmbJogo.Active = 0;

		for (var contador = 1; contador <= LIMITE_DE_JOGOS; contador++)
		{
			cmbJogoQuantidade.AppendText(contador.ToString());
		}
		cmbJogoQuantidade.Active = 0;
	}

	protected void OnBtnGerarClicked(object sender, EventArgs e)
	{
		string jogoSelecionado = cmbJogo.ActiveText;

		// Vamos validar os dados.
		if (!lista_de_jogos.ContainsKey(jogoSelecionado))
			return;

		int jogoAposta = int.Parse(cmbJogoAposta.ActiveText);
		if (!lista_de_jogos[jogoSelecionado].Contains(jogoAposta.ToString()))
			return;

		int jogoQuantidade = int.Parse(cmbJogoQuantidade.ActiveText);
		if (Math.Sign(jogoQuantidade) == -1)
		{
			jogoQuantidade = Math.Abs(jogoQuantidade);
		}
		if (jogoQuantidade == 0)
		{
			jogoQuantidade = 1;
		}


		if (jogoSelecionado == "LOTOMANIA")
		{
			GerarLotomania(jogoAposta, jogoQuantidade);
		}
	}

	private void PreencherGrupo(int colunaInicial, int quantidadePorGrupo, int indiceJogoGerado, int[] jogoGerado, int[]bolas )
	{
		int limite = colunaInicial + quantidadePorGrupo;

		for (var uA = colunaInicial; uA < limite; uA++)
		{
			jogoGerado[indiceJogoGerado] = bolas[uA];
			indiceJogoGerado++;
		}
	}


	/// <summary>
	/// Gerars the lotomania80 numeros.
	/// </summary>
	/// <param name="jogoAposta">Jogo aposta.</param>
	/// <param name="jogoQuantidade">A quantidade de jogos.</param>
	private void GerarLotomania80Numeros(int jogoAposta, int jogoQuantidade, int grupo_de_Bolas)
	{
		if (grupo_de_Bolas != 5 && grupo_de_Bolas != 10)
		{
			throw new IndexOutOfRangeException("Argumento 'grupo_de_bolas' deve ter o valor 5 ou 10");
		}


		// Uma lista com todas as bolas.
		List<int> bolasLotomania = new List<int>();

		// Preencher lista.
		for (var uA = 0; uA <= 99; uA++)
		{
			bolasLotomania.Add(uA);
		}

		// Cria uma lista com todas as bolas do último concurso.
		List<int> bolasUltimoConcurso = new List<int>(){
			/*
			1, 8, 17, 19, 29,
			31, 37, 40, 42, 43,
			45, 47, 50, 53, 62,
			66, 70, 80, 94, 96};
			*/

			/*
		8, 12, 17, 19, 21,
		26, 33, 35, 42, 57,
		62, 66, 73, 83, 85,
		87, 90, 93, 98, 99
			*/

			/** 
			 * Lotomania concurso 1753 (13/4/2017)
			 * 
			*/
			3, 6, 10, 25, 26,
			33, 40, 43, 45, 49,
			51, 65, 71, 72, 75,
			79, 83, 84, 93, 99

		};




		// Retira todas as bolas que saiu do concurso da lista que tem todas as bolas.
		foreach (var bola in bolasUltimoConcurso)
		{
			bolasLotomania.Remove(bola);
		}

		// Verifica se restaram oitenta números.
		if (bolasLotomania.Count != 80)
		{
			throw new IndexOutOfRangeException("A quantidade de números deve ser 80.");
		}

		List<int> distanciadores = new List<int>();

		// Inicia o gerador aleatório.
		Random geradorAleatorio = new Random((int)DateTime.Now.Ticks);

		// Guarda uma lista de arranjo de inteiros.
		List<int[]> listaJogos = new List<int[]>();

		// Guarda uma lista temporaria de todas as bolas.
		List<int> jogoOitentaBolas = new List<int>();

		int[] bolas = new int[80];

		int qtJogos = 0;
		while (qtJogos < jogoQuantidade)
		{
			int indiceAnterior = -1;

			jogoOitentaBolas.AddRange(bolasLotomania);

			// Gera aleatoriamente os 80 números.
			int qtBolas = 0;
			while (qtBolas < 80)
			{
				if (distanciadores.Count == 0)
				{
					distanciadores.AddRange(new int[] { 1, 2, 3, 4, 5 });
				}

				// Vamos pegar o índice do distanciador.
				int indiceDistanciador = geradorAleatorio.Next(distanciadores.Count);

				// Pega o valor que está naquele índice
				int proximoIndice = distanciadores[indiceDistanciador] + indiceAnterior;
				indiceAnterior = proximoIndice;

				// Apaga o índice do distanciador.
				distanciadores.RemoveAt(indiceDistanciador);

				// Verifica se o próximo índice está dentro do intervalo.
				if (proximoIndice > jogoOitentaBolas.Count - 1)
				{
					indiceAnterior = -1;
					distanciadores.Clear();
					continue;
				}

				int valorSelecionado = jogoOitentaBolas[proximoIndice];
				jogoOitentaBolas.RemoveAt(proximoIndice);

				// Adiciona ao arranjo.
				bolas[qtBolas] = valorSelecionado;

				/// O código abaixo é para testar os números sequencialmente.
				//bolas[qtBolas] = qtBolas;

				qtBolas++;

			}

			// Agora, vamos fazer grupos de 10 bolas, dos 80 números, 
			// Cada concurso terá 50 bolas, então terá 5 conjuntos.
			// Vamos permutar todos os conjuntos.
			int[] jogoGerado = new int[80];
			int indiceJogoGerado = 0;

			for (var grupoZero = 0; grupoZero <= 79; grupoZero += grupo_de_Bolas)
			{
				indiceJogoGerado = grupo_de_Bolas * 0;
				PreencherGrupo(grupoZero, grupo_de_Bolas, indiceJogoGerado, jogoGerado, bolas);

				for (var grupo1 = grupoZero + grupo_de_Bolas; grupo1 <= 79; grupo1 += grupo_de_Bolas)
				{
					indiceJogoGerado = grupo_de_Bolas * 1;
					PreencherGrupo(grupo1, grupo_de_Bolas, indiceJogoGerado, jogoGerado, bolas);

					for (var grupo2 = grupo1 + grupo_de_Bolas; grupo2 <= 79; grupo2 += grupo_de_Bolas)
					{
						indiceJogoGerado = grupo_de_Bolas * 2;
						PreencherGrupo(grupo2, grupo_de_Bolas, indiceJogoGerado, jogoGerado, bolas);

						for (var grupo3 = grupo2 + grupo_de_Bolas; grupo3 <= 79; grupo3 += grupo_de_Bolas)
						{
							indiceJogoGerado = grupo_de_Bolas * 3;
							PreencherGrupo(grupo3, grupo_de_Bolas, indiceJogoGerado, jogoGerado, bolas);

							for (var grupo4 = grupo3 + grupo_de_Bolas; grupo4 <= 79; grupo4 += grupo_de_Bolas)
							{
								indiceJogoGerado = grupo_de_Bolas * 4;
								PreencherGrupo(grupo4, grupo_de_Bolas, indiceJogoGerado, jogoGerado, bolas);

								//indiceJogoGerado = 40;
								//for (var uA = grupo4; uA < grupo4 + 10; uA++)
								//{
								//	jogoGerado[indiceJogoGerado] = bolas[uA];
								//	indiceJogoGerado++;
								//}
								if (grupo_de_Bolas == 5)
								{
									for (var grupo5 = grupo4 + grupo_de_Bolas; grupo5 <= 79; grupo5 += grupo_de_Bolas)
									{
										indiceJogoGerado = grupo_de_Bolas * 5;
										PreencherGrupo(grupo5, grupo_de_Bolas, indiceJogoGerado, jogoGerado, bolas);

										for (var grupo6 = grupo5 + grupo_de_Bolas; grupo6 <= 79; grupo6 += grupo_de_Bolas)
										{
											indiceJogoGerado = grupo_de_Bolas * 6;
											PreencherGrupo(grupo6, grupo_de_Bolas, indiceJogoGerado, jogoGerado, bolas);

											for (var grupo7 = grupo6 + grupo_de_Bolas; grupo7 <= 79; grupo7 += grupo_de_Bolas)
											{
												indiceJogoGerado = grupo_de_Bolas * 7;
												PreencherGrupo(grupo7, grupo_de_Bolas, indiceJogoGerado, jogoGerado, bolas);

												for (var grupo8 = grupo7 + grupo_de_Bolas; grupo8 <= 79; grupo8 += grupo_de_Bolas)
												{
													indiceJogoGerado = grupo_de_Bolas * 8;
													PreencherGrupo(grupo8, grupo_de_Bolas, indiceJogoGerado, jogoGerado, bolas);

													for (var grupo9 = grupo8 + grupo_de_Bolas; grupo9 <= 79; grupo9 += grupo_de_Bolas)
													{
														indiceJogoGerado = grupo_de_Bolas * 9;
														PreencherGrupo(grupo9, grupo_de_Bolas, indiceJogoGerado, jogoGerado, bolas);

														// Aqui, quer dizer, que todos os 80 números foram inseridos, devemos copiar
														// em um novo arranjo e adicionar à lista.
														int[] novoJogo = new int[80];
														novoJogo = (int[])jogoGerado.Clone();
														listaJogos.Add(novoJogo);
													}
												}
											}
										}
									}
								}
								else
								{
									// Aqui, quer dizer, que todos os 80 números foram inseridos, devemos copiar
									// em um novo arranjo e adicionar à lista.
									int[] novoJogo = new int[80];
									novoJogo = (int[])jogoGerado.Clone();

									listaJogos.Add(novoJogo);
								}

							}
						}
					}
				}
			}

			qtJogos++;
		}

		// Agora, vamos gerar um arranjo bidimensional, onde, haverá 100 colunas.
		// Nestas colunas, cada valor, será 0 ou 1, 1 indica selecionado.
		int[,] jogoResultado = null;

		qtJogos = listaJogos.Count;
		jogoResultado = new int[qtJogos, 100];

		// Passar para o arranjo.
		int contadorJogo = 0;
		foreach (int[] bolas_da_lista in listaJogos)
		{
			// No arranjo de 100 números, o número de cada índice, correspondente
			// a um dos 50 números, então, se o número foi sorteado, o conteúdo
			// deste índice do arranjo terá o valor 1.
			for (var uA = 0; uA <= 49; uA++)
			{
				jogoResultado[contadorJogo, bolas_da_lista[uA]] = 1;
			}
			contadorJogo += 1;
		}

		GravarModelo(jogoResultado);

	}



	/// <summary>
	/// Gerar aleatória vários números para a lotomania.
	/// </summary>
	/// <param name="jogoAposta">Jogo aposta.</param>
	/// <param name="jogoQuantidade">Jogo quantidade.</param>
	private void GerarLotomania(int jogoAposta, int jogoQuantidade)
	{
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
		List<int> distanciadores = new List<int>();

		// Inicializa o gerador de números aleatórios, com uma semente.
		Random geradorAleatorio = new Random((new Random()).Next(1000000));

		// Números do último jogo:
		List<int> bolasUltimoJogo = new List<int>(){
			1, 8, 17, 19, 29,
			31, 37, 40, 42, 43,
			45, 47, 50, 53, 62,
			66, 70, 80, 94, 96};

		while (indiceJogo < jogoQuantidade)
		{
			// Sempre zera o distanciador, quando começa um novo jogo.
			distanciadores.Clear();

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
			List<int> indiceNaoSorteado = new List<int>();
			for (var uA = 0; uA <= 99; uA++)
			{
				if (bolas_Sorteadas[indiceJogo, uA] == 0)
				{
					indiceNaoSorteado.Add(uA);
				}
			}

			// Pega o primeiro número.
			int indiceAnterior = geradorAleatorio.Next(0, 5);
			int qtBolasSorteadas = 1;
			bolas_Sorteadas[indiceJogo, indiceNaoSorteado[indiceAnterior]] = 1;

			System.Diagnostics.Debug.Print("Número Inicial: " + indiceAnterior + "\r\n");

			while (qtBolasSorteadas != jogoAposta)
			{
				if (distanciadores.Count == 0)
				{
					distanciadores.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
				}

				// Sortea o índice, em seguida, pega o valor que está armazenado neste índice
				// e adiciona este índice ao valor do índice anterior.
				//int indiceSorteado = geradorAleatorio.Next (0, distanciadores.Count);
				int indiceDistanciador = geradorAleatorio.Next(0, distanciadores.Count);

				string strTexto = "";
				strTexto += "qtBolasSorteadas: " + qtBolasSorteadas + "\r\n";
				strTexto += "Indice Distanciador sorteado: " + indiceDistanciador.ToString() + "\r\n";
				strTexto += "Valor no distanciador sorteado: " + distanciadores[indiceDistanciador] + "\r\n";
				strTexto += "Indice Anterior: " + indiceAnterior + "\r\n";

				int indiceProximo = distanciadores[indiceDistanciador] + indiceAnterior;
				distanciadores.RemoveAt(indiceDistanciador);

				strTexto += "Indice Próximo: " + indiceProximo.ToString() + "\r\n";

				if (indiceProximo > indiceNaoSorteado.Count - 1)
				{
					indiceAnterior = 0;
					distanciadores.Clear();
					indiceNaoSorteado.Clear();
					for (var uA = 0; uA <= 99; uA++)
					{
						if (bolas_Sorteadas[indiceJogo, uA] == 0)
						{
							indiceNaoSorteado.Add(uA);
						}
					}
				}
				else
				{
					// O arranjo 'indiceNaoSorteado' armazena os valores que são 
					// os índices do arranjo 'bolas_Sorteadas[,]', tais valores
					// correspondentes a valor do tipo bool com o valor falso.
					int numeroSorteado = indiceNaoSorteado[indiceProximo];
					strTexto += "Numero sorteado: " + numeroSorteado;
					strTexto += "\r\n=============================================\r\n";

					bolas_Sorteadas[indiceJogo, numeroSorteado] = 1;
					qtBolasSorteadas++;
					indiceAnterior = indiceProximo;
				}

				System.Diagnostics.Debug.Print(strTexto);
			}
			indiceJogo++;
		}

		// Agora, vamos criar o modelo pra o treeview.
		System.Type[] gTipos = new System.Type[jogoAposta + 2];

		for (var uA = 0; uA < gTipos.Length; uA++)
		{
			gTipos[uA] = typeof(string);
		}
		TreeStore treeStore = new TreeStore(gTipos);

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
		for (var uA = 0; uA < jogoQuantidade; uA += 2)
		{
			string[] bolasSelecionadas = new string[jogoAposta + 2];
			string[] bolasNaoSelecionadas = new string[jogoAposta + 2];
			bolasSelecionadas[0] = contadorRegistros.ToString();
			bolasNaoSelecionadas[0] = (++contadorRegistros).ToString() + " [" + bolasSelecionadas[0] + "]";
			bolasSelecionadas[1] = "LOTOMANIA";
			bolasNaoSelecionadas[1] = "LOTOMANIA";

			contadorRegistros++;

			int indiceBolaSelecionada = 2;
			int indiceBolaNaoSelecionada = 2;

			// Vamos concatenar as bolas, pra poder identificar depois se há bolas repetidas.
			StringBuilder strConcatenadaSelecionada = new StringBuilder();
			StringBuilder strConcatenadaNaoSelecionada = new StringBuilder();

			for (var uB = 0; uB <= 99; uB++)
			{
				if (bolas_Sorteadas[uA, uB] == 1)
				{
					bolasSelecionadas[indiceBolaSelecionada] = uB.ToString();
					indiceBolaSelecionada++;
				}
				else
				{
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


			treeStore.AppendValues(bolasSelecionadas);
			treeStore.AppendValues(bolasNaoSelecionadas);
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
		while (tvResultado.Columns.Length != 0)
		{
			tvResultado.RemoveColumn(tvResultado.Columns[0]);
		}

		// Vamos adicionar as colunas.
		tvResultado.AppendColumn(new TreeViewColumn("#Concurso", new CellRendererText(), "text", 0));
		tvResultado.AppendColumn(new TreeViewColumn("JOGO_TIPO", new CellRendererText(), "text", 1));

		// Vamos adicionar as colunas das bolas.
		for (var indice = 2; indice < jogoAposta + 2; indice++)
		{
			tvResultado.AppendColumn(new TreeViewColumn("B" + (indice - 1).ToString(),
				new CellRendererText(), "text", indice
			));
		}
		tvResultado.ColumnsAutosize();




	}

	private void GravarModelo(int[,] jogoNumeros)
	{
		int qtJogos = jogoNumeros.GetLength(0);

		// Gerar Modelo
		// Aqui, iremos definir o tipo das colunas que será usada no modelo.
		System.Type[] gTipos = new System.Type[52];

		for (var uA = 0; uA < gTipos.Length; uA++)
		{
			gTipos[uA] = typeof(string);
		}
		TreeStore treeStore = new TreeStore(gTipos);

		// Vamos criar os arranjos string que guardará as informações.
		string[] strBolasSelecionadas = new string[52];
		string[] strBolasNaoSelecionadas = new string[52];

		// Cria o arquivo que armazenará os resultados.
		System.IO.StreamWriter objArquivo = null;
		try
		{
			objArquivo = IniciarArquivo();
		}
		catch (System.IO.IOException)
		{
			return;
		}

		// Vamos percorrer o arranjo e apontar os números no arranjo correspondente.
		for (var uA = 0; uA < qtJogos; uA++)
		{
			strBolasSelecionadas[0] = (uA + 1).ToString();
			strBolasNaoSelecionadas[0] = (uA + 1).ToString() + ",ESP: " + strBolasSelecionadas[0];
			strBolasSelecionadas[1] = "LOTOMANIA";
			strBolasNaoSelecionadas[1] = "LOTOMANIA";
			int indiceBolaSelecionada = 2;
			int indiceBolaNaoSelecionada = 2;


			for (var uB = 0; uB <= 99; uB++)
			{
				if (jogoNumeros[uA, uB] == 1)
				{
					System.Diagnostics.Debug.Print("[1] => uB => " + uB);
					System.Diagnostics.Debug.Print("Antes de acessar o arranjo, indiceBolaSelecionada: " + indiceBolaSelecionada);
					strBolasSelecionadas[indiceBolaSelecionada] = uB.ToString();
					System.Diagnostics.Debug.Print("Após acessar o arranjo, indiceBolaSelecionada: " + indiceBolaSelecionada);
					indiceBolaSelecionada++;
				}
				else
				{
					System.Diagnostics.Debug.Print("[0] => uB => " + uB);
					System.Diagnostics.Debug.Print("Antes de acessar o arranjo, indiceBolaNaoSelecionada: " + indiceBolaNaoSelecionada);
					strBolasNaoSelecionadas[indiceBolaNaoSelecionada] = uB.ToString();
					System.Diagnostics.Debug.Print("Após acessar o arranjo, indiceBolaNaoSelecionada: " + indiceBolaNaoSelecionada);
					indiceBolaNaoSelecionada++;
				}
			}
			// Adiciona ao modelo.
			treeStore.AppendValues(strBolasSelecionadas);
			treeStore.AppendValues(strBolasNaoSelecionadas);

			ArquivoGravarLinha(objArquivo, strBolasSelecionadas);
			ArquivoGravarLinha(objArquivo, strBolasNaoSelecionadas);
		
		}

		// Terminamos de gravar, finalizar o arquivo.
		objArquivo.Flush();
		objArquivo.Close();
		objArquivo = null;

		// Agora, iremos associar o modelo criar ao treeView.
		tvResultado.Model = treeStore;
		tvResultado.EnableTreeLines = true;

		// Vamos apagar a coluna anterior.
		while (tvResultado.Columns.Length != 0)
		{
			tvResultado.RemoveColumn(tvResultado.Columns[0]);
		}

		// Vamos adicionar as colunas.
		tvResultado.AppendColumn(new TreeViewColumn("#Concurso", new CellRendererText(), "text", 0));
		tvResultado.AppendColumn(new TreeViewColumn("JOGO_TIPO", new CellRendererText(), "text", 1));

		// Vamos adicionar as colunas das bolas.
		for (var indice = 2; indice < 52; indice++)
		{
			tvResultado.AppendColumn(new TreeViewColumn("B" + (indice - 1).ToString(),
				new CellRendererText(), "text", indice
			));
		}
		tvResultado.ColumnsAutosize();



	}

	/// <summary>
	/// Iniciar um novo arquivo e coloca o cabeçalho inicial.
	/// </summary>
	/// <returns>The arquivo.</returns>
	private System.IO.StreamWriter IniciarArquivo()
	{
		// Gerar um nome do arquivo, com a data e hora no nome do arquivo.
		DateTime dataHoraAgora = DateTime.Now;
		string arquivoNome = "lotomania_" + dataHoraAgora.ToString("yyyy_MM_dd_HHmmss") + ".txt";
		System.IO.StreamWriter objArquivo = new System.IO.StreamWriter(arquivoNome);

		if (objArquivo != null)
		{
			ArquivoGravarCabecalho(objArquivo);
		}

		return objArquivo;
	}

	/// <summary>
	/// Grava o cabeçalho do arquivo.
	/// </summary>
	/// <param name="objArquivo">Object arquivo.</param>
	private void ArquivoGravarCabecalho(System.IO.StreamWriter objArquivo)
	{
		StringBuilder strCabecalho = new StringBuilder();

		strCabecalho.Append("#CONCURSO;JOGO_TIPO");

		// Adiciona os campos referente a bola.
		for (var indiceBola = 1; indiceBola <= 50; indiceBola++)
		{
			strCabecalho.Append(";");
			strCabecalho.Append("B");
			strCabecalho.Append(indiceBola);
		}
		objArquivo.WriteLine(strCabecalho.ToString());
		objArquivo.Flush();
	}


	private void ArquivoGravarLinha(System.IO.StreamWriter objArquivo, string[] strLinha)
	{
		StringBuilder strTexto = new StringBuilder();
		// Inicia o primeiro campo, para podermos evitar fazer o if dentro do loop.

		strTexto.Append(strLinha[0]);
		for (var uColunas = 1; uColunas < strLinha.Length; uColunas++)
		{
			strTexto.Append(";");
			strTexto.Append(strLinha[uColunas]);
		}
		objArquivo.WriteLine(strTexto);
	}


#if false



	// Gerar um nome do arquivo, com a data e hora no nome do arquivo.
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
		for (var uA = 0; uA < jogoQuantidade; uA += 2)
		{
			string[] bolasSelecionadas = new string[jogoAposta + 2];
			string[] bolasNaoSelecionadas = new string[jogoAposta + 2];
			bolasSelecionadas[0] = contadorRegistros.ToString();
			bolasNaoSelecionadas[0] = (++contadorRegistros).ToString() + " [" + bolasSelecionadas[0] + "]";
			bolasSelecionadas[1] = "LOTOMANIA";
			bolasNaoSelecionadas[1] = "LOTOMANIA";

			contadorRegistros++;

			int indiceBolaSelecionada = 2;
			int indiceBolaNaoSelecionada = 2;

			for (var uB = 0; uB <= 99; uB++)
			{
				if (bolas_Sorteadas[uA, uB] == 1)
				{
					bolasSelecionadas[indiceBolaSelecionada] = uB.ToString();
					indiceBolaSelecionada++;
				}
				else
				{
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


			treeStore.AppendValues(bolasSelecionadas);
			treeStore.AppendValues(bolasNaoSelecionadas);
		}

		// Grava os dados.
		objArquivo.Write(strLinhas.ToString());
		objArquivo.Flush();
		objArquivo.Close();
		objArquivo = null;

	}

#endif



	protected void OnCheckbutton1Toggled(object sender, EventArgs e)
	{
	}

	protected void OnChkVinteUltimosNumerosToggled(object sender, EventArgs e)
	{

	}

	protected void OnBtnGerar80Clicked (object sender, EventArgs e)
	{
		string jogoSelecionado = cmbJogo.ActiveText;

		// Vamos validar os dados.
		if (!lista_de_jogos.ContainsKey(jogoSelecionado))
			return;

		int jogoAposta = int.Parse(cmbJogoAposta.ActiveText);
		if (!lista_de_jogos[jogoSelecionado].Contains(jogoAposta.ToString()))
			return;

		int jogoQuantidade = int.Parse(cmbJogoQuantidade.ActiveText);
		if (Math.Sign(jogoQuantidade) == -1)
		{
			jogoQuantidade = Math.Abs(jogoQuantidade);
		}

		if (jogoQuantidade == 0)
		{
			jogoQuantidade = 1;
		}

		if (jogoSelecionado == "LOTOMANIA") {
			GerarLotomania80Numeros (50, jogoQuantidade, 10);
		}
	}

	protected void OnBtnGerarGrupo5Clicked(object sender, EventArgs e)
	{
		string jogoSelecionado = cmbJogo.ActiveText;

		// Vamos validar os dados.
		if (!lista_de_jogos.ContainsKey(jogoSelecionado))
			return;

		int jogoAposta = int.Parse(cmbJogoAposta.ActiveText);
		if (!lista_de_jogos[jogoSelecionado].Contains(jogoAposta.ToString()))
			return;

		int jogoQuantidade = int.Parse(cmbJogoQuantidade.ActiveText);
		if (Math.Sign(jogoQuantidade) == -1)
		{
			jogoQuantidade = Math.Abs(jogoQuantidade);
		}

		if (jogoQuantidade == 0)
		{
			jogoQuantidade = 1;
		}

		if (jogoSelecionado == "LOTOMANIA")
		{
			GerarLotomania80Numeros(50, jogoQuantidade, 5);
		}
	}
}
