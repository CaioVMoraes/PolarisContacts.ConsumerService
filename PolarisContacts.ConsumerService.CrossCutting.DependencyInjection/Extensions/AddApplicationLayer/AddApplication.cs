using Microsoft.Extensions.DependencyInjection;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Application.Services;

namespace PolarisContacts.ConsumerService.CrossCutting.DependencyInjection.Extensions.AddApplicationLayer;

public static partial class AddApplicationLayerExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddScoped<IUsuarioService, UsuarioService>()
                .AddScoped<IContatoService, ContatoService>()
                .AddScoped<ITelefoneService, TelefoneService>()
                .AddScoped<ICelularService, CelularService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<IEnderecoService, EnderecoService>();

    public static IServiceCollection AddApplication(this IServiceCollection services) => services
        .AddServices();
}
