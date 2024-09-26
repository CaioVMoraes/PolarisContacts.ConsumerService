using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.CrossCutting.Helpers;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static PolarisContacts.ConsumerService.CrossCutting.Helpers.Exceptions.CustomExceptions;

namespace PolarisContacts.ConsumerService.Application.Services
{
    public class ContatoService(IDatabaseConnection dbConnection,
                                IContatoRepository contatoRepository,
                                ITelefoneRepository telefoneRepository,
                                ICelularRepository celularRepository,
                                IEmailRepository emailRepository,
                                IEnderecoRepository enderecoRepository) : IContatoService
    {
        private readonly IDatabaseConnection _dbConnection = dbConnection;
        private readonly IContatoRepository _contatoRepository = contatoRepository;
        private readonly ITelefoneRepository _telefoneRepository = telefoneRepository;
        private readonly ICelularRepository _celularRepository = celularRepository;
        private readonly IEmailRepository _emailRepository = emailRepository;
        private readonly IEnderecoRepository _enderecoRepository = enderecoRepository;

        public async Task<bool> ProcessContato(EntityMessage message)
        {
            var contato = JsonConvert.DeserializeObject<Contato>(message.EntityData.ToString());

            bool isSuccess = false;

            switch (message.Operation)
            {
                case OperationType.Create:
                    isSuccess = await ProcessAddContato(contato);
                    break;
                case OperationType.Update:
                    isSuccess = await _contatoRepository.Update(contato);
                    break;
                case OperationType.Inactivate:
                    isSuccess = await _contatoRepository.Inactivate(contato.Id);
                    break;
            }

            return isSuccess;
        }

        private async Task<bool> ProcessAddContato(Contato contato)
        {
            if (contato.IdUsuario <= 0)
            {
                throw new UsuarioNotFoundException();
            }
            if (string.IsNullOrEmpty(contato.Nome))
            {
                throw new NomeObrigatorioException();
            }

            bool isSucesso = false;
            using IDbConnection connection = _dbConnection.AbrirConexao();

            IDbTransaction transaction = null;
            try
            {
                if (connection is SqlConnection)
                {
                    // Apenas inicie uma transação se estiver usando SQL Server
                    transaction = connection.BeginTransaction();
                }

                contato.Ativo = true;
                contato.Id = await _contatoRepository.Add(contato, connection, transaction);

                if (contato.Telefones is not null)
                {
                    foreach (var telefone in contato.Telefones)
                    {
                        if (!Validacoes.IsValidTelefone(telefone.NumeroTelefone))
                            throw new TelefoneInvalidoException();

                        telefone.IdContato = contato.Id;
                        telefone.Ativo = true;

                        telefone.Id = await _telefoneRepository.Add(telefone, connection, transaction);
                    }
                }

                if (contato.Celulares is not null)
                {
                    foreach (var celular in contato.Celulares)
                    {
                        if (!Validacoes.IsValidCelular(celular.NumeroCelular))
                            throw new CelularInvalidoException();

                        celular.IdContato = contato.Id;
                        celular.Ativo = true;

                        celular.Id = await _celularRepository.Add(celular, connection, transaction);
                    }
                }

                if (contato.Emails is not null)
                {
                    foreach (var email in contato.Emails)
                    {
                        if (!Validacoes.IsValidEmail(email.EnderecoEmail))
                            throw new EmailInvalidoException();

                        email.IdContato = contato.Id;
                        email.Ativo = true;

                        email.Id = await _emailRepository.Add(email, connection, transaction);
                    }
                }

                if (contato.Enderecos is not null)
                {
                    foreach (var endereco in contato.Enderecos)
                    {
                        if (!Validacoes.IsValidEndereco(endereco))
                            throw new EnderecoInvalidoException();

                        endereco.IdContato = contato.Id;
                        endereco.Ativo = true;

                        endereco.Id = await _enderecoRepository.Add(endereco, connection, transaction);
                    }
                }

                if (transaction != null)
                {
                    transaction.Commit();
                }

                isSucesso = true;
                return isSucesso;
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}