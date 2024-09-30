using Dapper;
using Microsoft.Extensions.DependencyInjection;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Infrastructure.Repositories;
using Xunit;

namespace PolarisContacts.ConsumerService.IntegrationTests
{
    public class TelefoneIntegrationTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly IntegrationTestFixture _fixture;
        private readonly ITelefoneRepository _TelefoneRepository;

        public TelefoneIntegrationTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _TelefoneRepository = _fixture.ServiceProvider.GetService<ITelefoneRepository>();
        }

        [Fact]
        public async Task TesteAdicionarTelefone()
        {
            // Arrange
            var telefone = new Telefone { Id = 1, NumeroTelefone = "1234-5678", IdContato = 1, IdRegiao = 1 };

            // Act
            var id = await _TelefoneRepository.Add(telefone);

            // Assert
            Assert.True(id > 0, "O ID do Telefone adicionado deve ser maior que 0.");
        }

        [Fact]
        public async Task TesteAtualizarTelefone()
        {
            // Arrange
            var telefone = new Telefone { Id = 1, NumeroTelefone = "1234-5678", IdContato = 1, IdRegiao = 1 };

            // Act
            var resultado = await _TelefoneRepository.Update(telefone);

            // Assert
            Assert.True(resultado, "A atualização do Telefone deve ser bem-sucedida.");
        }

        [Fact]
        public async Task TesteInativarTelefone()
        {
            // Arrange
            var telefoneId = 1;

            // Act
            var resultado = await _TelefoneRepository.Inactivate(telefoneId);

            // Assert
            Assert.True(resultado, "A inativação do Telefone deve ser bem-sucedida.");
        }
    }
}
