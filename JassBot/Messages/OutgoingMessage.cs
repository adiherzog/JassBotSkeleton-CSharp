using JassBot.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JassBot.Messages
{
    public class OutgoingMessage
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "type")]
        public MessageType Type { get; }
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        public OutgoingMessage(MessageType type)
        {
            Type = type;
        }
    }
}
