using PolarisContacts.ConsumerService.Domain;
using System.Data;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Repositories
{
    public interface ICelularRepository
    {
        Task<int> Add(Celular celular, IDbConnection connection = null, IDbTransaction transaction = null);
        Task<bool> Update(Celular celular);
        Task<bool> Inactivate(int id);
    }
}
