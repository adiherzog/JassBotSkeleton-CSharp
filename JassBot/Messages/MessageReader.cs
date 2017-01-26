using System;
using System.Collections.Generic;
using System.Linq;
using JassBot.Model;
using Newtonsoft.Json.Linq;

/**
 * Reads messages that come from the Jass server and converts them into Java Objects.
 */
namespace JassBot.Messages
{
    public class MessageReader
    {
        public IncommingMessage Read(string messageFromServer)
        {
            JObject messageAsJsonObject = JObject.Parse(messageFromServer);

            JToken typePrimitive = messageAsJsonObject["type"];
            if (typePrimitive == null)
            {
                throw new IllegalMessageException("Field \"type\" is missing.", messageFromServer);
            }
            var type = typePrimitive.Value<string>();
            MessageType messageType;
            if (!Enum.TryParse(type, out messageType))
            {
                throw new ArgumentException("Can't parse type " + type);
            }
            IncommingMessage message = new IncommingMessage(messageType);
            AddFurtherMessageFields(message, messageAsJsonObject);
            return message;
        }

        private void AddFurtherMessageFields(IncommingMessage message, JObject messageAsJsonObject)
        {
            JToken data = messageAsJsonObject["data"];

            switch (message.Type)
            {
                case MessageType.DEAL_CARDS:
                    message.Cards = GetCardsFromMessage(data);
                    break;
                case MessageType.REQUEST_TRUMPF:
                    message.SchiebenAllowed = GetIsSchiebenAllowed(data);
                    break;
                case MessageType.BROADCAST_TRUMPF:
                    message.Trumpf = GetTrumpf(data);
                    break;
                case MessageType.REQUEST_CARD:
                    message.CardsOnTable = GetCardsFromMessageAsList(data);
                    break;
                case MessageType.BROADCAST_STICH:
                    message.Stich = GetStichFromMessage(data);
                    break;
            }
        }

        private static Stich GetStichFromMessage(JToken data)
        {
            return data.ToObject<Stich>();
        }

        private static bool GetIsSchiebenAllowed(JToken data)
        {
            return !data.ToObject<bool>();
        }

        private static ISet<Card> GetCardsFromMessage(JToken data)
        {
            var cardSet = new HashSet<Card>();
            foreach (var cardToken in data.Children().ToList())
            {
                var card = cardToken.ToObject<Card>();
                cardSet.Add(card);
            }
            return cardSet;
        }

        private static IList<Card> GetCardsFromMessageAsList(JToken data)
        {
            return data.Children().ToList().Select(cardToken => cardToken.ToObject<Card>()).ToList();
        }

        private static Trumpf GetTrumpf(JToken data)
        {
            return data.ToObject<Trumpf>();
        }
    }
}
