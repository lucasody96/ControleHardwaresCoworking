using ControleHardwaresCoworking.Entities.Dtos;
using ControleHardwaresCoworking.Interfaces;
using ControleHardwaresCoworking.Repositories;
using HextecInformatica.Entities;
using System;
using System.Collections.Generic;

namespace ControleHardwaresCoworking.Services
{
    public class MovimentacoesService: IServices<MovimentacaoRepository>
    {
        public void ProcessarFuncionalidade(MovimentacaoRepository movimentacao)
        {
            
            while (true)
            {
                Console.Clear();
                Utils.FormataCabecalho("LISTAGEM DE MOVIMENTAÇÕES DOS ITENS");


                // 1. CARREGA A LISTA COMPLETA INICIALMENTE
                // Como mudamos o Utils para receber lista, precisamos buscar a lista antes
                var listaCompleta = movimentacao.Listar();
                Utils.ListarMovimentacoesTela(listaCompleta);

                Console.Write("\nDeseja buscar alguma movimentação (S/N)? ");
                string resposta = Console.ReadLine().Trim().ToUpper();

                if (resposta != "S")
                    break;

                Console.Write("Informe qual campo que deseja realizar a busca" +
                             "(D - Data, N - Nome(Descrição) Produto, C - Nome Colaborador): ");
                string campoBusca = Console.ReadLine().Trim().ToUpper();

                // 2. CRIAMOS UMA VARIÁVEL PARA GUARDAR O RESULTADO DA BUSCA
                List<MovimentacaoRelatorio> listaFiltrada = new List<MovimentacaoRelatorio>();

                if (campoBusca == "D")
                {
                    DateTime dataBusca = Utils.EvitaQuebraCodData("Informe a data(DD-MM-AAAA): ");
                    movimentacao.BuscaPorData(dataBusca);
                    listaFiltrada = movimentacao.BuscaPorData(dataBusca);
                }
                else if (campoBusca == "N")
                {
                    Console.Write("Informe o nome(Descrição) do Produto: ");
                    string nomeProduto = Console.ReadLine().Trim();
                    listaFiltrada = movimentacao.BuscaPorNomeProduto(nomeProduto);
                }
                else if (campoBusca == "C")
                {
                    Console.Write("Informe o nome do Colaborador: ");
                    string nomeColaborador = Console.ReadLine().Trim();
                    listaFiltrada = movimentacao.BuscaPorNomeColaborador(nomeColaborador);
                }
                else
                {
                    Console.WriteLine($"Campo de busca inválido. {Utils.PressioneTecla()}");
                    Console.ReadKey();
                    continue; // Volta para o início do while
                }

                // 3. AQUI ESTÁ O PULO DO GATO:
                // Limpamos a tela e mostramos SÓ A LISTA FILTRADA que guardamos acima
                Console.Clear();
                Utils.FormataCabecalho("RESULTADO DA BUSCA");
                Utils.ListarMovimentacoesTela(listaFiltrada);

                Console.WriteLine("\nBusca concluída. Pressione ENTER para listar tudo novamente.");
                Console.ReadLine();

            }

        }
    }
    
}
