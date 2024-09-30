using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;
using System.Threading.Tasks;

namespace PolarisContacts.ConsumerService.Application.Services
{
    public class EmailService(IEmailRepository emailRepository) : IEmailService
    {
        private readonly IEmailRepository _emailRepository = emailRepository;

        public async Task ProcessEmail(EntityMessage message)
        {
            var email = JsonConvert.DeserializeObject<Email>(message.EntityData.ToString());

            switch (message.Operation)
            {
                case OperationType.Create:
                    await _emailRepository.Add(email); 
                    break;
                case OperationType.Update:
                    await _emailRepository.Update(email); 
                    break;
                case OperationType.Inactivate:
                    await _emailRepository.Inactivate(email.Id); 
                    break;
            }
        }
    }
}