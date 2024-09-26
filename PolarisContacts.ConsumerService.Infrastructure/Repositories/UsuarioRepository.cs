using Dapper;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain;
using System.Data;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Infrastructure.Repositories
{
    public class UsuarioRepository(IDatabaseConnection dbConnection) : IUsuarioRepository
    {
        private readonly IDatabaseConnection _dbConnection = dbConnection;

        public async Task<bool> Add(Usuario usuario)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = "INSERT INTO Usuarios ([Login], Senha, Ativo) VALUES (@Login, @Senha, 1)";

            return await conn.ExecuteAsync(query, new { Login = usuario.Login, Senha = usuario.Senha }) > 0;
        }

        public async Task<bool> ChangeUserPassword(Usuario usuario)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = "UPDATE Usuarios SET Senha = @NewPassword WHERE [Login] = @Login AND Senha = @OldPassword AND Ativo = 1";

            return await conn.ExecuteAsync(query, new { Login = usuario.Login, OldPassword = usuario.Senha, NewPassword = usuario.NovaSenha }) > 0;
        }
    }
}
