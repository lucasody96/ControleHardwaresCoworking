using ControleHardwaresCoworking.BancoDados;
using ControleHardwaresCoworking.Entities.Core;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ControleHardwaresCoworking.Repositories
{
    public class MovimentacaoRepository
    {
        private readonly ConexaoBD _conexaoBD = new ConexaoBD();

        public List<Movimentacao> Listar()
        {
            string sql = @"
                SELECT Id, Id_Produto AS IdProduto, Id_Colaborador AS IdColaborador, 
                       Tipo, Quantidade, Data_Movimentacao AS DataMovimentacao
                FROM Movimentacoes
                ORDER BY Data_Movimentacao DESC";
            
            using (var conexao = _conexaoBD.ObterConexao())
            {
                return conexao.Query<Movimentacao>(sql).ToList();
            }
        }

        public Movimentacao ObterPorCodigo(int codigo)
        {
            string sql = @"
                SELECT Id, Id_Produto AS IdProduto, Id_Colaborador AS IdColaborador,
                       Tipo, Quantidade, Data_Movimentacao AS DataMovimentacao
                FROM Movimentacoes
                WHERE Id = @Codigo";
            
            using (var conexao = _conexaoBD.ObterConexao())
            {                 
                return conexao.QueryFirstOrDefault<Movimentacao>(sql, new { Codigo = codigo });
            }
        }

        // ✅ Versão SEM transação (para operações simples)
        public void Inserir(Movimentacao novaMovimentacao)
        {
            string sql = @"
                INSERT INTO Movimentacoes (Id_Produto, Id_Colaborador, Tipo, Quantidade, Data_Movimentacao)
                VALUES (@IdProduto, @IdColaborador, @Tipo, @Quantidade, @DataMovimentacao)";
            
            using (var conexao = _conexaoBD.ObterConexao())
            {
                conexao.Execute(sql, new
                {
                    novaMovimentacao.IdProduto,
                    novaMovimentacao.IdColaborador,
                    novaMovimentacao.Tipo,
                    novaMovimentacao.Quantidade,
                    novaMovimentacao.DataMovimentacao
                });
            }
        }

        // ✅ Versão COM transação (recebe conexão externa)
        public void Inserir(Movimentacao novaMovimentacao, IDbConnection conexao, IDbTransaction transacao)
        {
            string sql = @"
                INSERT INTO Movimentacoes (Id_Produto, Id_Colaborador, Tipo, Quantidade, Data_Movimentacao)
                VALUES (@IdProduto, @IdColaborador, @Tipo, @Quantidade, @DataMovimentacao)";
            
            conexao.Execute(sql, new
            {
                novaMovimentacao.IdProduto,
                novaMovimentacao.IdColaborador,
                novaMovimentacao.Tipo,
                novaMovimentacao.Quantidade,
                novaMovimentacao.DataMovimentacao
            }, transacao);
        }
    }
}
