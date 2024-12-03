using Microsoft.Extensions.DependencyInjection;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain.Settings;
using PolarisContacts.ConsumerService.Infrastructure.Repositories;

namespace PolarisContacts.ConsumerService.CrossCutting.DependencyInjection.Extensions.AddInfrastructureLayer;

public static partial class AddInfrastructureLayerExtensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services) =>
        services.AddBindedSettings<DbSettings>()
                .AddBindedSettings<RabbitMQ>();

    public static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddTransient<IUsuarioRepository, UsuarioRepository>()
                .AddTransient<IContatoRepository, ContatoRepository>()
                .AddTransient<ITelefoneRepository, TelefoneRepository>()
                .AddTransient<ICelularRepository, CelularRepository>()
                .AddTransient<IEmailRepository, EmailRepository>()
                .AddTransient<IEnderecoRepository, EnderecoRepository>()
                .AddTransient<IDatabaseConnection, DatabaseConnection>();

    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services
            .AddSettings()
            .AddRepositories();
}