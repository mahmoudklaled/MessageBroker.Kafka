using Confluent.Kafka;
using MessageBroker.Kafka.Contract.Models;
using Newtonsoft.Json;
using System.Text;

namespace MessageBroker.Kafka.Contract.Serializer
{
    public class MessageDeserializer<T> : IDeserializer<Message<T>>
    {
        public Message<T> Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            var json = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<Message<T>>(json);
        }
    }
}
