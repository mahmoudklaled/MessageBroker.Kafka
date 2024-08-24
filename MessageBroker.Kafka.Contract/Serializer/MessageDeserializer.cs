using Confluent.Kafka;
using MessageBroker.Kafka.Contract.Models;
using Newtonsoft.Json;
using System.Text;
using LogMessage = MessageBroker.Kafka.Contract.Models.LogMessage;

namespace MessageBroker.Kafka.Contract.Serializer
{
    public class MessageDeserializer<T> : IDeserializer<T> where T : Message
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return null; // Handle null data gracefully
            }

            try
            {
                var json = Encoding.UTF8.GetString(data);
                var baseMessage = JsonConvert.DeserializeObject<Message>(json);

                Message deserializedMessage = baseMessage.MessageType switch
                {
                    nameof(InformationMessage) => JsonConvert.DeserializeObject<InformationMessage>(json),
                    nameof(ExceptionMessage) => JsonConvert.DeserializeObject<ExceptionMessage>(json),
                    nameof(LogMessage) => JsonConvert.DeserializeObject<LogMessage>(json),
                    _ => throw new InvalidOperationException($"Unknown message type: {baseMessage.MessageType}")
                };

                return deserializedMessage as T;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Failed to deserialize message: {ex.Message}");
                throw; // Re-throw the exception after logging
            }
        }
    }
}
