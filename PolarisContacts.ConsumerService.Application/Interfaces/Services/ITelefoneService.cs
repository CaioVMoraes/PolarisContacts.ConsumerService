using PolarisContacts.ConsumerService.Domain;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Services
{
    public interface ITelefoneService
    {
        void ProcessTelefone(EntityMessage message);
    }
}