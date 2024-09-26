using Microsoft.Extensions.DependencyInjection;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Application.Services;

namespace PolarisContacts.ConsumerService.CrossCutting.DependencyInjection.Extensions.AddApplicationLayer;

public static partial class AddApplicationLayerExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddTransient<IUsuarioService, UsuarioService>()
                .AddTransient<IContatoService, ContatoService>()
                .AddTransient<ITelefoneService, TelefoneService>()
                .AddTransient<ICelularService, CelularService>()
                .AddTransient<IEmailService, EmailService>()
                .AddTransient<IEnderecoService, EnderecoService>();

    public static IServiceCollection AddApplication(this IServiceCollection services) => services
        .AddServices();
}
