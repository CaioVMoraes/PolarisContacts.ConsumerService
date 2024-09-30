using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.CrossCutting.DependencyInjection;
using PolarisContacts.ConsumerService.Domain.Settings;
using PolarisContacts.ConsumerService.Infrastructure.Repositories;
using System;
using System.Data;
using System.Data.Common;

public class IntegrationTestFixture : WebApplicationFactory<Program>
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

        services.AddSingleton<IConfiguration>(configuration);

        services.RegisterServices();

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
