using PolarisContacts.ConsumerService.Domain;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Services
{
    public interface IEnderecoService
    {
        void ProcessEndereco(EntityMessage message);
    }
}