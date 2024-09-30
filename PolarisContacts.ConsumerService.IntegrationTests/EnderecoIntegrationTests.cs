using Dapper;
using Microsoft.Extensions.DependencyInjection;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Infrastructure.Repositories;
using Xunit;

namespace PolarisContacts.ConsumerService.IntegrationTests
{
    public class EnderecoIntegrationTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly IntegrationTestFixture _fixture;
        private readonly IEnderecoRepository _enderecoRepository;

        public EnderecoIntegrationTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _enderecoRepository = _fixture.ServiceProvider.GetService<IEnderecoRepository>();
        }

        [Fact]
        public async Task TesteAdicionarEndereco()
        {
            // Arrange
            var endereco = new Endereco
            {
                Id = 1,
                IdContato = 1,
                Logradouro = "Rua Exemplo",
                Cidade = "São Paulo",
                Estado = "SP",
                CEP = "00000-000"
            };

            // Act
            var id = await _enderecoRepository.Add(endereco);

            // Assert
            Assert.True(id > 0, "O ID do Endereco adicionado deve ser maior que 0.");
        }

        [Fact]
        public async Task TesteAtualizarEndereco()
        {
            // Arrange
            var endereco = new Endereco
            {
                Id = 1,
                Logradouro = "Rua Exemplo",
                Cidade = "São Paulo",
                Estado = "SP",
                CEP = "00000-000"
            };

            // Act
            var resultado = await _enderecoRepository.Update(endereco);

            // Assert
            Assert.True(resultado, "A atualização do Endereco deve ser bem-sucedida.");
        }

        [Fact]
        public async Task TesteInativarEndereco()
        {
            // Arrange
            var enderecoId = 1; 

            // Act
            var resultado = await _enderecoRepository.Inactivate(enderecoId);

            // Assert
            Assert.True(resultado, "A inativação do Endereco deve ser bem-sucedida.");
        }
    }
}
