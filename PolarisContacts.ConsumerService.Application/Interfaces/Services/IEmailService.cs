using PolarisContacts.ConsumerService.Domain;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Services
{
    public interface IEmailService
    {
        void ProcessEmail(EntityMessage message);
    }
}
