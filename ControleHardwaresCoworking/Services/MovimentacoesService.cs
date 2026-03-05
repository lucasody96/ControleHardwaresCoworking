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
                Console.Out.Flush(); // ✅ FORÇA LIMPAR O BUFFER
                
                var listaCompleta = movimentacao.Listar();
                Utils.ListarMovimentacoesTela(listaCompleta);

                Console.Write("\nDeseja buscar alguma movimentação (S/N)? ");
                string resposta = Console.ReadLine().Trim().ToUpper();

                if (resposta != "S")
                    break;

                Console.Write("\nInforme qual campo que deseja realizar a busca " +
                             "(D - Data, N - Nome(Descrição) Produto, C - Nome Colaborador): ");
                string campoBusca = Console.ReadLine().Trim().ToUpper();

                List<MovimentacaoRelatorio> listaFiltrada = new List<MovimentacaoRelatorio>();

                if (campoBusca == "D")
                {
                    DateTime dataBusca = Utils.EvitaQuebraCodData("\nInforme a data(DD/MM/AAAA): ");
                    listaFiltrada = movimentacao.BuscaPorData(dataBusca);
                }
                else if (campoBusca == "N")
                {
                    Console.Write("\nInforme o nome(Descrição) do Produto: ");
                    string nomeProduto = Console.ReadLine().Trim();
                    listaFiltrada = movimentacao.BuscaPorNomeProduto(nomeProduto);
                }
                else if (campoBusca == "C")
                {
                    Console.Write("\nInforme o nome do Colaborador: ");
                    string nomeColaborador = Console.ReadLine().Trim();
                    listaFiltrada = movimentacao.BuscaPorNomeColaborador(nomeColaborador);
                }
                else
                {
                    Console.WriteLine($"\nCampo de busca inválido. {Utils.PressioneTecla()}");
                    Console.ReadKey();
                    continue;
                }

                Console.Clear();
                Console.Out.Flush(); // ✅ FORÇA LIMPAR O BUFFER
                Utils.ListarMovimentacoesTela(listaFiltrada);
                
                Console.WriteLine($"\nBusca concluída. {Utils.PressioneTecla()}");
                Console.ReadKey(); // ✅ MUDEI PARA ReadKey (melhor controle)
            }
        }
    }
}
