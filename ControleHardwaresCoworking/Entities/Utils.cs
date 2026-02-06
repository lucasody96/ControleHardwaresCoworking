using ControleHardwaresCoworking.Entities.Dtos;
using ControleHardwaresCoworking.Repositories;
using System;
using System.Collections.Generic;

namespace HextecInformatica.Entities
{
    public class Utils
    {
        // Largura padrão para manter tudo alinhado (pode ajustar para 80 ou 100)
        private const int LarguraPadrao = 80;

        public static int EvitaQuebraCodInt(string mensagem)
        {
            int numInteiro;

            Console.Write(mensagem);

            while (!int.TryParse(Console.ReadLine(), out numInteiro))
            {
                Console.Write("Erro: Valor inválido (Informe apenas números inteiros) \n\n");
                Console.Write(mensagem);
            }
            return numInteiro;
        }

        public static DateTime EvitaQuebraCodData(string mensagem)
        {
            DateTime data;

            Console.Write(mensagem);

            while (!DateTime.TryParse(Console.ReadLine(), out data))
            {
                Console.Write("Erro: Valor inválido (Informe apenas a data) \n\n");
                Console.Write(mensagem);
            }

            return data;

        }

        public static double EvitaQuebraCodFloat(string mensagem)
        {
            double numFloat;

            Console.Write(mensagem);

            while (!double.TryParse(Console.ReadLine(), out numFloat))
            {
                Console.Write("Erro: Valor inválido , não é permitido informar letras e deve ser informado algum valor\n\n");
                Console.Write(mensagem);
            }

            return numFloat;
        }

        public static decimal EvitaQuebraCodDecimal(string mensagem)
        {
            decimal numDecimal; // 1. Mudamos a variável para decimal

            Console.Write(mensagem);

            // 2. Usamos decimal.TryParse
            while (!decimal.TryParse(Console.ReadLine(), out numDecimal))
            {
                Console.Write("Erro: Valor inválido. Não é permitido informar letras e deve ser informado algum valor.\n\n");
                Console.Write(mensagem);
            }

            return numDecimal;
        }

        public static void FormataCabecalho(string texto, char caractereBorda = '=')
        {
            string linhaSeparadora = new string(caractereBorda, LarguraPadrao);
            Console.WriteLine(linhaSeparadora);

            // Centraliza o texto
            int espacosTotais = LarguraPadrao - texto.Length;
            int espacosEsquerda = espacosTotais / 2;
            // O PadRight garante que complete a linha caso a divisão seja ímpar
            string textoCentralizado = texto.PadLeft(espacosEsquerda + texto.Length).PadRight(LarguraPadrao);

            Console.WriteLine(textoCentralizado);
            Console.WriteLine(linhaSeparadora);
        }

        // linhas divisórias simples
        public static void ImprimeLinhaSeparadora(char caractere)
        {
            Console.WriteLine(new string(caractere, LarguraPadrao));
        }

        public static void ListarProdutosTela(EstoqueRepository repo)
        {
            var lista = repo.Listar();

            if (lista.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[AVISO] Nenhum produto encontrado no estoque.");
                Console.ResetColor();
                return;
            }

            // --- 1. Formatação do Título ---
            Console.WriteLine(); // Linha em branco
            Console.ForegroundColor = ConsoleColor.Cyan; // Cor Azul Ciano para destaque
            Console.WriteLine("===================================================================");
            Console.WriteLine("                   LISTA DE PRODUTOS EM ESTOQUE                    ");
            Console.WriteLine("===================================================================");
            Console.ResetColor(); // Volta para a cor padrão

            // --- 2. Cabeçalho da Tabela ---
            // Explicação do {0,-5}: Ocupa 5 espaços e alinha à esquerda
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("{0,-5} | {1,-35} | {2,-10} | {3,-10}", "ID", "DESCRIÇÃO", "SALDO", "MÍNIMO");
            Console.WriteLine(new string('-', 67)); // Cria uma linha de separação
            Console.ResetColor();

            // --- 3. Linhas de Dados ---
            foreach (var item in lista)
            {
                // Tratamento para nomes muito longos não quebrarem a tabela
                string nomeFormatado = item.Descricao.Length > 32
                    ? item.Descricao.Substring(0, 32) + "..."
                    : item.Descricao;

                // Destaque visual: Se estoque estiver baixo (menor que o mínimo), pinta de vermelho
                if (item.SaldoAtual < item.EstoqueMinimo)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.WriteLine("{0,-5} | {1,-35} | {2,-10} | {3,-10}",
                    item.Id,
                    nomeFormatado,
                    item.SaldoAtual,
                    item.EstoqueMinimo
                );

                Console.ResetColor(); // Reseta a cor para a próxima linha
            }
            Console.WriteLine("===================================================================");
        }

        // Mude o parâmetro de (MovimentacaoRepository repo) PARA (List<MovimentacaoRelatorio> lista)
        public static void ListarMovimentacoesTela(List<MovimentacaoRelatorio> lista)
        {
            // REMOVA esta linha: var lista = repo.ListarComNomes(); 
            // (A lista já veio pronta no parâmetro!)

            if (lista.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[AVISO] Nenhuma movimentação encontrada para essa busca.");
                Console.ResetColor();
                return;
            }

            // --- CABEÇALHO COM AQUELE VISUAL ---
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=============================================================================================");
            Console.WriteLine("                                  HISTÓRICO DE MOVIMENTAÇÕES                                 ");
            Console.WriteLine("=============================================================================================");
            Console.ResetColor();

            // Cabeçalho das Colunas (Alinhamento negativo = esquerda)
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("{0,-18} | {1,-10} | {2,-30} | {3,-5} | {4,-20}",
                "DATA/HORA", "TIPO", "PRODUTO", "QTD", "COLABORADOR");
            Console.WriteLine(new string('-', 93));
            Console.ResetColor();

            // --- LINHAS ---
            foreach (var item in lista)
            {
                // 1. Define a cor e o texto do Tipo
                string textoTipo = "";
                ConsoleColor corTipo = ConsoleColor.White;

                switch (char.ToUpper(item.Tipo))
                {
                    case 'E':
                        textoTipo = "ENTRADA";
                        corTipo = ConsoleColor.Green; // Verde para lucro/entrada
                        break;
                    case 'S':
                        textoTipo = "SAÍDA";
                        corTipo = ConsoleColor.Red;   // Vermelho para saída
                        break;
                    case 'A':
                        textoTipo = "AJUSTE";
                        corTipo = ConsoleColor.Yellow; // Amarelo para atenção
                        break;
                }

                // 2. Trata nomes muito longos (para não quebrar a tabela) e nulos
                string prodFormatado = item.NomeProduto.Length > 28 ? item.NomeProduto.Substring(0, 28) + ".." : item.NomeProduto;

                string colabFormatado = item.NomeColaborador ?? "-"; // Se for null, coloca um traço
                if (colabFormatado.Length > 18) colabFormatado = colabFormatado.Substring(0, 18) + "..";

                // 3. Imprime a linha formatada
                Console.Write("{0,-18} | ", item.DataMovimentacao.ToString("dd/MM/yy"));

                Console.ForegroundColor = corTipo;
                Console.Write("{0,-10}", textoTipo);
                Console.ResetColor();

                Console.WriteLine(" | {0,-30} | {1,-5} | {2,-20}",
                    prodFormatado,
                    item.Quantidade,
                    colabFormatado);
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=============================================================================================");
            Console.ResetColor();
        }

        public static string PressioneTecla()
        {
            return "Pressione qualquer tecla para continuar...";
        }

    }
}

