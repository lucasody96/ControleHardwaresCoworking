using ControleHardwaresCoworking.BancoDados;
using ControleHardwaresCoworking.Entities.Core;
using ControleHardwaresCoworking.Interfaces;
using ControleHardwaresCoworking.Repositories;
using HextecInformatica.Entities;
using System;

namespace ControleHardwaresCoworking.Services
{

    public class ManutencaoColaboradorService: IServices<ColaboradorRepository>
    {
        private readonly ConexaoBD _conexaoBD = new ConexaoBD();
        public void ProcessarFuncionalidade(ColaboradorRepository colaboradorRepos)
        {
            while (true)
            {
                Console.Clear();
                Utils.FormataCabecalho("CADASTRO DE COLABORADORES");
                //listar os colaboradores atuais
                var listaColaboradores = colaboradorRepos.Listar();
                Utils.ListarColaboradoresTela(listaColaboradores);

                Console.WriteLine("\nOpções disponíveis: ");
                Console.WriteLine("\n [I] Incluir Novo Colaborador");
                Console.WriteLine(" [A] Alterar");
                Console.WriteLine(" [E] Excluir");
                Console.WriteLine(" [S] Sair");
                Console.Write("\nO que você deseja fazer: ");
                string resposta = Console.ReadLine().ToUpper();

                if (resposta == "S")
                {
                    Console.WriteLine($"Operação cancelada pelo usuário.{Utils.PressioneTecla()}");
                    return;
                }

                switch (resposta)
                {
                    case "I":

                        Console.WriteLine("Informe os dados do novo colaborador:");
                        Console.Write("Nome: ");
                        string nome = Console.ReadLine();

                        try
                        {
                            using (var conexao = _conexaoBD.ObterConexao())
                            {
                                conexao.Open();

                                using (var transacao = conexao.BeginTransaction())
                                {
                                    try
                                    {
                                        var novoColaborador = new Colaborador
                                        {
                                            Nome = nome
                                        };
                                        colaboradorRepos.Inserir(novoColaborador);

                                        transacao.Commit();

                                        Console.WriteLine($"\nItem {novoColaborador.Nome} cadastrado com sucesso.{Utils.PressioneTecla()}");
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
                        break;

                    case "A":
                        int codColaborador = Utils.EvitaQuebraCodInt("\nPara ajustar o nome do colaborador, " +
                                                              "informe o código do colaborador desejado ou 0 para sair: ");

                        if (codColaborador == 0)
                        {
                            Console.WriteLine("Voltando ao menu anterior...");
                            Console.ReadKey();
                            return;
                        }

                        Colaborador colaborador = colaboradorRepos.ObterPorCodigo(codColaborador);

                        if (colaborador == null)
                        {
                            Console.WriteLine("Colaborador não encontrado. Operação cancelada.");
                            Console.ReadKey();
                            return;
                        }

                        Console.WriteLine($"Colaborador: {colaborador.Nome}");
                        Console.Write("Informe o nome correto do colaborador: ");
                        string novoNome = Console.ReadLine();

                        try
                        {
                            using (var conexao = _conexaoBD.ObterConexao())
                            {
                                conexao.Open();

                                using (var transacao = conexao.BeginTransaction())
                                {
                                    try
                                    {
                                        // Atualizar colaborador passando a conexão e a transação
                                        colaboradorRepos.Atualizar(
                                            colaborador.Id,
                                            novoNome,
                                            conexao,      
                                            transacao     
                                        );

                                        // ✅ Confirma a gravação no banco de dados!
                                        transacao.Commit();

                                        Console.WriteLine("\nNome do colaborador atualizado com sucesso!");
                                        Console.WriteLine(Utils.PressioneTecla());
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
                            Console.WriteLine($"\n✖ Erro ao processar atualização do colaborador: {ex.Message}");
                        }

                        break;
                    case "E":

                        codColaborador = Utils.EvitaQuebraCodInt("\nPara excluir o colaborador, " +
                                                              "informe o código do colaborador desejado ou 0 para sair: ");

                        if (codColaborador == 0)
                        {
                            Console.WriteLine("Voltando ao menu anterior...");
                            Console.ReadKey();
                            return;
                        }

                        colaborador = colaboradorRepos.ObterPorCodigo(codColaborador);

                        if (colaborador == null)
                        {
                            Console.WriteLine("Colaborador não encontrado. Operação cancelada.");
                            Console.ReadKey();
                            return;
                        }

                        Console.WriteLine($"Colaborador: {colaborador.Nome}");
                        Console.Write("Tem certeza que deseja excluir o colaborador (S/N)? ");
                        resposta = Console.ReadLine();

                        if (resposta.ToUpper() != "S")
                        {
                            Console.WriteLine("Operação de exclusão cancelada.");
                            Console.ReadKey();
                            return;
                        }

                        try
                        {
                            using (var conexao = _conexaoBD.ObterConexao())
                            {
                                conexao.Open();

                                using (var transacao = conexao.BeginTransaction())
                                {
                                    try
                                    {
                                        colaboradorRepos.Excluir(
                                            colaborador.Id,
                                            conexao,
                                            transacao
                                        );

                                        // ✅ Confirma a gravação no banco de dados!
                                        transacao.Commit();

                                        Console.WriteLine("\nColaborador excluído com sucesso!");
                                        Console.WriteLine(Utils.PressioneTecla());
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
                            Console.WriteLine($"\n✖ Erro ao processar atualização do colaborador: {ex.Message}");
                        }

                        break;

                    default:
                        Console.WriteLine($"Opção inválida. Operação cancelada.{Utils.PressioneTecla()}");
                        break;
                }
            }
        }
    }
}
