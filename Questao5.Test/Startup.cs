using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Questao5.Application;
using Questao5.Infrastructure;
using Xunit.DependencyInjection;

namespace Questao5.Test;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Certifique-se de que o arquivo esteja na raiz do projeto
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        services.AddInfrastructure(config);
        services.AddApplication();
    }

    public void Configure(ITestOutputHelperAccessor accessor)
    {
        // Essa parte é opcional, usada para configurar o acesso ao `ITestOutputHelper`
    }
}