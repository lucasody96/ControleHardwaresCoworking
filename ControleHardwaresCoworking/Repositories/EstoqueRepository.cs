using System;
using System.Collections.Generic;
using System.Linq;
using ControleHardwaresCoworking.BancoDados;
using ControleHardwaresCoworking.Entities.Core;
using Dapper;

namespace ControleHardwaresCoworking.Repositories
{
    public class EstoqueRepository
    {
        private readonly ConexaoBD _conexaoBD = new ConexaoBD();

        public List<Produto> ListarItens()
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
    }
}
