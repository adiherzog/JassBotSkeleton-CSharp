using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JassBot.Model
{
    public class Card : IComparable<Card>
    {
        [JsonProperty(PropertyName = "number")]
        public int Number { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "color")]
        public Suit Suit { get; }

        public Card(Suit suit, int number)
        {
            Suit = suit;
            Number = number;
        }

        public override string ToString()
        {
            return getUnicodeCharacterForSuit(Suit) + " " + Number;
        }

        private string getUnicodeCharacterForSuit(Suit suit)
        {
            switch(suit) {
                case Suit.CLUBS:
                    return "\u2663";
                case Suit.SPADES:
                    return "\u2660";
                case Suit.HEARTS:
                    return "\u2665";
                case Suit.DIAMONDS:
                    return "\u2666";
                default:
                    return "-"; // never happens, just to make VS happy
            }
        }

        public int GetNumberTrumpfOrder()
        {
            switch (Number)
            {
                case 11:
                    return 16;
                case 9:
                    return 15;
                default:
                    return Number;
            }
        }

        public int CompareTo(Card other)
        {
            return Number.CompareTo(other.Number);
        }

        public int CompareToTrumpfOrder(Card other)
        {
            return GetNumberTrumpfOrder().CompareTo(other.GetNumberTrumpfOrder());
        }

        // TODO generate equals and hash code

        public bool IsSuitEqualTo(Suit otherSuit)
        {
            return Suit.Equals(otherSuit);
        }

        public bool BeatsCard(Card card, Trumpf trumpf)
        {
            if (TrumpfMode.TRUMPF.Equals(trumpf.Mode))
            {
                return BeatsCardTrumpf(card, trumpf);
            }
            else if (TrumpfMode.OBEABE.Equals(trumpf.Mode))
            {
                return BeatsCardObeabe(card);
            }
            else if (TrumpfMode.UNDEUFE.Equals(trumpf.Mode))
            {
                return BeatsCardUndeufe(card);
            }
            else
            {
                throw new ArgumentException("Unexpected " + trumpf.Mode);
            }
        }

        private bool BeatsCardUndeufe(Card card)
        {
            return CompareTo(card) < 0;
        }

        private bool BeatsCardObeabe(Card card)
        {
            return CompareTo(card) > 0;
        }

        private bool BeatsCardTrumpf(Card card, Trumpf trumpf)
        {
            if (Suit.Equals(trumpf.Suit))
            {
                // This card is trumpf
                if (card.Suit.Equals(trumpf.Suit))
                {
                    // Comparing two trumpfs
                    return CompareToTrumpfOrder(card) > 0;
                }
                else
                {
                    // Trumpf always beats non-trumpf
                    return true;
                }
            }
            else
            {
                // This card is not trumpf
                if (card.Suit.Equals(trumpf.Suit))
                {
                    // Other card is trumpf
                    return false;
                }
                else
                {
                    // Both cards are non-trumpf
                    return CompareTo(card) > 0;
                }
            }
        }

        /**
         * Allows to sort cards by their strength given a certain trumpf.
         */
        public static IComparer<Card> GetComperatorForTrumpf(Trumpf trumpf)
        {
            return new CardComparer(trumpf);
        }

        public bool IsTrumpf(Trumpf trumpf)
        {
            return trumpf.Mode.Equals(TrumpfMode.TRUMPF) && Suit.Equals(trumpf.Suit);
        }

        protected bool Equals(Card other)
        {
            return Number == other.Number && Suit == other.Suit;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Card) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Number*397) ^ (int) Suit;
            }
        }
    }

    class CardComparer : IComparer<Card>
    {
        private readonly Trumpf _trumpf;
        public CardComparer(Trumpf trumpf)
        {
            _trumpf = trumpf;
        }
        public int Compare(Card leftCard, Card rightCard)
        {
            return leftCard.BeatsCard(rightCard, _trumpf) ? 1 : -1;
        }
    }
}
