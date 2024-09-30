using Dapper;
using Microsoft.Extensions.DependencyInjection;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain;
using Xunit;

namespace PolarisContacts.ConsumerService.IntegrationTests
{
    public class CelularIntegrationTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly IntegrationTestFixture _fixture;
        private readonly ICelularRepository _celularRepository;

        public CelularIntegrationTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _celularRepository = _fixture.ServiceProvider.GetService<ICelularRepository>();
        }

        [Fact]
        public async Task TesteAdicionarCelular()
        {
            // Arrange
            var celular = new Celular
            {
                IdRegiao = 1,
                IdContato = 1,
                NumeroCelular = "91234-5678",
                Ativo = true
            };

            // Act
            var id = await _celularRepository.Add(celular);

            // Assert
            Assert.True(id > 0, "O ID do celular adicionado deve ser maior que 0.");           
        }

        [Fact]
        public async Task TesteAtualizarCelular()
        {
            // Arrange
            var celular = new Celular
            {
                Id = 1,
                IdRegiao = 1,
                IdContato = 1,
                NumeroCelular = "91234-5678",
                Ativo = true
            };

            // Act
            var resultado = await _celularRepository.Update(celular);

            // Assert
            Assert.True(resultado, "A atualização do celular deve ser bem-sucedida.");
        }

        [Fact]
        public async Task TesteInativarCelular()
        {
            // Arrange
            var celularId = 1; // ID do celular a ser inativado

            // Act
            var resultado = await _celularRepository.Inactivate(celularId);

            // Assert
            Assert.True(resultado, "A inativação do celular deve ser bem-sucedida.");
        }
    }
}
