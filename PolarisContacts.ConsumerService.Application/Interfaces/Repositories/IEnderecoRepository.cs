using PolarisContacts.ConsumerService.Domain;
using System.Data;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Repositories
{
    public interface IEnderecoRepository
    {
        Task<int> Add(Endereco endereco, IDbConnection connection = null, IDbTransaction transaction = null);
        Task<bool> Update(Endereco endereco);
        Task<bool> Inactivate(int id);
    }
}