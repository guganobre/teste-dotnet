using Questao5.Application.Handlers;
using Questao5.Domain.Handlers;

namespace Questao5.Application
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IMovimentoHandler, MovimentoHandler>();
            services.AddSingleton<IContaCorrenteHandler, ContaCorrenteHandler>();

            return services;
        }
    }
}