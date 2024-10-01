using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Application.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xunit;
using static PolarisContacts.ConsumerService.Domain.Exceptions.CustomExceptions;

namespace PolarisContacts.ConsumerService.IntegrationTests
{
    public class ContatoIntegrationTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly IDatabaseConnection _dbConnection;
        private readonly IContatoRepository _contatoRepository;
        private readonly ITelefoneRepository _telefoneRepository;
        private readonly ICelularRepository _celularRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IContatoService _contatoService;

        public ContatoIntegrationTests(IntegrationTestFixture fixture)
        {
            _dbConnection = fixture.DatabaseConnection;
            _contatoRepository = fixture.ServiceProvider.GetService<IContatoRepository>();
            _telefoneRepository = fixture.ServiceProvider.GetService<ITelefoneRepository>();
            _celularRepository = fixture.ServiceProvider.GetService<ICelularRepository>();
            _emailRepository = fixture.ServiceProvider.GetService<IEmailRepository>();
            _enderecoRepository = fixture.ServiceProvider.GetService<IEnderecoRepository>();
            _contatoService = new ContatoService(_dbConnection, _contatoRepository, _telefoneRepository, _celularRepository, _emailRepository, _enderecoRepository);
        }


        [Fact]
        public async Task ProcessContato_DeveAdicionarContatoEInformacoesRelacionadas()
        {
            // Arrange
            var contato = new Contato
            {
                Nome = "Test Contato",
                IdUsuario = 1,
                Telefones = new List<Telefone>
                {
                    new Telefone { NumeroTelefone = "9999-9999" }
                },
                Celulares = new List<Celular>
                {
                    new Celular { NumeroCelular = "99999-9999" }
                },
                Emails = new List<Email>
                {
                    new Email { EnderecoEmail = "teste@exempl231o.com" }
                },
                Enderecos = new List<Endereco>
                {
                    new Endereco { Logradouro = "Rua Teste",
                                   Numero = "123",
                                   Cidade = "São Paulo",
                                   Estado = "SP",
                                   Bairro = "Jardim Itajaí",
                                   Complemento = "Casa",
                                   CEP = "04855-140"  }
                }
            };

            var message = new EntityMessage
            {
                Operation = OperationType.Create,
                EntityData = JsonConvert.SerializeObject(contato)
            };

            // Act
            var result = await _contatoService.ProcessContato(message);

            // Assert
            Assert.True(result, "O contato deveria ser adicionado com sucesso.");
        }

        [Fact]
        public async Task ProcessContato_DeveAtualizarContato()
        {
            // Arrange
            var contato = new Contato
            {
                Nome = "Contato Original",
                IdUsuario = 1
            };

            // Adiciona o contato inicialmente
            var messageCreate = new EntityMessage
            {
                Operation = OperationType.Create,
                EntityData = JsonConvert.SerializeObject(contato)
            };
            var createResult = await _contatoService.ProcessContato(messageCreate);

            Assert.True(createResult, "O contato deveria ser adicionado com sucesso.");

            contato.Id = 1;
            contato.Nome = "Contato Atualizado";

            var messageUpdate = new EntityMessage
            {
                Operation = OperationType.Update,
                EntityData = JsonConvert.SerializeObject(contato)
            };

            // Act
            var updateResult = await _contatoService.ProcessContato(messageUpdate);

            // Assert
            Assert.True(updateResult, "O contato deveria ser atualizado com sucesso.");
        }

        [Fact]
        public async Task ProcessContato_DeveInativarContato()
        {
            // Arrange
            var contato = new Contato
            {
                Nome = "Contato Para Inativar",
                IdUsuario = 1
            };

            // Adiciona o contato inicialmente
            var messageCreate = new EntityMessage
            {
                Operation = OperationType.Create,
                EntityData = JsonConvert.SerializeObject(contato)
            };
            var createResult = await _contatoService.ProcessContato(messageCreate);

            Assert.True(createResult, "O contato deveria ser adicionado com sucesso.");

            contato.Id = 1;

            // Act 
            var messageInactivate = new EntityMessage
            {
                Operation = OperationType.Inactivate,
                EntityData = JsonConvert.SerializeObject(contato)
            };

            var inactivateResult = await _contatoService.ProcessContato(messageInactivate);

            // Assert
            Assert.True(inactivateResult, "O contato deveria ser inativado com sucesso.");
        }

        [Fact]
        public async Task ProcessContato_DeveRetornarErroQuandoNomeObrigatorioNaoForInformado()
        {
            // Arrange
            var contato = new Contato
            {
                IdUsuario = 1
            };

            var message = new EntityMessage
            {
                Operation = OperationType.Create,
                EntityData = JsonConvert.SerializeObject(contato)
            };

            // Act & Assert
            await Assert.ThrowsAsync<NomeObrigatorioException>(() => _contatoService.ProcessContato(message));
        }


    }
}
