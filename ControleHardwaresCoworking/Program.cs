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



            while (true)
            {
                Utils.FormataCabecalho("CONTROLE DE HARDWARES COWORKING MONTENEGRO");

                Console.WriteLine("1. Entrada de Estoque (Compra)");
                Console.WriteLine("2. Saída de Itens (Uso)");
                Console.WriteLine("3. Ajuste Item (Quebra/ Esquecimento de lançar)");
                Console.WriteLine("4. Ver Itens e Quantidade Disponível ");
                Console.WriteLine("5. Listar movimentações");
                Console.WriteLine("6. Cadastrar Novo Item");
                Console.WriteLine("0. Sair");

                Console.Write("\nSelecione uma opção: ");
                int opcao = Utils.EvitaQuebraCodInt(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        EntradaEstoqueService entradaEstoqueService = new EntradaEstoqueService();
                        entradaEstoqueService.ProcessarFuncionalidade();
                        break;
                    case 2:
                        SaidaEstoqueService saidaEstoqueService = new SaidaEstoqueService();
                        saidaEstoqueService.ProcessarFuncionalidade();
                        break;
                    case 3:
                        AjusteEstoqueService ajusteEstoqueService = new AjusteEstoqueService();
                        ajusteEstoqueService.ProcessarFuncionalidade();
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }

            }

        }
    }
}
