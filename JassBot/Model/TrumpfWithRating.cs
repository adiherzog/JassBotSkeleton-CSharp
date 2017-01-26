using System;

namespace JassBot.Model
{
    class TrumpfWithRating : IComparable<TrumpfWithRating>
    {
        public Trumpf Trumpf { get; set; }
        public int Rating { get; set; }

        public int CompareTo(TrumpfWithRating other)
        {
            return Rating.CompareTo(other.Rating);
        }
    }
}
