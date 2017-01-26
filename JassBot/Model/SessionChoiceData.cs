using JassBot.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JassBot.Model
{
    public class SessionChoiceData
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "sessionChoice")]
        public SessionChoiceType SessionChoice { get; set; }
        [JsonProperty(PropertyName = "sessionName")]
        public string SessionName { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "sessionType")]
        public SessionType SessionType { get; set; }

        public SessionChoiceData(SessionChoiceType sessionChoice, string sessionName, SessionType sessionType)
        {
            Preconditions.CheckNotNull(sessionChoice);
            Preconditions.CheckNotNull(sessionType);
            SessionChoice = sessionChoice;
            SessionName = sessionName;
            SessionType = sessionType;
        }
    }
}
