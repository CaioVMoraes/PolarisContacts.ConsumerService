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
    public class EnderecoServiceTests
    {
        private readonly IEnderecoRepository _enderecoRepository = Substitute.For<IEnderecoRepository>();
        private readonly EnderecoService _enderecoService;

        public EnderecoServiceTests()
        {
            _enderecoService = new EnderecoService(_enderecoRepository);
        }

        [Fact]
        public async Task ProcessEndereco_CriarEndereco_DeveAdicionarEnderecoComSucesso()
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
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(endereco),
                Operation = OperationType.Create
            };

            // Act
            await _enderecoService.ProcessEndereco(message);

            // Assert
            await _enderecoRepository.Received(1).Add(Arg.Is<Endereco>(x => x.Logradouro == endereco.Logradouro &&
                                                                            x.Cidade == endereco.Cidade &&
                                                                            x.Estado == endereco.Estado &&
                                                                            x.CEP == endereco.CEP));
        }

        [Fact]
        public async Task ProcessEndereco_AtualizarEndereco_DeveAtualizarEnderecoComSucesso()
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
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(endereco),
                Operation = OperationType.Update
            };

            // Act
            await _enderecoService.ProcessEndereco(message);

            // Assert
            await _enderecoRepository.Received(1).Update(Arg.Is<Endereco>(x => x.Id == endereco.Id &&
                                                                            x.Logradouro == endereco.Logradouro &&
                                                                            x.Cidade == endereco.Cidade &&
                                                                            x.Estado == endereco.Estado &&
                                                                            x.CEP == endereco.CEP));
        }

        [Fact]
        public async Task ProcessEndereco_InativarEndereco_DeveInativarEnderecoComSucesso()
        {
            // Arrange
            var endereco = new Endereco { Id = 1 };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(endereco),
                Operation = OperationType.Inactivate
            };

            // Act
            await _enderecoService.ProcessEndereco(message);

            // Assert
            await _enderecoRepository.Received(1).Inactivate(Arg.Is<int>(x => x == endereco.Id));
        }

        [Fact]
        public async Task ProcessEndereco_DeveLancarExcecao_QuandoEnderecoForInvalido()
        {
            // Arrange
            var message = new EntityMessage
            {
                EntityData = "InvalidEnderecoData",  // Dados inválidos que causam a exceção
                Operation = OperationType.Create
            };

            // Act & Assert
            await Assert.ThrowsAsync<JsonReaderException>(() => _enderecoService.ProcessEndereco(message));
        }
    }
}
