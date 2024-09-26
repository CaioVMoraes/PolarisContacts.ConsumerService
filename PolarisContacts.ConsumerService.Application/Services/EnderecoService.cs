using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;

namespace PolarisContacts.ConsumerService.Application.Services
{
    public class EnderecoService(IEnderecoRepository enderecoRepository) : IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository = enderecoRepository;

        public void ProcessEndereco(EntityMessage message)
        {
            var endereco = JsonConvert.DeserializeObject<Endereco>(message.EntityData.ToString());

            switch (message.Operation)
            {
                case OperationType.Create:
                    _enderecoRepository.Add(endereco);
                    break;
                case OperationType.Update:
                    _enderecoRepository.Update(endereco);
                    break;
                case OperationType.Inactivate:
                    _enderecoRepository.Inactivate(endereco.Id);
                    break;
            }
        }
    }
}