using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;

namespace PolarisContacts.ConsumerService.Application.Services
{
    public class CelularService(ICelularRepository celularRepository) : ICelularService
    {
        private readonly ICelularRepository _celularRepository = celularRepository;

        public void ProcessCelular(EntityMessage message)
        {
            var celular = JsonConvert.DeserializeObject<Celular>(message.EntityData.ToString());

            switch (message.Operation)
            {
                case OperationType.Create:
                    _celularRepository.Add(celular);
                    break;
                case OperationType.Update:
                    _celularRepository.Update(celular);
                    break;
                case OperationType.Inactivate:
                    _celularRepository.Inactivate(celular.Id);
                    break;
            }
        }
    }
}