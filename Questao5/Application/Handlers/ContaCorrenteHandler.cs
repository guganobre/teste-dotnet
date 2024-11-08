using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Handlers;
using Questao5.Domain.Language;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Repository;

namespace Questao5.Application.Handlers
{
    public class ContaCorrenteHandler : BaseHandler, IContaCorrenteHandler
    {
        private readonly IContaCorrenteRepository contaCorrenteRepository;
        private readonly IMovimentoRepository movimentoRepository;
        //private readonly ICommandRepository commandRepository;

        public ContaCorrenteHandler(
            IContaCorrenteRepository contaCorrenteRepository,
            IMovimentoRepository movimentoRepository,
            ICommandRepository commandRepository) : base(commandRepository)
        {
            this.contaCorrenteRepository = contaCorrenteRepository;
            this.movimentoRepository = movimentoRepository;
            //this.commandRepository = commandRepository;
        }

        public async Task<SaldoContaCorrenteResponse> GetSaldo(string idempotencyKey, SaldoContaCorrenteRequest request)
        {
            // validação
            if (string.IsNullOrWhiteSpace(request.IdContaCorrente) || !contaCorrenteRepository.Exists(request.IdContaCorrente))
            {
                throw new ArgumentException(MensagemErro.INVALID_ACCOUNT);
            }

            if (!contaCorrenteRepository.IsActive(request.IdContaCorrente))
            {
                throw new ArgumentException(MensagemErro.INACTIVE_ACCOUNT);
            }

            if (!Guid.TryParse(idempotencyKey, out var key))
            {
                throw new Exception("Idempotency-Key não é um Guid válido");
            }

            // validação idenpotencia
            var commandStore = ValidCommandStore<SaldoContaCorrenteResponse>(idempotencyKey);
            if (commandStore != null)
            {
                return commandStore;
            }

            var response = contaCorrenteRepository.GetSaldo<SaldoContaCorrenteResponse>(request.IdContaCorrente);

            if (response != null)
            {
                response.Data = DateTime.Now;
            }

            await SaveCommandStore(idempotencyKey, request, response);

            return response;
        }
    }
}