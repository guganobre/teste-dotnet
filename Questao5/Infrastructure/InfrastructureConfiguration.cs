using Questao5.Domain.Repositories;
using Questao5.Domain.Sqlite;
using Questao5.Infrastructure.Repository;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // sqlite
            services.AddSingleton(new DatabaseConfig { Name = configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
            services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

            // repository
            services.AddSingleton<ICommandRepository, CommandStroreRepository>();
            services.AddSingleton<IContaCorrenteRepository, ContaCorrenteRepository>();
            services.AddSingleton<IMovimentoRepository, MovimentoRepository>();

            return services;
        }
    }
}