using ControleHardwaresCoworking.BancoDados;
using ControleHardwaresCoworking.Interfaces;
using ControleHardwaresCoworking.Repositories;
using HextecInformatica.Entities;
using ControleHardwaresCoworking.Entities.Core;
using System;

namespace ControleHardwaresCoworking.Services
{
    public class CadastrarItemService: IServices<EstoqueRepository>
    {
        private readonly ConexaoBD _conexaoBD = new ConexaoBD();
        public void ProcessarFuncionalidade(EstoqueRepository estoqueRepository)
        {
            Console.Clear();
            Utils.FormataCabecalho("CADASTRAR ITEM NO ESTOQUE");
            Utils.ListarProdutosTela(estoqueRepository);

            Console.Write("Deseja cadastrar um novo produto (S/N)? ");
            string resposta = Console.ReadLine().ToUpper();

            if (resposta == "N")
            {
                Console.WriteLine($"Operação cancelada pelo usuário.{Utils.PressioneTecla()}");
                return;
            }

            Console.WriteLine("Informe os dados do novo produto:");
            Console.Write("Descrição: ");
            string descricao = Console.ReadLine();
            int saldoAtual = Utils.EvitaQuebraCodInt("Saldo Atual: ");
            int estoqueMinimo = Utils.EvitaQuebraCodInt("Estoque Mínimo: ");

            try
            {
                using (var conexao = _conexaoBD.ObterConexao())
                {
                    conexao.Open();

                    using (var transacao = conexao.BeginTransaction())
                    {
                        try
                        {
                            var novoProduto = new Produto
                            {
                                Descricao = descricao,
                                SaldoAtual = saldoAtual,
                                EstoqueMinimo = estoqueMinimo
                            };
                            estoqueRepository.Inserir(novoProduto);

                            transacao.Commit();

                            Console.WriteLine($"\n✔ Item {novoProduto.Descricao} cadastrado com sucesso.{Utils.PressioneTecla()}");
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

                Console.WriteLine($"\n✖ Erro ao processar inserção do item: {ex.Message}");
            }

        }
    }
}
