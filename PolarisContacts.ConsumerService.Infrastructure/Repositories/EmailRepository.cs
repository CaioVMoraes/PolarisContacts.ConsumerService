using Dapper;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Infrastructure.Repositories
{
    public class EmailRepository(IDatabaseConnection dbConnection) : IEmailRepository
    {
        private readonly IDatabaseConnection _dbConnection = dbConnection;

        public async Task<int> Add(Email email, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            connection ??= _dbConnection.AbrirConexao();

            string query;

            var isSqlServer = connection is SqlConnection;
            if (isSqlServer)
            {
                // SQL Server
                query = @"INSERT INTO Emails (IdContato, EnderecoEmail, Ativo) 
                             OUTPUT INSERTED.Id
                             VALUES (@IdContato, @EnderecoEmail, @Ativo)";
            }
            else
            {
                // SQLite
                query = @"INSERT INTO Emails (IdContato, EnderecoEmail, Ativo) 
                            VALUES (@IdContato, @EnderecoEmail, @Ativo);
                            SELECT last_insert_rowid();";
            }

            return await connection.QuerySingleAsync<int>(query, email, transaction);
        }

        public async Task<bool> Update(Email email)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = @"UPDATE Emails SET 
                             EnderecoEmail = @EnderecoEmail
                             WHERE Id = @Id";
            return await conn.ExecuteAsync(query, email) > 0;
        }

        public async Task<bool> Inactivate(int id)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = @"UPDATE Emails SET 
                             Ativo = 0
                             WHERE Id = @Id";
            return await conn.ExecuteAsync(query, new { Id = id }) > 0;
        }
    }
}
