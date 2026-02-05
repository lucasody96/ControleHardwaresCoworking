using ControleHardwaresCoworking.Interfaces;
using ControleHardwaresCoworking.Repositories;
using ControleHardwaresCoworking.Entities.Core;
using ControleHardwaresCoworking.BancoDados;
using HextecInformatica.Entities;
using System;

namespace ControleHardwaresCoworking.Services
{
    public class EntradaEstoqueService: IServices<EstoqueRepository>
    {
        private readonly MovimentacaoRepository _movimentacaoRepos = new MovimentacaoRepository();
        private readonly ConexaoBD _conexaoBD = new ConexaoBD();
        
        public void ProcessarFuncionalidade(EstoqueRepository estoqueRepository)
        {
            Console.Clear();
            Utils.FormataCabecalho("ENTRADA DE ITENS NO ESTOQUE");
            Utils.ListarProdutosTela(estoqueRepository);

            int codItem = Utils.EvitaQuebraCodInt("\nPara atualizar a quantidade disponível de um produto, "+
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
            int quantidadeEntrada = Utils.EvitaQuebraCodInt("\nInforme a quantidade a ser adicionada ao estoque:");

            if (quantidadeEntrada <= 0)
            {
                Console.WriteLine("Quantidade inválida. Operação cancelada.");
                Console.ReadKey();
                return;
            }

            int idColaborador = Utils.EvitaQuebraCodInt("\nInforme o código do colaborador que registrou a entrada do produto: ");

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
                                produtoSelecionado.SaldoAtual + quantidadeEntrada,
                                conexao,
                                transacao
                            );

                            // Inserir movimentação (usando a sobrecarga COM transação)
                            _movimentacaoRepos.Inserir(new Movimentacao
                            {
                                IdProduto = produtoSelecionado.Id,
                                IdColaborador = idColaborador,
                                Tipo = 'E',
                                Quantidade = quantidadeEntrada,
                                DataMovimentacao = DateTime.Now
                            }, conexao, transacao);

                            // Confirma tudo
                            transacao.Commit();

                            Console.WriteLine($"\n✔ Entrada de {quantidadeEntrada} unidades do produto '{produtoSelecionado.Descricao}' registrada com sucesso."+
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
                Console.WriteLine($"\n✖ Erro ao processar entrada: {ex.Message}");
            }
        }
    }
}
