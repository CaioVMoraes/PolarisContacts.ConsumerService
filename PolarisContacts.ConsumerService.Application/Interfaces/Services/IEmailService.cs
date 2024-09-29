using PolarisContacts.ConsumerService.Domain;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task ProcessEmail(EntityMessage message);
    }
}
