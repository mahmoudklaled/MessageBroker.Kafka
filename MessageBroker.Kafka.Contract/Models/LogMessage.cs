namespace MessageBroker.Kafka.Contract.Models
{
    public sealed class LogMessage : Message
    {
        public string Message { get; set; }

        public LogMessage()
        {
            MessageType = nameof(LogMessage);
        }
    }
}
