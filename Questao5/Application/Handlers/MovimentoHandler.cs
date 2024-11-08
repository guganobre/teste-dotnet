using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Handlers;
using Questao5.Domain.Language;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using System.Text.Json;

namespace Questao5.Application.Handlers
{
    public class MovimentoHandler : BaseHandler, IMovimentoHandler
    {
        private readonly IMovimentoRepository movimentoRepository;
        private readonly IContaCorrenteRepository contaCorrenteRepository;

        public MovimentoHandler(IMovimentoRepository movimentoRepository,
            ICommandRepository commandRepository,
            IContaCorrenteRepository contaCorrenteRepository) : base(commandRepository)
        {
            this.movimentoRepository = movimentoRepository;
            this.contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<CreateMovimentoResponse> SaveAync(string idempotencyKey, CreateMovimentoRequest request)
        {
            // validação
            if (string.IsNullOrWhiteSpace(request.IdContaCorrente) || !contaCorrenteRepository.Exists(request.IdContaCorrente))
            {
                throw new ArgumentException(MensagemErro.INVALID_ACCOUNT);
            }

            if (!request.Valor.HasValue || request.Valor <= 0)
            {
                throw new ArgumentException(MensagemErro.INVALID_VALUE);
            }

            if (!request.TipoMovimento.Equals((char)TipoMovimento.Debito) && !request.TipoMovimento.Equals((char)TipoMovimento.Credito))
            {
                throw new ArgumentException(MensagemErro.INVALID_TYPE);
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
            var commandStore = ValidCommandStore<CreateMovimentoResponse>(idempotencyKey);
            if (commandStore != null)
            {
                return commandStore;
            }

            var entity = new Movimento
            {
                DataMovimento = DateTime.Now,
                IdContaCorrente = request.IdContaCorrente,
                Valor = request.Valor.Value,
                IdMovimento = Guid.NewGuid().ToString(),
                TipoMovimento = request.TipoMovimento.Value,
            };

            var movimento = await movimentoRepository.Save(entity);

            var result = new CreateMovimentoResponse
            {
                IdMovimento = movimento.IdMovimento
            };

            // salvado o resultado
            await SaveCommandStore(idempotencyKey, request, result);

            return result;
        }
    }
}