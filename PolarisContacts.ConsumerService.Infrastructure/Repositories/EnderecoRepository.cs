﻿using Dapper;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Domain;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Infrastructure.Repositories
{
    public class EnderecoRepository(IDatabaseConnection dbConnection) : IEnderecoRepository
    {
        private readonly IDatabaseConnection _dbConnection = dbConnection;

        public async Task<int> Add(Endereco endereco, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            connection ??= _dbConnection.AbrirConexao();

            string query;

            var isSqlServer = connection is SqlConnection;
            if (isSqlServer)
            {
                // SQL Server
                query = @"INSERT INTO Enderecos (IdContato, Logradouro, Numero, Cidade, Estado, Bairro, Complemento, CEP, Ativo) 
                             OUTPUT INSERTED.Id
                             VALUES (@IdContato, @Logradouro, @Numero, @Cidade, @Estado, @Bairro, @Complemento, @CEP, @Ativo)";
            }
            else
            {
                // SQLite
                query = @"INSERT INTO Enderecos (IdContato, Logradouro, Numero, Cidade, Estado, Bairro, Complemento, CEP, Ativo) 
                            VALUES (@IdContato, @Logradouro, @Numero, @Cidade, @Estado, @Bairro, @Complemento, @CEP, @Ativo);
                            SELECT last_insert_rowid();";
            }

            return await connection.QuerySingleAsync<int>(query, endereco, transaction);

        }

        public async Task<bool> Update(Endereco endereco)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = @"UPDATE Enderecos SET 
                                Logradouro = @Logradouro, Numero = @Numero, Cidade = @Cidade, Estado = @Estado, 
                                Bairro = @Bairro, Complemento = @Complemento, CEP = @CEP 
                                WHERE Id = @Id";

            return await conn.ExecuteAsync(query, endereco) > 0;

        }

        public async Task<bool> Inactivate(int id)
        {
            using IDbConnection conn = _dbConnection.AbrirConexao();

            string query = @"UPDATE Enderecos SET 
                             Ativo = 0
                             WHERE Id = @Id";

            return await conn.ExecuteAsync(query, new { Id = id }) > 0;

        }
    }
}
