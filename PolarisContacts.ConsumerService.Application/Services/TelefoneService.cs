using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;

namespace PolarisContacts.ConsumerService.Application.Services
{
    public class TelefoneService(ITelefoneRepository telefoneRepository) : ITelefoneService
    {
        private readonly ITelefoneRepository _telefoneRepository = telefoneRepository;

        public void ProcessTelefone(EntityMessage message)
        {
            var telefone = JsonConvert.DeserializeObject<Telefone>(message.EntityData.ToString());

            switch (message.Operation)
            {
                case OperationType.Create:
                    _telefoneRepository.Add(telefone);
                    break;
                case OperationType.Update:
                    _telefoneRepository.Update(telefone);
                    break;
                case OperationType.Inactivate:
                    _telefoneRepository.Inactivate(telefone.Id);
                    break;
            }
        }
    }
}