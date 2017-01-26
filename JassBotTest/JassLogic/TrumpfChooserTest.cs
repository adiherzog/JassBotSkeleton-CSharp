using System.Collections.Generic;
using JassBot.JassLogic;
using JassBot.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JassBotTest.JassLogic
{
    [TestClass]
    public class TrumpfChooserTest
    {
        [TestMethod]
        public void TrumpfIsAlwaysObenabe()
        {
            var cards = CreateCardsWithJackNineAndThirdCard();

            var trumpf = new TrumpfChooser().ChooseTrumpf(cards, true);

            Assert.IsNotNull(trumpf);
            Assert.AreEqual(TrumpfMode.OBEABE, trumpf.Mode);
        }

        private ISet<Card> CreateCardsWithJackNineAndThirdCard()
        {
            return new HashSet<Card>
            {
                new Card(Suit.HEARTS, 9),
                new Card(Suit.HEARTS, 11),
                new Card(Suit.HEARTS, 7),
                new Card(Suit.CLUBS, 6),
                new Card(Suit.CLUBS, 7),
                new Card(Suit.CLUBS, 8),
                new Card(Suit.DIAMONDS, 12),
                new Card(Suit.DIAMONDS, 13),
                new Card(Suit.DIAMONDS, 14)
            };
        }
    }
}
