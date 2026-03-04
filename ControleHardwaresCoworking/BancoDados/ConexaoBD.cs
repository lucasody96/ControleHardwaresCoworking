using System.Data.SqlClient;

namespace ControleHardwaresCoworking.BancoDados
{
    public class ConexaoBD
    {
        // Ajuste o "Server" para o nome do seu servidor SQL
        private readonly string connectionString = "Server=192.168.10.83;Database=CONTROLE_HARDWARE_COWORKING;User Id=CNP;Password=ninguemsabe;";

        public SqlConnection ObterConexao()
        {
            return new SqlConnection(connectionString);
        }
    }
}
