using MessageBroker.Kafka.Contract.Models;
using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace MessageBroker.Kafka.Contract.Serializer
{
    public class MessageSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            var json = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
