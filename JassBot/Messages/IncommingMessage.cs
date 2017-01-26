using System.Collections.Generic;
using JassBot.Model;

/**
 * A message comming from the server.
 */
namespace JassBot.Messages
{
    public class IncommingMessage
    {
        public MessageType Type { get; set; }
        public ISet<Card> Cards { get; set; }
        // Only relevant for MessageType.REQUEST_TRUMPF
        public bool SchiebenAllowed { get; set; }
        public Trumpf Trumpf { get; set; }
        public IList<Card> CardsOnTable { get; set; }
        public Stich Stich { get; set; }

        public IncommingMessage(MessageType type)
        {
            Type = type;
        }
    }
}
