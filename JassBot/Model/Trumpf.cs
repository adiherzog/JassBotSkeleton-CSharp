using System.Text;
using JassBot.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JassBot.Model
{
    public class Trumpf
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "mode")]
        public TrumpfMode Mode { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "trumpfColor")]
        public Suit? Suit { get; }

        /**
         * Must not be used for TrumpfMode.TRUMPF!
         */
        public Trumpf(TrumpfMode mode)
        {
            Preconditions.CheckNotNull(mode);
            Preconditions.CheckArgument(!TrumpfMode.TRUMPF.Equals(mode));
            Mode = mode;
            Suit = null;
        }

        /**
         * Use this for TrumpfMode.TRUMPF only!
         */
         [JsonConstructor]
        public Trumpf(TrumpfMode mode, Suit suit)
        {
            Preconditions.CheckNotNull(mode);
            Mode = mode;
            Suit = suit;
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Mode);
            if (TrumpfMode.TRUMPF.Equals(Mode))
            {
                builder.Append(" ");
                builder.Append(Suit);
            }

            return builder.ToString();
        }

        public bool IsObenabeOrUndeufe()
        {
            return TrumpfMode.OBEABE.Equals(Mode) || TrumpfMode.UNDEUFE.Equals(Mode);
        }
    }
}
