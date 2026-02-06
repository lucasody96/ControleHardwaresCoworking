using ControleHardwaresCoworking.BancoDados;
using ControleHardwaresCoworking.Entities.Core;
using ControleHardwaresCoworking.Entities.Dtos;
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

        public List<MovimentacaoRelatorio> Listar()
        {
            string sql = @"
                    SELECT 
                        m.DATA_MOVIMENTACAO AS DataMovimentacao,
                        m.TIPO,
                        m.QUANTIDADE,
                        p.DESCRICAO AS NomeProduto,
                        c.NOME AS NomeColaborador
                    FROM MOVIMENTACOES m
                    INNER JOIN PRODUTOS p ON m.ID_PRODUTO = p.ID
                    LEFT JOIN COLABORADORES c ON m.ID_COLABORADOR = c.ID
                    ORDER BY m.DATA_MOVIMENTACAO DESC";

            using (var conexao = _conexaoBD.ObterConexao())
            {
                return conexao.Query<MovimentacaoRelatorio>(sql).ToList();
            }
        }

        public List<MovimentacaoRelatorio> BuscaPorData(DateTime data)
        {
            // 1. O TRUQUE DO SQL: CAST(campo AS DATE)
            // Isso faz o SQL ignorar a hora (14:30) e comparar apenas o dia (06/02/2026)
            string sql = @"
                    SELECT 
                        m.DATA_MOVIMENTACAO AS DataMovimentacao,
                        m.TIPO,
                        m.QUANTIDADE,
                        p.DESCRICAO AS NomeProduto,
                        c.NOME AS NomeColaborador
                    FROM MOVIMENTACOES m
                    INNER JOIN PRODUTOS p ON m.ID_PRODUTO = p.ID
                    LEFT JOIN COLABORADORES c ON m.ID_COLABORADOR = c.ID
                    WHERE CAST(m.DATA_MOVIMENTACAO AS DATE) = @DataBusca
                    ORDER BY m.DATA_MOVIMENTACAO DESC";

            using (var conexao = _conexaoBD.ObterConexao())
            {
                // 2. PASSANDO O PARÂMETRO
                // Você cria um objeto anônimo "new { ... }" para ligar o @DataBusca do SQL com a variável C#
                return conexao.Query<MovimentacaoRelatorio>(sql, new { DataBusca = data.Date }).ToList();
            }
        }

        public List<MovimentacaoRelatorio> BuscaPorNomeProduto(string nomeProduto)
        {
            // Mudança 1: Troquei o '=' por 'LIKE'
            string sql = @"
                    SELECT 
                        m.DATA_MOVIMENTACAO AS DataMovimentacao,
                        m.TIPO,
                        m.QUANTIDADE,
                        p.DESCRICAO AS NomeProduto,
                        c.NOME AS NomeColaborador
                    FROM MOVIMENTACOES m
                    INNER JOIN PRODUTOS p ON m.ID_PRODUTO = p.ID
                    LEFT JOIN COLABORADORES c ON m.ID_COLABORADOR = c.ID
                    WHERE p.DESCRICAO LIKE @NomeProduto
                    ORDER BY m.DATA_MOVIMENTACAO DESC";

            using (var conexao = _conexaoBD.ObterConexao())
            {
                // Mudança 2: Adicionei os '%' antes e depois do nome
                // Isso significa: "Qualquer coisa antes + nome digitado + qualquer coisa depois"
                return conexao.Query<MovimentacaoRelatorio>(sql, new { NomeProduto = "%" + nomeProduto + "%" }).ToList();
            }
        }

        public List<MovimentacaoRelatorio> BuscaPorNomeColaborador(string nomeColaborador)
        {
            string sql = @"
                    SELECT 
                        m.DATA_MOVIMENTACAO AS DataMovimentacao,
                        m.TIPO,
                        m.QUANTIDADE,
                        p.DESCRICAO AS NomeProduto,
                        c.NOME AS NomeColaborador
                    FROM MOVIMENTACOES m
                    INNER JOIN PRODUTOS p ON m.ID_PRODUTO = p.ID
                    LEFT JOIN COLABORADORES c ON m.ID_COLABORADOR = c.ID
                    WHERE c.NOME LIKE @NomeColaborador
                    ORDER BY m.DATA_MOVIMENTACAO DESC";

            using (var conexao = _conexaoBD.ObterConexao())
            {
                return conexao.Query<MovimentacaoRelatorio>(sql, new { NomeColaborador = "%" + nomeColaborador + "%" }).ToList();
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
