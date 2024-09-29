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
    public class TelefoneServiceTests
    {
        private readonly ITelefoneRepository _telefoneRepository = Substitute.For<ITelefoneRepository>();
        private readonly TelefoneService _telefoneService;

        public TelefoneServiceTests()
        {
            _telefoneService = new TelefoneService(_telefoneRepository);
        }

        [Fact]
        public async Task ProcessTelefone_CriarTelefone_DeveAdicionarTelefoneComSucesso()
        {
            // Arrange
            var telefone = new Telefone { Id = 1, NumeroTelefone = "1234-5678", IdContato = 1, IdRegiao = 1 };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(telefone),
                Operation = OperationType.Create
            };

            // Act
            await _telefoneService.ProcessTelefone(message);

            // Assert
            await _telefoneRepository.Received(1).Add(Arg.Is<Telefone>(x => x.NumeroTelefone == telefone.NumeroTelefone));
        }

        [Fact]
        public async Task ProcessTelefone_AtualizarTelefone_DeveAtualizarTelefoneComSucesso()
        {
            // Arrange
            var telefone = new Telefone { Id = 1, NumeroTelefone = "1234-5678", IdContato = 1, IdRegiao = 1 };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(telefone),
                Operation = OperationType.Update
            };

            // Act
            await _telefoneService.ProcessTelefone(message);

            // Assert
            await _telefoneRepository.Received(1).Update(Arg.Is<Telefone>(x => x.Id == telefone.Id && x.NumeroTelefone == telefone.NumeroTelefone));
        }

        [Fact]
        public async Task ProcessTelefone_InativarTelefone_DeveInativarTelefoneComSucesso()
        {
            // Arrange
            var telefone = new Telefone { Id = 1 };
            var message = new EntityMessage
            {
                EntityData = JsonConvert.SerializeObject(telefone),
                Operation = OperationType.Inactivate
            };

            // Act
            await _telefoneService.ProcessTelefone(message);

            // Assert
            await _telefoneRepository.Received(1).Inactivate(Arg.Is<int>(x => x == telefone.Id));
        }

        [Fact]
        public async Task ProcessTelefone_DeveLancarExcecao_QuandoTelefoneForInvalido()
        {
            // Arrange
            var message = new EntityMessage
            {
                EntityData = "InvalidTelefoneData",  // Dados inválidos que causam a exceção
                Operation = OperationType.Create
            };

            // Act & Assert
            await Assert.ThrowsAsync<JsonReaderException>(() => _telefoneService.ProcessTelefone(message));
        }
    }
}
