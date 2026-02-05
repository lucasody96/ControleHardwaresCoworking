using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ControleHardwaresCoworking.BancoDados;
using ControleHardwaresCoworking.Entities.Core;
using Dapper;

namespace ControleHardwaresCoworking.Repositories
{
    public class EstoqueRepository
    {
        private readonly ConexaoBD _conexaoBD = new ConexaoBD();

        public List<Produto> Listar()
        {
            string sql = @"
                SELECT Id, Descricao, Saldo_Atual AS SaldoAtual, Estoque_Minimo AS EstoqueMinimo
                FROM Produtos
                ORDER BY Descricao";

            using (var conexao = _conexaoBD.ObterConexao())
            {
                return conexao.Query<Produto>(sql).ToList();
            }
        }

        public Produto ObterPorCodigo(int codigo)
        {
            string sql = @"
                SELECT Id, Descricao, Saldo_Atual AS SaldoAtual, Estoque_Minimo AS EstoqueMinimo
                FROM Produtos
                WHERE Id = @Codigo";
            
            using (var conexao = _conexaoBD.ObterConexao())
            {
                return conexao.QueryFirstOrDefault<Produto>(sql, new { Codigo = codigo });
            }
        }

        // ✅ Versão SEM transação (para operações simples)
        public void Atualizar(int idProduto, int novaQuantidade)
        {
            string sql = @"
                UPDATE Produtos
                SET Saldo_Atual = @NovaQuantidade
                WHERE Id = @IdProduto";
            
            using (var conexao = _conexaoBD.ObterConexao())
            {
                conexao.Execute(sql, new { NovaQuantidade = novaQuantidade, IdProduto = idProduto });
            }
        }

        // ✅ Versão COM transação (recebe conexão externa)
        public void Atualizar(int idProduto, int novaQuantidade, IDbConnection conexao, IDbTransaction transacao)
        {
            string sql = @"
                UPDATE Produtos
                SET Saldo_Atual = @NovaQuantidade
                WHERE Id = @IdProduto";
            
            conexao.Execute(sql, new { NovaQuantidade = novaQuantidade, IdProduto = idProduto }, transacao);
        }

        public void Inserir(Produto novoProduto)
        {
            string sql = @"
                INSERT INTO Produtos (Descricao, Saldo_Atual, Estoque_Minimo)
                VALUES (@Descricao, @SaldoAtual, @EstoqueMinimo)";
            
            using (var conexao = _conexaoBD.ObterConexao())
            {
                conexao.Execute(sql, new
                {
                    novoProduto.Descricao,
                    novoProduto.SaldoAtual,
                    novoProduto.EstoqueMinimo
                });
            }
        }
    }
}
