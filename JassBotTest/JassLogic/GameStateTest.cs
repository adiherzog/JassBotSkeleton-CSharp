using System.Collections.Generic;
using System.Linq;
using JassBot.JassLogic;
using JassBot.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JassBotTest.JassLogic
{
    [TestClass]
    public class GameStateTest
    {
        private GameState _gameState ;

        [TestMethod]
        public void trumpf_noCardsOnTable_allCardsAllowed()
        {
            GivenDefaultGameState();
            _gameState.CardsOnTable = new List<Card>();

            Assert.AreEqual(9, _gameState.GetAllowedCardsToPlay().ToList().Count);
        }

        [TestMethod]
        public void undenufe_withNoMatchingSuitInMyHand_allCardsAllowed()
        {
            GivenUndeufeAndOneCardPlayedThatDoesNotMatchMyCards();

            Assert.AreEqual(9, _gameState.GetAllowedCardsToPlay().ToList().Count);
        }
        
        [TestMethod]
        public void undenufe_withMatchingSuitInMyHand_onlyMatchingCardsAllowed()
        {
            GivenUndeufeAndOneCardPlayedThatDoesNotMatchMyCards();
            _gameState.CardsOnTable = new List<Card>
            {
                new Card(Suit.CLUBS, 9)
            };

            _gameState.GetAllowedCardsToPlay().ToList().ForEach(x => Assert.AreEqual(Suit.CLUBS, x.Suit));
            Assert.AreEqual(4, _gameState.GetAllowedCardsToPlay().ToList().Count);
        }

        private void GivenUndeufeAndOneCardPlayedThatDoesNotMatchMyCards()
        {
            GivenDefaultGameState();
            _gameState.Trumpf = new Trumpf(TrumpfMode.UNDEUFE);
        }

        private void GivenDefaultGameState()
        {
            _gameState = new GameState
            {
                CardsOnTable = new List<Card>
                {
                    new Card(Suit.DIAMONDS, 9)
                },
                DidNotSchiebe = false,
                MyCards = CreateMyCards(),
                Round = 0,
                Trumpf = new Trumpf(TrumpfMode.TRUMPF, Suit.CLUBS)
            };
        }

        private static HashSet<Card> CreateMyCards()
        {
            return new HashSet<Card>
            {
                new Card(Suit.CLUBS, 11),
                new Card(Suit.CLUBS, 6),
                new Card(Suit.CLUBS, 7),
                new Card(Suit.CLUBS, 8),
                new Card(Suit.SPADES, 7),
                new Card(Suit.SPADES, 8),
                new Card(Suit.HEARTS, 12),
                new Card(Suit.HEARTS, 13),
                new Card(Suit.HEARTS, 14)
            };
        }
    }
}
