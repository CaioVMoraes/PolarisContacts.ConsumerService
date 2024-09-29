using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Application.Services
{
    public class EnderecoService(IEnderecoRepository enderecoRepository) : IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository = enderecoRepository;
        public async Task ProcessEndereco(EntityMessage message)
        {
            var endereco = JsonConvert.DeserializeObject<Endereco>(message.EntityData.ToString());

            switch (message.Operation)
            {
                case OperationType.Create:
                    await _enderecoRepository.Add(endereco);
                    break;
                case OperationType.Update:
                    await _enderecoRepository.Update(endereco);
                    break;
                case OperationType.Inactivate:
                    await _enderecoRepository.Inactivate(endereco.Id);
                    break;
            }
        }
    }
}