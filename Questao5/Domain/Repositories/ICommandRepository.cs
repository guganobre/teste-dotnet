using Questao5.Infrastructure.Database.CommandStore.Requests;

namespace Questao5.Domain.Repositories
{
    public interface ICommandRepository
    {
        Task<bool> SaveAsync(CreateCommandStoreRequest command);

        TResult? GetByKey<TResult>(string? chave);
    }
}