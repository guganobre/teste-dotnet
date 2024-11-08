namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public string IdMovimento { get; set; } = string.Empty;
        public string IdContaCorrente { get; set; } = string.Empty;
        public DateTime DataMovimento { get; set; }
        public char TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}