using PolarisContacts.ConsumerService;
using PolarisContacts.ConsumerService.CrossCutting.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.RegisterServices();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
