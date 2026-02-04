
namespace ControleHardwaresCoworking.Entities.Core
{
    public class Produto :  Entity
    {
        public string Descricao { get; set; }
        public int SaldoAtual { get; set; }
        public int EstoqueMinimo { get; set; }


    }
}
