using Newtonsoft.Json;
using PolarisContacts.ConsumerService.Application.Interfaces.Repositories;
using PolarisContacts.ConsumerService.Application.Interfaces.Services;
using PolarisContacts.ConsumerService.Domain;
using PolarisContacts.ConsumerService.Domain.Enuns;

namespace PolarisContacts.ConsumerService.Application.Services
{
    public class EmailService(IEmailRepository emailRepository) : IEmailService
    {
        private readonly IEmailRepository _emailRepository = emailRepository;

        public void ProcessEmail(EntityMessage message)
        {
            var email = JsonConvert.DeserializeObject<Email>(message.EntityData.ToString());

            switch (message.Operation)
            {
                case OperationType.Create:
                    _emailRepository.Add(email);
                    break;
                case OperationType.Update:
                    _emailRepository.Update(email);
                    break;
                case OperationType.Inactivate:
                    _emailRepository.Inactivate(email.Id);
                    break;
            }
        }
    }
}