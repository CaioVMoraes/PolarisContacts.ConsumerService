using PolarisContacts.ConsumerService.Domain;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<bool> Add(Usuario usuario);
        Task<bool> ChangeUserPassword(Usuario usuario);
    }
}