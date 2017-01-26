using System.Collections.Generic;
using System.Linq;
using JassBot.JassLogic;
using JassBot.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JassBotTest.JassLogic
{
    [Ignore]
    [TestClass]
    public class CardChooserTest
    {
        private readonly CardChooser _cardChooser = new CardChooser();

        [TestMethod]
        public void FirstAllowedCardOnMyHandIsChosen()
        {
            var gameState = new GameState
            {
                MyCards = CreateMyCards(),
                CardsOnTable = NoCardsOnTable(),
                Trumpf = new Trumpf(TrumpfMode.TRUMPF, Suit.CLUBS)
            };

            var chosenCard = _cardChooser.ChooseCard(gameState);

            Assert.IsNotNull(chosenCard);
            Assert.AreEqual(gameState.MyCards.First(), chosenCard);
        }
        
        private IList<Card> NoCardsOnTable()
        {
            return new List<Card>();
        }
        
        private static ISet<Card> CreateMyCards()
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
