using NSubstitute;
using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;
using Xunit;

namespace PolarisContacts.ConsumerService.UnitTests
{
    public class CelularUnitTests
    {
        private readonly ICelularRepository _celularRepository = Substitute.For<ICelularRepository>();
        private readonly CelularService _celularService;

        public CelularUnitTests()
        {
            _celularService = new CelularService(_celularRepository);
        }

        [Fact]
        public void ProcessarCelular_DeveChamarAdicionar_QuandoOperacaoForCriar()
        {
            // Arrange
            var celular = new Celular { Id = 1, NumeroCelular = "99999-9999" };
            var message = new EntityMessage
            {
                Operation = OperationType.Create,
                EntityData = JsonConvert.SerializeObject(celular)
            };

            // Act
            _celularService.ProcessCelular(message);

            // Assert
            _celularRepository.Received(1).Add(Arg.Is<Celular>(x => x.Id == celular.Id && x.NumeroCelular == celular.NumeroCelular));
        }

        [Fact]
        public void ProcessarCelular_DeveChamarAtualizar_QuandoOperacaoForAtualizar()
        {
            // Arrange
            var celular = new Celular { Id = 1, NumeroCelular = "88888-8888" };
            var message = new EntityMessage
            {
                Operation = OperationType.Update,
                EntityData = JsonConvert.SerializeObject(celular)
            };

            // Act
            _celularService.ProcessCelular(message);

            // Assert
            _celularRepository.Received(1).Update(Arg.Is<Celular>(x => x.Id == celular.Id && x.NumeroCelular == celular.NumeroCelular));
        }

        [Fact]
        public void ProcessarCelular_DeveChamarInativar_QuandoOperacaoForInativar()
        {
            // Arrange
            var celular = new Celular { Id = 1, NumeroCelular = "77777-7777" };
            var message = new EntityMessage
            {
                Operation = OperationType.Inactivate,
                EntityData = JsonConvert.SerializeObject(celular)
            };

            // Act
            _celularService.ProcessCelular(message);

            // Assert
            _celularRepository.Received(1).Inactivate(celular.Id);
        }
    }
}
