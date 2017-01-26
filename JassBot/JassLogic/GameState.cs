using System;
using System.Linq;
using System.Collections.Generic;
using JassBot.Model;
using JassBot.Util;

namespace JassBot.JassLogic
{
    public class GameState
    {
        public ISet<Card> MyCards { get; set; } = new HashSet<Card>();
        public Trumpf Trumpf { get; set; }
        public IList<Card> CardsOnTable { get; set; }
        public bool DidNotSchiebe { get; set; }
        public int Round { get; set; }

        public void StartNextRound()
        {
            Round++;
        }

        public void CheckIsValid()
        {
            Preconditions.CheckNotNull(CardsOnTable);
            Preconditions.CheckNotNull(MyCards);
            Preconditions.CheckNotNull(Trumpf);
            Preconditions.CheckArgument(Round >= 0 && Round <= 8);
        }

        /// <summary>
        /// Reset after one game round (when everybody played one full hand)
        /// </summary>
        public void ResetAfterGameRound()
        {
            MyCards = new HashSet<Card>();
            Trumpf = null;
            CardsOnTable = null;
            DidNotSchiebe = false;
            Round = 0;
        }

        /// <summary>
        /// Reset after an entire game ended (after one team has reached the target amount of points)
        /// </summary>
        public void ResetEverythingForNewGame()
        {
            ResetAfterGameRound();
        }

        public IEnumerable<Card> GetAllowedCardsToPlay()
        {
            // If there's not card on the table yet and I'm asked to play it means
            // that I am starting the round so I can play any card I want.
            if (CardsOnTable.Count == 0)
            {
                return MyCards;
            }

            return IsTrumpfGame() ? GetAllowedCardsForTrumpfGame() : GetAllowedCardsForNonTrumpfGame();
        }

        private IEnumerable<Card> GetAllowedCardsForTrumpfGame()
        {
            /*
             * Wer eine Karte der ausgespielten Farbe besitzt, muss diese Farbe spielen.
             * Die Ausnahme ist der Trumpf-Bauer/Trumpf-Under.
             */
            if (HaveAtLeastOneCardThatMatchesThePlayedSuit())
            {
                if (PlayedCardIsTrumpf())
                {
                    /*
                     * Untertrumpf-Regel trifft in diesem Fall nicht zu, da Trumpf ausgegeben wurde.
                     * Wurde die Trumpffarbe ausgespielt, dann muss ebenfalls eine Trumpfkarte gespielt werden.
                     * Wer neben dem Trumpf-Bauer/Trumpf-Under keine andere Trumpfkarte mehr besitzt, darf statt
                     * des Trumpf-Bauer/Trumpf-Under auch jede andere Karte spielen.
                     */
                    return TrumpfBauerIsMyOnlyTrumpf() ? MyCards : GetMyTrumpfs();
                }
                else
                {
                    return GetMyCardsThatMatchPlayedSuit().Concat(GetMyTrumpfs());
                }
            }
            else if (HaveOtherCardsThanTrumpf())
            {
                /*
                 * Wer keine Karte der ausgespielten Farbe hat, darf irgendeine Karte spielen
                 * (Untertrumpfen ist nur erlaubt, wenn man nur noch Trumpfkarten besitzen).
                 */
                return GetMyNonTrumpfCards().Concat(GetMyTrumpfsThatDoNotUndertrumpf());
            }
            else
            {
                // Untertrumpfen ist hier erlaubt, da ich nur noch Trumpf-Karten habe.
                // Wenn eine Karte in einer Nebenfarbe ausgespielt wurde und ein anderer Spieler bereits 
                // eine Trumpfkarte auf den Stich gespielt hat, dürfen Sie nicht eine Trumpfkarte mit
                // einem niedrigeren Wert spielen, es sei denn, Sie haben nur noch Karten in der Trumpffarbe
                // im Blatt, in welchem Fall sie jeden beliebigen Trumpf spielen können.
                return MyCards;
            }
        }

        private bool HaveAtLeastOneCardThatMatchesThePlayedSuit()
        {
            return GetMyCardsThatMatchPlayedSuit().Any();
        }

        private bool HaveOtherCardsThanTrumpf()
        {
            return GetMyNonTrumpfCards().Any();
        }

        private IEnumerable<Card> GetMyNonTrumpfCards()
        {
            return (from card in MyCards where !card.IsTrumpf(Trumpf) select card);
        }

