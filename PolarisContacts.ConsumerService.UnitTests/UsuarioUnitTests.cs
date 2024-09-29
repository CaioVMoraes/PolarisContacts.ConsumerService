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
    public class UsuarioServiceTests
    {
        private readonly IUsuarioRepository _usuarioRepository = Substitute.For<IUsuarioRepository>();
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioService = new UsuarioService(_usuarioRepository);
        }

        [Fact]
        public async Task ProcessUsuario_CriarUsuario_DeveAdicionarUsuarioComSucesso()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Login = "usuario1", Senha = "senha123" };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(usuario),
                Operation = OperationType.Create
            };

            // Act
            await _usuarioService.ProcessUsuario(message);

            // Assert
            await _usuarioRepository.Received(1).Add(Arg.Is<Usuario>(x => x.Login == usuario.Login && x.Senha == usuario.Senha));
        }

        [Fact]
        public async Task ProcessUsuario_AtualizarSenha_DeveAtualizarSenhaDoUsuarioComSucesso()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Login = "usuario1", Senha = "senha123", NovaSenha = "novaSenha123" };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(usuario),
                Operation = OperationType.Update
            };

            // Act
            await _usuarioService.ProcessUsuario(message);

            // Assert
            await _usuarioRepository.Received(1).ChangeUserPassword(Arg.Is<Usuario>(x => x.Id == usuario.Id && x.NovaSenha == usuario.NovaSenha));
        }

        [Fact]
        public async Task ProcessUsuario_InativarUsuario_DeveLancarNotImplementedException()
        {
            // Arrange
            var usuario = new Usuario { Id = 1 };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(usuario),
                Operation = OperationType.Inactivate
            };

            // Act & Assert
            await Assert.ThrowsAsync<System.NotImplementedException>(() => _usuarioService.ProcessUsuario(message));
        }

        [Fact]
        public async Task ProcessUsuario_DeveLancarExcecao_QuandoUsuarioForInvalido()
        {
            // Arrange
            var message = new EntityMessage
            {
                EntityData = "InvalidUsuarioData",  // Dados inválidos que causam a exceção
                Operation = OperationType.Create
            };

            // Act & Assert
            await Assert.ThrowsAsync<JsonReaderException>(() => _usuarioService.ProcessUsuario(message));
        }
    }
}
