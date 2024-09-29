using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain.Settings;
using PolarisContacts.ConsumerService.Infrastructure.Repositories;
using System;
using System.Data;

public class IntegrationTestFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }
    public IDatabaseConnection DatabaseConnection { get; private set; }
    public IDbConnection Connection { get; private set; }

    public IntegrationTestFixture()
    {
        var services = new ServiceCollection();

        // Carregar as configurações do AppSettings.Test.json
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json") 
            .Build();

        services.Configure<DbSettings>(configuration.GetSection("DbSettings"));

        services.AddScoped<IDatabaseConnection, DatabaseConnection>()
                .AddScoped<ICelularRepository, CelularRepository>()
                .AddScoped<IContatoRepository, ContatoRepository>()
                .AddScoped<IEmailRepository, EmailRepository>()
                .AddScoped<IEnderecoRepository, EnderecoRepository>()
                .AddScoped<ITelefoneRepository, TelefoneRepository>()
                .AddScoped<ICelularRepository, CelularRepository>();

        ServiceProvider = services.BuildServiceProvider();

        // Obter o DatabaseConnection e abrir a conexão
        DatabaseConnection = ServiceProvider.GetService<IDatabaseConnection>();
        Connection = DatabaseConnection.AbrirConexao(); 

    }

    public void Dispose()
    {
        Connection?.Dispose();
        ServiceProvider?.Dispose();
    }
}
