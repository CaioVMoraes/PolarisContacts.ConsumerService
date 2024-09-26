using PolarisContacts.ConsumerService.Domain;
using System.Data;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Repositories
{
    public interface IContatoRepository
    {
        Task<int> Add(Contato contato, IDbConnection connection = null, IDbTransaction transaction = null);
        Task<bool> Update(Contato contato);
        Task<bool> Inactivate(int idContato);
    }
}