using Swashbuckle.AspNetCore.Annotations;

namespace Questao5.Application.Queries.Requests
{
    public class SaldoContaCorrenteRequest
    {
        [SwaggerSchema("Id da conta correte para consulta.", Nullable = false)]
        public string IdContaCorrente { get; set; }

        public SaldoContaCorrenteRequest()
        {
        }

        public SaldoContaCorrenteRequest(string idContaCorrente)
        {
            IdContaCorrente = idContaCorrente;
        }
    }
}