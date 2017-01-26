using System.Collections.Generic;
using JassBot.Model;
using JassBot.Util;

namespace JassBot.JassLogic
{
    public class TrumpfChooser
    {
        public Trumpf ChooseTrumpf(ISet<Card> myCards, bool schiebenAllowed)
        {
            Preconditions.CheckNotNull(myCards);
            Preconditions.CheckArgument(myCards.Count == 9);
            foreach (var card in myCards)
            {
                Preconditions.CheckNotNull(card);
                Preconditions.CheckNotNull(card.Suit);
                Preconditions.CheckArgument(card.Number >= 6);
                Preconditions.CheckArgument(card.Number <= 14);
            }

            return new Trumpf(TrumpfMode.OBEABE);
        }
    }
}
