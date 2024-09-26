using PolarisContacts.ConsumerService.Domain;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Services
{
    public interface IUsuarioService
    {
        void ProcessUsuario(EntityMessage message);
    }
}