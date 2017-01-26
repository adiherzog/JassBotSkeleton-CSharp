using JassBot.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JassBotTest.Model
{
    [TestClass]
    public class CardTest
    {
        [TestMethod]
        public void compareTo_sixToSeven_sevenIsBigger()
        {
            var six = new Card(Suit.CLUBS, 6);
            var seven = new Card(Suit.CLUBS, 7);

            var compareResult = six.CompareTo(seven);
            Assert.AreEqual(-1, compareResult);
        }

        [TestMethod]
        public void equals_forTwoCardsWithSameSuitAndNumber_returnsTrue()
        {
            var six = new Card(Suit.CLUBS, 6);
            var six2 = new Card(Suit.CLUBS, 6);

            Assert.AreEqual(six, six2);
        }

        [TestMethod]
        public void BeatsCard_trumpf_withNonTrumpfCards()
        {
            var lowerCard = new Card(Suit.CLUBS, 6);
            var higherCard = new Card(Suit.CLUBS, 8);
            var trumpf = new Trumpf(TrumpfMode.TRUMPF, Suit.HEARTS);

            Assert.IsTrue(higherCard.BeatsCard(lowerCard, trumpf));
            Assert.IsFalse(lowerCard.BeatsCard(higherCard, trumpf));
        }

        [TestMethod]
        public void BeatsCard_trumpf_withOneTrumpfCard()
        {
            var trumpfCard = new Card(Suit.CLUBS, 6);
            var nonTrumpfCard = new Card(Suit.SPADES, 8);
            var trumpf = new Trumpf(TrumpfMode.TRUMPF, Suit.CLUBS);

            Assert.IsTrue(trumpfCard.BeatsCard(nonTrumpfCard, trumpf));
            Assert.IsFalse(nonTrumpfCard.BeatsCard(trumpfCard, trumpf));
        }

        [TestMethod]
        public void BeatsCard_trumpf_withTwoTrumpfCardOneOfThemJack()
        {
            var higherTrumpf = new Card(Suit.CLUBS, 11);
            var lowerTrumpf = new Card(Suit.CLUBS, 14);
            var trumpf = new Trumpf(TrumpfMode.TRUMPF, Suit.CLUBS);

            Assert.IsTrue(higherTrumpf.BeatsCard(lowerTrumpf, trumpf));
            Assert.IsFalse(lowerTrumpf.BeatsCard(higherTrumpf, trumpf));
        }

        [TestMethod]
        public void BeatsCard_trumpf_withTwoTrumpfCardOneOfThemNine()
        {
            var higherTrumpf = new Card(Suit.CLUBS, 9);
            var lowerTrumpf = new Card(Suit.CLUBS, 14);
            var trumpf = new Trumpf(TrumpfMode.TRUMPF, Suit.CLUBS);

            Assert.IsTrue(higherTrumpf.BeatsCard(lowerTrumpf, trumpf));
            Assert.IsFalse(lowerTrumpf.BeatsCard(higherTrumpf, trumpf));
        }

        [TestMethod]
        public void BeatsCard_forUndeufe()
        {
            var lowerCard = new Card(Suit.CLUBS, 6);
            var higherCard = new Card(Suit.CLUBS, 8);
            var trumpf = new Trumpf(TrumpfMode.UNDEUFE);

            Assert.IsTrue(lowerCard.BeatsCard(higherCard, trumpf));
            Assert.IsFalse(higherCard.BeatsCard(lowerCard, trumpf));
        }

        [TestMethod]
        public void BeatsCard_forObeabe()
        {
            var lowerCard = new Card(Suit.CLUBS, 6);
            var higherCard = new Card(Suit.CLUBS, 8);
            var trumpf = new Trumpf(TrumpfMode.OBEABE);

            Assert.IsTrue(higherCard.BeatsCard(lowerCard, trumpf));
            Assert.IsFalse(lowerCard.BeatsCard(higherCard, trumpf));
        }

    }
}
