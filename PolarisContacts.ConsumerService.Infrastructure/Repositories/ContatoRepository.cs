using Dapper;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Infrastructure.Repositories
{
    public class ContatoRepository(IDatabaseConnection dbConnection) : IContatoRepository
    {
        private readonly IDatabaseConnection _dbConnection = dbConnection;

        public async Task<int> Add(Contato contato, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            connection ??= _dbConnection.AbrirConexao();

            string query;

            var isSqlServer = connection is SqlConnection;
            if (isSqlServer)
            {
                // SQL Server
                query = @"INSERT INTO Contatos (Nome, IdUsuario, Ativo)
                          OUTPUT INSERTED.Id
                          VALUES (@Nome, @IdUsuario, @Ativo)";
            }
            else
            {
                // SQLite
                query = @"INSERT INTO Contatos (Nome, IdUsuario, Ativo)
                          VALUES (@Nome, @IdUsuario, @Ativo);
                          SELECT last_insert_rowid();";
            }

            return await connection.QuerySingleAsync<int>(query, contato, transaction);
        }


        public async Task<bool> Update(Contato contato)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = @"UPDATE Contatos SET 
                             Nome = @Nome, Ativo = @Ativo 
                             WHERE Id = @Id";
            return await conn.ExecuteAsync(query, contato) > 0;
        }

        public async Task<bool> Inactivate(int idContato)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = @"UPDATE Contatos SET 
                             Ativo = 0
                             WHERE Id = @Id";
            return await conn.ExecuteAsync(query, new { Id = idContato }) > 0;
        }
    }
}