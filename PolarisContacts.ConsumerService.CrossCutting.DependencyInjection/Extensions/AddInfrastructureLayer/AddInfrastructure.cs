using Microsoft.Extensions.DependencyInjection;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain.Settings;
using PolarisContacts.ConsumerService.Infrastructure.Repositories;

namespace PolarisContacts.ConsumerService.CrossCutting.DependencyInjection.Extensions.AddInfrastructureLayer;

public static partial class AddInfrastructureLayerExtensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services) =>
        services.AddBindedSettings<DbSettings>();

    public static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped<IUsuarioRepository, UsuarioRepository>()
                .AddScoped<IContatoRepository, ContatoRepository>()
                .AddScoped<ITelefoneRepository, TelefoneRepository>()
                .AddScoped<ICelularRepository, CelularRepository>()
                .AddScoped<IEmailRepository, EmailRepository>()
                .AddScoped<IEnderecoRepository, EnderecoRepository>()
                .AddScoped<IDatabaseConnection, DatabaseConnection>();

    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services
            .AddSettings()
            .AddRepositories();
}