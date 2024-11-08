using Swashbuckle.AspNetCore.Annotations;

namespace Questao5.Application.Commands.Requests
{
    public class CreateMovimentoRequest
    {
        [SwaggerSchema("Id da conta correte para movimentação.", Nullable = false)]
        public string? IdContaCorrente { get; set; }

        [SwaggerSchema("Tipo de movimentação. Ex: C = Crédito e D = Débito.", Nullable = false)]
        public char? TipoMovimento { get; set; }

        [SwaggerSchema("Valor a ser movimentado. Obs.: O valor precisa ser um numero positivo.", Nullable = false)]
        public decimal? Valor { get; set; }

        public CreateMovimentoRequest()
        {
        }

        public CreateMovimentoRequest(string idContaCorrete, char tipoMovimento, decimal valor)
        {
            IdContaCorrente = idContaCorrete;
            TipoMovimento = tipoMovimento;
            Valor = valor;
        }
    }
}