using System;

namespace ControleHardwaresCoworking.Entities.Dtos
{
    public class MovimentacaoRelatorio
    {
        public DateTime DataMovimentacao { get; set; }
        public char Tipo { get; set; }
        public int Quantidade { get; set; }

        // Propriedades que NÃO existem na tabela Movimentacao, 
        // mas existem no resultado do SELECT com JOIN
        public string NomeProduto { get; set; }
        public string NomeColaborador { get; set; }

    }
}
