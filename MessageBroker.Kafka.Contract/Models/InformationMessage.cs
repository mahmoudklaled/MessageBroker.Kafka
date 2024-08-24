using MessageBroker.Kafka.Contract.Enums;

namespace MessageBroker.Kafka.Contract.Models
{
    public sealed class InformationMessage : Message
    {
        public string Message { get; set; }
        public InformationLevel  Level { get; set; }
        public InformationMessage()
        {
            MessageType = nameof(InformationMessage);
        }
    }
}
