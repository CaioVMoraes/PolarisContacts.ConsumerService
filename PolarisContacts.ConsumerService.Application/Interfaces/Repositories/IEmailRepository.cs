using PolarisContacts.ConsumerService.Domain;
using System.Data;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Repositories
{
    public interface IEmailRepository
    {
        Task<int> Add(Email email, IDbConnection connection = null, IDbTransaction transaction = null);
        Task<bool> Update(Email email);
        Task<bool> Inactivate(int id);
    }
}
