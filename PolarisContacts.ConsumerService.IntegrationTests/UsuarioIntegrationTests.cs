using Dapper;
using Microsoft.Extensions.DependencyInjection;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Infrastructure.Repositories;
using Xunit;

namespace PolarisContacts.ConsumerService.IntegrationTests
{
    public class UsuarioIntegrationTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly IntegrationTestFixture _fixture;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioIntegrationTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _usuarioRepository = _fixture.ServiceProvider.GetService<IUsuarioRepository>();
        }

        [Fact]
        public async Task TesteAdicionarUsuario()
        {
            // Arrange
            var usuario = new Usuario { Login = "usuario1", Senha = "senha123" };

            // Act
            var id = await _usuarioRepository.Add(usuario);

            // Assert
            Assert.True(id, "O usuário deve ser adicionado com sucesso.");
        }

        [Fact]
        public async Task TesteAtualizarUsuario()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Login = "Login Teste", Senha = "1", NovaSenha = "Senha321"};

            // Act
            var resultado = await _usuarioRepository.ChangeUserPassword(usuario);

            // Assert
            Assert.True(resultado, "A atualização do Usuario deve ser bem-sucedida.");
        }       
    }
}
