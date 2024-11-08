namespace Questao5.Domain.Repositories
{
    public interface IContaCorrenteRepository
    {
        bool Exists(string idContaCorrente);

        TResult GetSaldo<TResult>(string idContaCorrente);

        bool IsActive(string idContaCorrente);
    }
}