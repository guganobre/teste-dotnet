using Questao5.Application.Queries.Requests;

namespace Questao5.Domain.Handlers
{
    public interface IContaCorrenteHandler
    {
        Task<SaldoContaCorrenteResponse> GetSaldo(string idempotencyKey, SaldoContaCorrenteRequest request);
    }
}