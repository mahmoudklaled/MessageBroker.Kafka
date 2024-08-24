namespace MessageBroker.Kafka.Contract.Models
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public long TimeStampUtc { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public string MessageType { get;  set; }
       
    }
}