        private IEnumerable<Card> GetAllowedCardsForNonTrumpfGame()
        {
            // If player has at least one matching card, they have to play a matching one.
            // Otherwise any card is fine.
            return GetMyCardsThatMatchPlayedSuit().ToList().Count > 0
                ? GetMyCardsThatMatchPlayedSuit()
                : MyCards;
        }

        private bool TrumpfBauerIsMyOnlyTrumpf()
        {
            var myTrumpfs = GetMyTrumpfs().ToList();
            return myTrumpfs.Count<Card>() == 1
                   && (from card in myTrumpfs where card.Number == 11 select card).Any();
        }

        public bool PlayedCardIsTrumpf()
        {
            return CardsOnTable.Count != 0 && CardsOnTable[0].Suit.Equals(Trumpf.Suit);
        }

        private IEnumerable<Card> GetMyCardsThatMatchPlayedSuit()
        {
            var suitOfFirstCard = CardsOnTable[0].Suit;
            return from card in MyCards where card.Suit.Equals(suitOfFirstCard) select card;
        }

        private IEnumerable<Card> GetMyTrumpfs()
        {
            return from card in MyCards where card.Suit.Equals(Trumpf.Suit) select card;
        }

        private IEnumerable<Card> GetMyTrumpfsThatDoNotUndertrumpf()
        {
            return from card in GetMyCardsThatCanBeatCurrentCardsOnTable() where card.IsTrumpf(Trumpf) select card;
        }

        private bool IsTrumpfGame()
        {
            return TrumpfMode.TRUMPF.Equals(Trumpf.Mode);
        }

        public IEnumerable<Card> GetSafeTricks()
        {
            if (CardsOnTable.Count == 3)
            {
                return GetMyAllowedCardsThatCanBeatCurrentCardsOnTable();
            }

            // TODO implement more logic

            return Enumerable.Empty<Card>();
        }

        /**
         * Returns the card on the table that would make the trick if the round would finish now
         */
        private Card GetStrongestCardOnTable()
        {
            var allowedCardsArray =  GetCardsOnTableThatAreAllowedToTrick().ToArray();
            Array.Sort(allowedCardsArray, Card.GetComperatorForTrumpf(Trumpf));

            return allowedCardsArray[allowedCardsArray.Length - 1];
        }

        /**
         * Only the cards that could actually trick if they were the highest. E.g. let's assume the first card is
         * SPADES and trumpf is HEARTS. Then HEARTS and SPADES cards that follow are allowed to trick, but CLUBS
         * and DIAMONDS are not allowed to trick, no matter how high they are.
         */
        private IEnumerable<Card> GetCardsOnTableThatAreAllowedToTrick()
        {
            switch (CardsOnTable.Count)
            {
                case 0:
                    return Enumerable.Empty<Card>();
                case 1:
                    return CardsOnTable;
                default:
                    var cardsOnTableThatMatchSuitOfFirstCard = from card in CardsOnTable where card.Suit.Equals(CardsOnTable[0].Suit) select card;
                    var trumpfsOnTable = from card in CardsOnTable where card.IsTrumpf(Trumpf) select card;
                    return cardsOnTableThatMatchSuitOfFirstCard.Concat(trumpfsOnTable);
            }
        }

        /**
         * Returns all cards of my hand that can beat the cards currently on the table.
         * This does not mean that I make a trick in the end, if some other player can play after me.
         */
        public IEnumerable<Card> GetMyAllowedCardsThatCanBeatCurrentCardsOnTable()
        {
            // Can be null
            var strongestCardOnTable = GetStrongestCardOnTable();
            if (strongestCardOnTable == null)
            {
                return MyCards;
            }

            return from card in GetAllowedCardsToPlay() where card.BeatsCard(strongestCardOnTable, Trumpf) select card;
        }

        /**
         * Also returns cards that are not allowed!
         */
        public IEnumerable<Card> GetMyCardsThatCanBeatCurrentCardsOnTable()
        {
            // Can be null
            var strongestCardOnTable = GetStrongestCardOnTable();
            if (strongestCardOnTable == null)
            {
                return MyCards;
            }
            return from card in MyCards where card.BeatsCard(strongestCardOnTable, Trumpf) select card;
        }

        public override string ToString()
        {
            return "GameState {" +
                    "MyCards=" + MyCards +
                    ", Trumpf=" + Trumpf +
                    ", CardsOnTable=" + CardsOnTable +
                    ", IMadeTrumpf=" + DidNotSchiebe +
                    ", Round=" + Round +
                    '}';
        }
    }
}
