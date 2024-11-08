using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Database.CommandStore.Requests;

namespace Questao5.Application.Handlers
{
    public class BaseHandler
    {
        private readonly ICommandRepository commandRepository;

        public BaseHandler(ICommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        protected TResult? ValidCommandStore<TResult>(string? requestId)
        {
            return commandRepository.GetByKey<TResult>(requestId);
        }

        public async Task SaveCommandStore(string requestId, object request, object response)
        {
            // salvado o resultado
            await commandRepository.SaveAsync(new CreateCommandStoreRequest
            {
                Chave = requestId,
                Requisicao = request,
                Resultado = response
            });
        }
    }
}