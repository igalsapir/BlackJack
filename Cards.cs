using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Card
    {
        private CardRank rank;
        private CardSuite suite;

        public const int FirstRank = (int)CardRank.Ace;
        public const int LastRank = (int)CardRank.King;
        public const int MaxRank = LastRank;
        public const int FirstSuite = (int)CardSuite.Club;
        public const int LastSuite = (int)CardSuite.Spade;
        public const int MaxSuite = LastSuite;

        static readonly Random rnd = new();

        public CardRank Rank
        {
            get { return rank; }
            set { rank = value; }
        }
        public CardSuite Suite
        {
            get { return suite; }
            set { suite = value; }
        }

        // Constructor with all parameters
        public Card (CardRank newrank, CardSuite newsuite)
        {
            this.rank = newrank;
            this.suite = newsuite;
        }
        // Constructor with No parameters
        public Card()
        {
            this.rank = (CardRank) BlackJack.rnd.Next(FirstRank, LastRank + 1);
            this.suite = (CardSuite) BlackJack.rnd.Next(FirstSuite, LastSuite + 1);
        }
        // Constructor with a Card parameter
        public Card(Card other)
        {
            this.rank = other.rank;
            this.suite = other.suite;
        }

        public override String ToString()
        {
            return $"{this.rank,5} of {this.suite,-7}";
        }

        public bool Equals(Card other)
        {
            return this.suite == other.suite && this.rank == other.rank;
        }

        public void Swap(Card other)
        {
            CardRank tmprank;
            CardSuite tmpsuite;
            
            // Swap Rank
            tmprank = this.rank;
            this.rank = other.Rank;
            other.rank = tmprank;

            // Swap Suite
            tmpsuite = this.suite;
            this.suite = other.Suite;
            other.suite = tmpsuite;
        }

        public bool IsPic()
        {
            return this.rank == CardRank.Jack || this.rank == CardRank.Queen || this.rank == CardRank.King;
        }

        public int Value()
        {
            if (this.IsPic())
                return 10;
            return (int)this.rank;
        }
    }
}
