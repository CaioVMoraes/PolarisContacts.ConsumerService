using Microsoft.Extensions.DependencyInjection;
using PolarisContacts.ConsumerService.CrossCutting.DependencyInjection.Extensions.AddApplicationLayer;
using PolarisContacts.ConsumerService.CrossCutting.DependencyInjection.Extensions.AddInfrastructureLayer;

namespace PolarisContacts.ConsumerService.CrossCutting.DependencyInjection;

public static class Bootstrap
{
    public static IServiceCollection RegisterServices(this IServiceCollection services) =>
        services
            .AddInfrastructure()
            .AddApplication();
}
