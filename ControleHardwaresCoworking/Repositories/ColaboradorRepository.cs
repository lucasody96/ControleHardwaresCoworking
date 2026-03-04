
using ControleHardwaresCoworking.BancoDados;
using ControleHardwaresCoworking.Entities.Core;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ControleHardwaresCoworking.Repositories
{
    public class ColaboradorRepository
    {
        private readonly ConexaoBD _conexaoBD = new ConexaoBD();

        public List<Colaborador> Listar()
        {
            string sql = @"
                SELECT * 
                  FROM Colaboradores";

            using (var conexao = _conexaoBD.ObterConexao())
            {
                return conexao.Query<Colaborador>(sql).ToList();
            }
        }

        public Colaborador ObterPorCodigo(int codigo)
        {
            string sql = @"
                SELECT Id, Nome
                  FROM Colaboradores
                 WHERE Id = @Id";

            using (var conexao = _conexaoBD.ObterConexao())
            {
                return conexao.QueryFirstOrDefault<Colaborador>(sql, new { Id = codigo });
            }
        }
        public void Inserir(Colaborador novoColaborador)
        {
            string sql = @"
                Insert into COLABORADORES (NOME)
                VALUES (@Nome)";

            using (var conexao = _conexaoBD.ObterConexao())
            {
                conexao.Execute(sql, new
                {
                    novoColaborador.Nome
                });
            }
        }

        public void Atualizar(int idColaborador, string novoNome, IDbConnection conexao, IDbTransaction transacao)
        {
            string sql = @"
                UPDATE Colaboradores
                SET Nome = @NovoNome
                WHERE Id = @IdColaborador";

            conexao.Execute(sql, new { NovoNome = novoNome, IdColaborador = idColaborador }, transacao);
        }

        public void Excluir(int idColaborador, IDbConnection conexao, IDbTransaction transacao)
        {
            string sql = @"
                DELETE FROM Colaboradores
                WHERE Id = @IdColaborador";
            conexao.Execute(sql, new { IdColaborador = idColaborador }, transacao);

        }
    }
}
