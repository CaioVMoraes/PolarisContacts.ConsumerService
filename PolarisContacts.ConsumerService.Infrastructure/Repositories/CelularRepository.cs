using Dapper;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Infrastructure.Repositories
{
    public class CelularRepository(IDatabaseConnection dbConnection) : ICelularRepository
    {
        private readonly IDatabaseConnection _dbConnection = dbConnection;

        public async Task<int> Add(Celular celular, IDbConnection connection, IDbTransaction transaction)
        {
            string query;

            var isSqlServer = connection.GetType() == typeof(SqlConnection);

            if (isSqlServer)
            {
                // SQL Server
                query = @"INSERT INTO Celulares (IdRegiao, IdContato, NumeroCelular, Ativo) 
                             OUTPUT INSERTED.Id
                             VALUES (@IdRegiao, @IdContato, @NumeroCelular, @Ativo)";
            }
            else
            {
                // SQLite
                query = @"INSERT INTO Celulares (IdRegiao, IdContato, NumeroCelular, Ativo) 
                            VALUES (@IdRegiao, @IdContato, @NumeroCelular, @Ativo);
                            SELECT last_insert_rowid();";
            }

            return await connection.QuerySingleAsync<int>(query, celular, transaction);
        }

        public async Task<bool> Update(Celular celular)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = @"UPDATE Celulares SET 
                             IdRegiao = @IdRegiao, NumeroCelular = @NumeroCelular
                             WHERE Id = @Id";
            return await conn.ExecuteAsync(query, celular) > 0;
        }

        public async Task<bool> Inactivate(int id)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = @"UPDATE Celulares SET 
                             Ativo = 0
                             WHERE Id = @Id";
            return await conn.ExecuteAsync(query, new { Id = id }) > 0;
        }
    }
}
