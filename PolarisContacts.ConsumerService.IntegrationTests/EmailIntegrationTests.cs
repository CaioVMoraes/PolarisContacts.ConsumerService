using Dapper;
using Microsoft.Extensions.DependencyInjection;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain;
using Xunit;

namespace PolarisContacts.ConsumerService.IntegrationTests
{
    public class EmailIntegrationTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly IntegrationTestFixture _fixture;
        private readonly IEmailRepository _EmailRepository;

        public EmailIntegrationTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _EmailRepository = _fixture.ServiceProvider.GetService<IEmailRepository>();
        }

        [Fact]
        public async Task TesteAdicionarEmail()
        {
            // Arrange
            var email = new Email { Id = 1, IdContato = 1, EnderecoEmail = "teste@exemplo.com" };

            // Act
            var id = await _EmailRepository.Add(email);

            // Assert
            Assert.True(id > 0, "O ID do Email adicionado deve ser maior que 0.");
        }

        [Fact]
        public async Task TesteAtualizarEmail()
        {
            // Arrange
            var email = new Email { Id = 1, IdContato = 1, EnderecoEmail = "teste@exemplo.com" };

            // Act
            var resultado = await _EmailRepository.Update(email);

            // Assert
            Assert.True(resultado, "A atualização do Email deve ser bem-sucedida.");
        }

        [Fact]
        public async Task TesteInativarEmail()
        {
            // Arrange
            var emailId = 1; // ID do Email a ser inativado

            // Act
            var resultado = await _EmailRepository.Inactivate(emailId);

            // Assert
            Assert.True(resultado, "A inativação do Email deve ser bem-sucedida.");
        }
    }
}
