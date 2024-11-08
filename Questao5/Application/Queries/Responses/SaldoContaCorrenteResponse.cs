using Swashbuckle.AspNetCore.Annotations;

namespace Questao5.Application.Queries.Requests
{
    public class SaldoContaCorrenteResponse
    {
        [SwaggerSchema("Número da conta corrente.")]
        public string? Numero { get; set; }

        [SwaggerSchema("Nome do titular da conta corrente.")]
        public string? Titular { get; set; }

        [SwaggerSchema("Data e hora da resposta da consulta.")]
        public DateTime Data { get; set; }

        [SwaggerSchema("Valor do Saldo atual.")]
        public decimal Saldo { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is SaldoContaCorrenteResponse newobj)
            {
                return (Numero == newobj.Numero) &&
                    (Titular == newobj.Titular) &&
                    (Data == newobj.Data) &&
                    (Saldo == newobj.Saldo);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Numero, Titular, Data, Saldo);
        }
    }
}