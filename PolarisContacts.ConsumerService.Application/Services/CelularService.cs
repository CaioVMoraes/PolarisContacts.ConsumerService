using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Application.Services
{
    public class CelularService(ICelularRepository celularRepository) : ICelularService
    {
        private readonly ICelularRepository _celularRepository = celularRepository;

        public async Task ProcessCelular(EntityMessage message)
        {
            var celular = JsonConvert.DeserializeObject<Celular>(message.EntityData.ToString());

            switch (message.Operation)
            {
                case OperationType.Create:
                    await _celularRepository.Add(celular);
                    break;
                case OperationType.Update:
                    await _celularRepository.Update(celular);
                    break;
                case OperationType.Inactivate:
                    await _celularRepository.Inactivate(celular.Id);
                    break;
            }
        }
    }
}