using ControleHardwaresCoworking.BancoDados;
using ControleHardwaresCoworking.Entities.Core;
using ControleHardwaresCoworking.Interfaces;
using ControleHardwaresCoworking.Repositories;
using HextecInformatica.Entities;
using System;
using System.Collections.Generic;
namespace ControleHardwaresCoworking.Services
{
    public class SaidaEstoqueService: IServices<EstoqueRepository>
    {
        private readonly MovimentacaoRepository _movimentacaoRepos = new MovimentacaoRepository();

        private readonly ColaboradorRepository _colaboradorRepos = new ColaboradorRepository();

        private readonly ConexaoBD _conexaoBD = new ConexaoBD();

        public void ProcessarFuncionalidade(EstoqueRepository estoqueRepository)
        {
            while (true)
            {
                Console.Clear();
                Utils.FormataCabecalho("RETIRADA DE ITENS NO ESTOQUE");
                Utils.ListarProdutosTela(estoqueRepository);

                int codItem = Utils.EvitaQuebraCodInt("\nPara retirar um produto, " +
                                                      "informe o código do produto desejado ou 0 para sair: ");

                if (codItem == 0)
                {
                    Console.WriteLine("Voltando ao menu anterior...");
                    Console.ReadKey();
                    return;
                }

                Produto produtoSelecionado = estoqueRepository.ObterPorCodigo(codItem);

                if (produtoSelecionado == null)
                {
                    Console.WriteLine("Produto não encontrado. Operação cancelada.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine($"Produto selecionado: {produtoSelecionado.Descricao}");
                int quantidadeRetirada = Utils.EvitaQuebraCodInt("\nInforme a quantidade a ser retirada: ");

                if (quantidadeRetirada <= 0)
                {
                    Console.WriteLine("Quantidade inválida. Operação cancelada.");
                    Console.ReadKey();
                    return;
                }

                var listaColaboradores = _colaboradorRepos.Listar();
                Utils.ListarColaboradoresTela(listaColaboradores);

                int idColaborador = Utils.EvitaQuebraCodInt("\nInforme o código do colaborador que registrou a retirada do produto: ");

                // ✅ AGORA COM TRANSAÇÃO: Usando a mesma conexão para ambas as operações
                try
                {
                    using (var conexao = _conexaoBD.ObterConexao())
                    {
                        conexao.Open();
                        using (var transacao = conexao.BeginTransaction())
                        {
                            try
                            {
                                // Atualizar estoque (usando a sobrecarga COM transação)
                                estoqueRepository.Atualizar(
                                    produtoSelecionado.Id,
                                    produtoSelecionado.SaldoAtual - quantidadeRetirada,
                                    conexao,
                                    transacao
                                );

                                // Inserir movimentação (usando a sobrecarga COM transação)
                                _movimentacaoRepos.Inserir(new Movimentacao
                                {
                                    IdProduto = produtoSelecionado.Id,
                                    IdColaborador = idColaborador,
                                    Tipo = 'S',
                                    Quantidade = quantidadeRetirada,
                                    DataMovimentacao = DateTime.Now
                                }, conexao, transacao);

                                // Confirma tudo
                                transacao.Commit();

                                Console.WriteLine($"\nRetirada de {quantidadeRetirada} unidades do produto '{produtoSelecionado.Descricao}' registrada com sucesso." +
                                                  $"\n{Utils.PressioneTecla()}");
                                Console.ReadKey();
                            }
                            catch
                            {
                                transacao.Rollback();
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nErro ao processar retirada de produto: {ex.Message}");
                }
            }
        }
    }
}
