using System.Data.SqlClient;

namespace ControleHardwaresCoworking.ConexaoBD
{
    public class ConexaoBD
    {
        // Ajuste o "Server" para o nome do seu servidor SQL
        private readonly string connectionString = "Server=MTZNOTFS058675;Database=CONTROLE_HARDWARE_COWORKING;Trusted_Connection=True;";

        public SqlConnection ObterConexao()
        {
            return new SqlConnection(connectionString);
        }
    }
}
