using PolarisContacts.ConsumerService.Domain.Enuns;

namespace PolarisContacts.ConsumerService.Domain
{
    public class EntityMessage
    {
        public OperationType Operation { get; set; }
        public EntityType EntityType { get; set; }
        public object EntityData { get; set; }
    }
}
