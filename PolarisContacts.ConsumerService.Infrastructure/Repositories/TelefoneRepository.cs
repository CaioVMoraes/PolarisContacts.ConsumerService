using Dapper;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Infrastructure.Repositories
{
    public class TelefoneRepository(IDatabaseConnection dbConnection) : ITelefoneRepository
    {
        private readonly IDatabaseConnection _dbConnection = dbConnection;

        public async Task<int> Add(Telefone telefone, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            connection ??= _dbConnection.AbrirConexao();

            string query;

            var isSqlServer = connection is SqlConnection;
            if (isSqlServer)
            {
                // SQL Server
                query = @"INSERT INTO Telefones (IdRegiao, IdContato, NumeroTelefone, Ativo) 
                             OUTPUT INSERTED.Id
                             VALUES (@IdRegiao, @IdContato, @NumeroTelefone, @Ativo)";
            }
            else
            {
                // SQLite
                query = @"INSERT INTO Telefones (IdRegiao, IdContato, NumeroTelefone, Ativo) 
                            VALUES (@IdRegiao, @IdContato, @NumeroTelefone, @Ativo);
                            SELECT last_insert_rowid();";
            }

            return await connection.QuerySingleAsync<int>(query, telefone, transaction);
        }

        public async Task<bool> Update(Telefone telefone)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = @"UPDATE Telefones SET 
                             IdRegiao = @IdRegiao, NumeroTelefone = @NumeroTelefone
                             WHERE Id = @Id";
            return await conn.ExecuteAsync(query, telefone) > 0;
        }

        public async Task<bool> Inactivate(int id)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = @"UPDATE Telefones SET 
                             Ativo = 0
                             WHERE Id = @Id";
            return await conn.ExecuteAsync(query, new { Id = id }) > 0;
        }
    }
}