using PolarisContacts.ConsumerService.Domain;
using System.Data;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Repositories
{
    public interface ITelefoneRepository
    {
        Task<int> Add(Telefone telefone, IDbConnection connection = null, IDbTransaction transaction = null);
        Task<bool> Update(Telefone telefone);
        Task<bool> Inactivate(int id);
    }
}