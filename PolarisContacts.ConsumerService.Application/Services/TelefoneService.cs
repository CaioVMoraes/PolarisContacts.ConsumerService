using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Application.Services
{
    public class TelefoneService(ITelefoneRepository telefoneRepository) : ITelefoneService
    {
        private readonly ITelefoneRepository _telefoneRepository = telefoneRepository;

        public async Task ProcessTelefone(EntityMessage message)
        {
            var telefone = JsonConvert.DeserializeObject<Telefone>(message.EntityData.ToString());

            switch (message.Operation)
            {
                case OperationType.Create:
                    await _telefoneRepository.Add(telefone);
                    break;
                case OperationType.Update:
                    await _telefoneRepository.Update(telefone);
                    break;
                case OperationType.Inactivate:
                    await _telefoneRepository.Inactivate(telefone.Id);
                    break;
            }
        }
    }
}