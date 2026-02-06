using ControleHardwaresCoworking.Entities;
using ControleHardwaresCoworking.Repositories;
using ControleHardwaresCoworking.Services;
using HextecInformatica.Entities;
using System;

namespace ControleHardwaresCoworking
{
    class Program
    {
        static void Main()
        {
            EstoqueRepository estoqueRepository = new EstoqueRepository();
            ColaboradorRepository colaboradorRepository = new ColaboradorRepository();
            MovimentacaoRepository movimentacaoRepository = new MovimentacaoRepository();

            while (true)
            {
                Console.Clear();
                Utils.FormataCabecalho("CONTROLE DE HARDWARES COWORKING MONTENEGRO");

                Console.WriteLine("1. Entrada de Estoque (Compra)");
                Console.WriteLine("2. Saída de Itens (Uso)");
                Console.WriteLine("3. Ajuste Item (Quebra/ Esquecimento de lançar)");
                Console.WriteLine("4. Listar movimentações");
                Console.WriteLine("5. Cadastrar Novo Item");
                Console.WriteLine("6. Cadastrar novo Colaborador");
                Console.WriteLine("0. Sair");

                int opcao = Utils.EvitaQuebraCodInt("\nSelecione uma opção: ");

                if (opcao == 0)
                {
                    Console.WriteLine($"Saindo do sistema. {Utils.PressioneTecla()}");
                    Console.ReadKey();
                    break;
                }

                switch (opcao)
                {
                    case 1:
                        EntradaEstoqueService entradaEstoqueService = new EntradaEstoqueService();
                        entradaEstoqueService.ProcessarFuncionalidade(estoqueRepository);
                        break;
                    case 2:
                        SaidaEstoqueService saidaEstoqueService = new SaidaEstoqueService();
                        saidaEstoqueService.ProcessarFuncionalidade(estoqueRepository);
                        break;
                    case 3:
                        AjusteEstoqueService ajusteEstoqueService = new AjusteEstoqueService();
                        ajusteEstoqueService.ProcessarFuncionalidade(estoqueRepository);
                        break;
                    case 4:
                        MovimentacoesService MovimentacoesService = new MovimentacoesService();
                        MovimentacoesService.ProcessarFuncionalidade(movimentacaoRepository);
                        break;
                    case 5:
                        CadastrarItemService cadastrarItemService = new CadastrarItemService();
                        cadastrarItemService.ProcessarFuncionalidade(estoqueRepository);
                        break;
                    case 6:
                        //implementação futura cadastro colaborador
                        ManutencaoColaboradorService manutencaoColaboradorService = new ManutencaoColaboradorService();
                        manutencaoColaboradorService.ProcessarFuncionalidade(colaboradorRepository);
                        break;  
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }

            }

        }
    }
}
