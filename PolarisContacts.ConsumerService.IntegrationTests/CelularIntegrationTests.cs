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

        //[Fact]
        //public async Task TesteAdicionarCelular()
        //{
        //    // Arrange
        //    var celular = new Celular
        //    {
        //        IdRegiao = 1,
        //        IdContato = 1,
        //        NumeroCelular = "91234-5678",
        //        Ativo = true
        //    };

        //    // Act
        //    var id = await _celularRepository.Add(celular);

        //    // Assert
        //    Assert.True(id > 0, "O ID do celular adicionado deve ser maior que 0.");

        //    // Verificar se o celular foi realmente adicionado
        //    using var connection = _fixture.ServiceProvider.GetService<IDatabaseConnection>().AbrirConexao();
        //    var celularAdicionado = await connection.QuerySingleOrDefaultAsync<Celular>("SELECT * FROM Celulares WHERE Id = @Id", new { Id = id });
        //    Assert.NotNull(celularAdicionado);
        //    Assert.Equal(celular.NumeroCelular, celularAdicionado.NumeroCelular);
        //}

    }
}
