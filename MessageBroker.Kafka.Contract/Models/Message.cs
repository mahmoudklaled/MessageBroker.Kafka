namespace MessageBroker.Kafka.Contract.Models
{
    public sealed class Message<T>
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public long TimeStampUtc { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public T MessageDetails { get; set; }

        public Message(T MessageDetails)
        {
            this.MessageDetails = MessageDetails;
        }
    }
}
