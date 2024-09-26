using PolarisContacts.ConsumerService.Domain;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Services
{
    public interface ICelularService
    {
        void ProcessCelular(EntityMessage message);
    }
}
