using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ControleHardwaresCoworking.BancoDados;
using ControleHardwaresCoworking.Entities.Core;

namespace ControleHardwaresCoworking.Repositories
{
    public class EstoqueRepository
    {
        private readonly ConexaoBD _conexaoBD = new ConexaoBD();

        public List<Produto> ListarItens()
        {
            // LISTAR PRODUTOS (Para o menu de escolha)
            var lista = new List<Produto>();

            using (var conexao = _conexaoBD.ObterConexao())
            {
                conexao.Open();
                string sql = @"
                    SELECT Id, Descricao, Saldo_Atual, Estoque_Minimo
                    FROM Produtos
                    ORDER BY Descricao";

                using (var cmd = new SqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Produto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Descricao = reader["Descricao"].ToString(),
                            SaldoAtual = Convert.ToInt32(reader["Saldo_Atual"]),
                            EstoqueMinimo = Convert.ToInt32(reader["Estoque_Minimo"])
                        });
                    }
                }
            }

            return lista;
        }


    }
}
