namespace MessageBroker.Kafka.Contract.Models
{
    public sealed class ExceptionMessage : Message
    {
        public string Message { get; set; }
        public ExceptionMessage()
        {
            MessageType = nameof(ExceptionMessage);
        }
    }
}
