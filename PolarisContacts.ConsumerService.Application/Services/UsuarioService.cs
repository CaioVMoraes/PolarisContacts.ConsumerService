using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;

namespace PolarisContacts.ConsumerService.Application.Services
{
    public class UsuarioService(IUsuarioRepository usuarioRepository) : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        public void ProcessUsuario(EntityMessage message)
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(message.EntityData.ToString());

            switch (message.Operation)
            {
                case OperationType.Create:
                    _usuarioRepository.Add(usuario);
                    break;
                case OperationType.Update:
                    _usuarioRepository.ChangeUserPassword(usuario);
                    break;
                case OperationType.Inactivate:
                    throw new System.NotImplementedException();
                    break;
            }
        }
    }
}

