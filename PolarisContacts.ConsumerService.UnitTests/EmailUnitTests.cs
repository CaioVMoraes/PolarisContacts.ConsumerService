using Newtonsoft.Json;
using NSubstitute;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Application.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;
using System.Threading.Tasks;
using Xunit;

namespace PolarisContacts.ConsumerService.UnitTests
{
    public class EmailServiceTests
    {
        private readonly IEmailRepository _emailRepository = Substitute.For<IEmailRepository>();
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            _emailService = new EmailService(_emailRepository);
        }

        [Fact]
        public async Task ProcessEmail_CriarEmail_DeveAdicionarEmailComSucesso()
        {
            // Arrange
            var email = new Email { Id = 1, EnderecoEmail = "teste@exemplo.com" };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(email),
                Operation = OperationType.Create
            };

            // Act
            await _emailService.ProcessEmail(message);

            // Assert
            await _emailRepository.Received(1).Add(Arg.Is<Email>(x => x.EnderecoEmail == email.EnderecoEmail));
        }

        [Fact]
        public async Task ProcessEmail_AtualizarEmail_DeveAtualizarEmailComSucesso()
        {
            // Arrange
            var email = new Email { Id = 1, EnderecoEmail = "teste@exemplo.com" };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(email),
                Operation = OperationType.Update
            };

            // Act
            await _emailService.ProcessEmail(message);

            // Assert
            await _emailRepository.Received(1).Update(Arg.Is<Email>(x => x.Id == email.Id && x.EnderecoEmail == email.EnderecoEmail));
        }

        [Fact]
        public async Task ProcessEmail_InativarEmail_DeveInativarEmailComSucesso()
        {
            // Arrange
            var email = new Email { Id = 1 };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(email),
                Operation = OperationType.Inactivate
            };

            // Act
            await _emailService.ProcessEmail(message);

            // Assert
            await _emailRepository.Received(1).Inactivate(Arg.Is<int>(x => x == email.Id));
        }

        [Fact]
        public async Task ProcessEmail_DeveLancarExcecao_QuandoEmailForInvalido()
        {
            // Arrange
            var message = new EntityMessage
            {
                EntityData = "InvalidEmailData",  // Dados inválidos que causam a exceção
                Operation = OperationType.Create
            };

            // Act & Assert
            await Assert.ThrowsAsync<JsonReaderException>(() => _emailService.ProcessEmail(message));
        }

    }
}
