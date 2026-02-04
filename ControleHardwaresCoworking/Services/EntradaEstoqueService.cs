using ControleHardwaresCoworking.Interfaces;
using ControleHardwaresCoworking.Repositories;
using HextecInformatica.Entities;
using System;

namespace ControleHardwaresCoworking.Services
{
    public class EntradaEstoqueService: IServices<EstoqueRepository>
    {
        
        public void ProcessarFuncionalidade(EstoqueRepository estoqueRepository)
        {
            Console.Clear();
            Utils.FormataCabecalho("ENTRADA DE ITENS NO ESTOQUE");
            Utils.ListarProdutosTela(estoqueRepository);

            Console.ReadKey();

        }
    }
}
