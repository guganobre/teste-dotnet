using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Domain.Handlers
{
    public interface IMovimentoHandler
    {
        Task<CreateMovimentoResponse> SaveAync(string idempotencyKey, CreateMovimentoRequest request);
    }
}