using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PolarisContacts.ConsumerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IConnection _connection;
    private IModel _channel;
    private readonly IContatoService _contatoService;
    private readonly IEmailService _emailService;
    private readonly ITelefoneService _telefoneService;
    private readonly IEnderecoService _enderecoService;
    private readonly ICelularService _celularService;
    private readonly IUsuarioService _usuarioService;

    public Worker(ILogger<Worker> logger, IContatoService contatoService, IEmailService emailService,
                                          ITelefoneService telefoneService, IEnderecoService enderecoService,
                                          ICelularService celularService, IUsuarioService usuarioService)
    {
        _logger = logger;
        _contatoService = contatoService;
        _emailService = emailService;
        _telefoneService = telefoneService;
        _enderecoService = enderecoService;
        _celularService = celularService;
        _usuarioService = usuarioService;
        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        var factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
            Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest",
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "contact_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                Consume(message);
                _logger.LogInformation($"Mensagem processada: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar a mensagem: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: "contact_queue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    private void Consume(string message)
    {
        var entityMessage = JsonConvert.DeserializeObject<EntityMessage>(message) ?? throw new InvalidOperationException("Não foi possível interpretar a mensagem!");

        switch (entityMessage.EntityType)
        {
            case EntityType.Contato:
                _contatoService.ProcessContato(entityMessage);
                break;
            case EntityType.Email:
                _emailService.ProcessEmail(entityMessage);
                break;
            case EntityType.Telefone:
                _telefoneService.ProcessTelefone(entityMessage);
                break;
            case EntityType.Endereco:
                _enderecoService.ProcessEndereco(entityMessage);
                break;
            case EntityType.Celular:
                _celularService.ProcessCelular(entityMessage);
                break;
            case EntityType.Usuario:
                _usuarioService.ProcessUsuario(entityMessage);
                break;
            default:
                throw new InvalidOperationException("Tipo de entidade desconhecido.");
        }
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
