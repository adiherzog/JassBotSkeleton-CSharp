using Newtonsoft.Json;
// ReSharper disable InconsistentNaming
#pragma warning disable 649

namespace JassBot.Model
{
    public class Stich
    {
        [JsonProperty(PropertyName = "name")]
        private string Name;
        [JsonProperty(PropertyName = "id")]
        private string Id;
        [JsonProperty(PropertyName = "playedCards")]
        private Card[] PlayedCards;
        [JsonProperty(PropertyName = "teams")]
        private Team[] Teams;

        public override string ToString()
        {
            return "Stich{" +
                "name='" + Name + '\'' +
                ", id=" + Id +
                ", playedCards=" + string.Join<Card>(",", PlayedCards) +
                ", teams=" + string.Join<Team>(",", Teams) +
                '}';
        }
    }
}
