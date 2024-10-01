using Newtonsoft.Json;
using NSubstitute;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Services;
using PolarisContacts.ConsumerService.CrossCutting.Helpers;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;
using static PolarisContacts.ConsumerService.Domain.Exceptions.CustomExceptions;

namespace PolarisContacts.ConsumerService.UnitTests
{
    public class ContatoServiceTests
    {
        private readonly IContatoRepository _contatoRepository = Substitute.For<IContatoRepository>();
        private readonly ITelefoneRepository _telefoneRepository = Substitute.For<ITelefoneRepository>();
        private readonly ICelularRepository _celularRepository = Substitute.For<ICelularRepository>();
        private readonly IEmailRepository _emailRepository = Substitute.For<IEmailRepository>();
        private readonly IEnderecoRepository _enderecoRepository = Substitute.For<IEnderecoRepository>();
        private readonly IDatabaseConnection _dbConnection = Substitute.For<IDatabaseConnection>();
        private readonly ContatoService _contatoService;

        public ContatoServiceTests()
        {
            _contatoService = new ContatoService(_dbConnection, _contatoRepository, _telefoneRepository, _celularRepository, _emailRepository, _enderecoRepository);
        }

        [Fact]
        public async Task ProcessarContato_CriarContato_DeveAdicionarContatoComSucesso()
        {
            // Arrange
            var contato = new Contato { IdUsuario = 1, Nome = "João" };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(contato),  // Serializando o objeto contato para JSON
                Operation = OperationType.Create
            };

            _contatoRepository.Add(Arg.Any<Contato>(), Arg.Any<IDbConnection>(), Arg.Any<IDbTransaction>()).Returns(Task.FromResult(1));

            // Act
            var resultado = await _contatoService.ProcessContato(message);

            // Assert
            Assert.True(resultado);
            await _contatoRepository.Received(1).Add(Arg.Is<Contato>(x => x.IdUsuario == contato.IdUsuario && x.Nome == contato.Nome), Arg.Any<IDbConnection>(), Arg.Any<IDbTransaction>());
        }

        [Fact]
        public async Task ProcessarContato_AtualizarContato_DeveAtualizarContatoComSucesso()
        {
            // Arrange
            var contato = new Contato
            {
                Id = 1,
                IdUsuario = 1,
                Nome = "João"
            };

            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(contato),  // Serializando o contato
                Operation = OperationType.Update
            };

            // Simula a atualização do repositório
            _contatoRepository.Update(Arg.Any<Contato>()).Returns(Task.FromResult(true));

            // Act
            var resultado = await _contatoService.ProcessContato(message);

            // Assert
            Assert.True(resultado);
            await _contatoRepository.Received(1).Update(Arg.Is<Contato>(x => x.Id == contato.Id && x.IdUsuario == contato.IdUsuario && x.Nome == contato.Nome));
        }


        [Fact]
        public async Task ProcessarContato_InativarContato_DeveInativarContatoComSucesso()
        {
            // Arrange
            var contato = new Contato { IdUsuario = 1, Id = 1, Nome = "João" };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(contato),  // Serializando o contato
                Operation = OperationType.Inactivate
            };

            _contatoRepository.Inactivate(contato.Id).Returns(Task.FromResult(true));

            // Act
            var resultado = await _contatoService.ProcessContato(message);

            // Assert
            Assert.True(resultado);
            await _contatoRepository.Received(1).Inactivate(Arg.Is<int>(x => x == contato.Id));
        }

        [Fact]
        public async Task ProcessarAdicaoContato_DeveLancarUsuarioNotFoundException_QuandoIdUsuarioForInvalido()
        {
            // Arrange
            var contato = new Contato { IdUsuario = 0, Nome = "João" };  // IdUsuario inválido
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(contato),  // Serializando o contato
                Operation = OperationType.Create
            };

            // Act & Assert
            await Assert.ThrowsAsync<UsuarioNotFoundException>(() => _contatoService.ProcessContato(message));
        }

        [Fact]
        public async Task ProcessarAdicaoContato_DeveLancarNomeObrigatorioException_QuandoNomeForVazio()
        {
            // Arrange
            var contato = new Contato { IdUsuario = 1, Nome = "" };  // Nome vazio
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(contato),  // Serializando o contato
                Operation = OperationType.Create
            };

            // Act & Assert
            await Assert.ThrowsAsync<NomeObrigatorioException>(() => _contatoService.ProcessContato(message));
        }

        [Fact]
        public async Task ProcessarAdicaoContato_DeveLancarTelefoneInvalidoException_QuandoTelefoneForInvalido()
        {
            // Arrange
            var contato = new Contato
            {
                IdUsuario = 1,
                Nome = "João",
                Telefones = new List<Telefone> { new Telefone { NumeroTelefone = "1234" } }  // Telefone inválido
            };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(contato),  // Serializando o contato
                Operation = OperationType.Create
            };

            // Act & Assert
            await Assert.ThrowsAsync<TelefoneInvalidoException>(() => _contatoService.ProcessContato(message));
        }

        [Fact]
        public async Task ProcessarAdicaoContato_DeveLancarCelularInvalidoException_QuandoCelularForInvalido()
        {
            // Arrange
            var contato = new Contato
            {
                IdUsuario = 1,
                Nome = "João",
                Celulares = new List<Celular> { new Celular { NumeroCelular = "12345" } }  // Celular inválido
            };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(contato),  // Serializando o contato
                Operation = OperationType.Create
            };

            // Act & Assert
            await Assert.ThrowsAsync<CelularInvalidoException>(() => _contatoService.ProcessContato(message));
        }

        [Fact]
        public async Task ProcessarAdicaoContato_DeveLancarEmailInvalidoException_QuandoEmailForInvalido()
        {
            // Arrange
            var contato = new Contato
            {
                IdUsuario = 1,
                Nome = "João",
                Emails = new List<Email> { new Email { EnderecoEmail = "emailinvalido" } }  // E-mail inválido
            };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(contato),  // Serializando o contato
                Operation = OperationType.Create
            };

            // Act & Assert
            await Assert.ThrowsAsync<EmailInvalidoException>(() => _contatoService.ProcessContato(message));
        }
    }
}
