using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleHardwaresCoworking.Entities.Core
{
    public class Movimentacao: Entity
    {
        public int IdProduto { get; set; }
        public int IdColaborador { get; set; }
        public char Tipo { get; set; } // 'E' para entrada, 'S' para saída, A para ajuste
        public int Quantidade { get; set; }
        public DateTime DataMovimentacao { get; set; }
    }
}
