using System.Data;

namespace PolarisContacts.ConsumerService.Application.Interfaces.Repositories
{
    public interface IDatabaseConnection
    {
        public IDbConnection AbrirConexao();
    }
}

