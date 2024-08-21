using MessageBroker.Kafka.Contract.Enums;

namespace MessageBroker.Kafka.Contract.Models
{
    public class InformationMessage
    {
        public string Message { get; set; }
        public InformationLevel  Level { get; set; }
}
}
