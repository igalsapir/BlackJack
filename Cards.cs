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
            this.rank = (CardRank) rnd.Next(FirstRank, LastRank + 1);
            this.suite = (CardSuite) rnd.Next(FirstSuite, LastSuite + 1);
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

    public class Deck
    {
        static int NextId = 0, LastId = -1;
        const int FirstId = 0;

        private int id;
        private int numCards;
        private Card[] cards;

        static readonly Random rnd = new();

        public int Id
        {
            get { return id; }
            // set { id = value; }
        }
        public int NumCards
        {
            get { return numCards; }
            set { numCards = value; }
        }
        public Card[] Cards
        {
            get { return cards; }
            set { cards = value; }
        }

        // Regular constructor
        public Deck()
        {
            // Set the Deck Id advance Id counter
            id = NextId;
            NextId++;
            LastId = id;

            // Initialize the cards
            NumCards = Card.MaxRank * Card.MaxSuite;
            cards = new Card[Card.MaxRank * Card.MaxSuite];
            for (int i = Card.FirstSuite, cnt = 0; i < Card.LastSuite + 1; i++)
                for (int j = Card.FirstRank; j < Card.LastRank + 1; j++, cnt++)
                    cards[cnt] = new Card((CardRank)j, (CardSuite)i);
        }

        /* Maybe don't need one like this
        // Constructor from another Deck
        public Deck(Deck other)
        {
        }
        */

        public override String ToString()
        {
            String s;

            s = $"{this.GetType()}: {id,2} , Num of cards: {numCards,2}\n";
            for (int i = 0; i < numCards; i++)
            {
                s += cards[i].ToString() + "\t";
                if ((i + 1) % 5 == 0)
                    s += "\n";
            }

            return s;
        }

        public bool Equals(Deck other)
        {
            return this.id == other.id;
        }

        public bool IsFull(int numDecks = 1)
        {
            int[,] Counted = new int[Card.MaxRank + 1, Card.MaxSuite + 1];
            int i, j, k;
            bool ret = true;

            // Initialize Counted
            for (i = Card.FirstSuite; i < Card.LastSuite + 1; i++)
                for (j = Card.FirstRank; j < Card.LastRank + 1; j++)
                    Counted[j, i] = 0;

            // Count all cards
            for (k = 0; k < numCards; k++)
            {
                Counted[(int)cards[k].Rank, (int)cards[k].Suite]++;
                if (Counted[(int)cards[k].Rank, (int)cards[k].Suite] > numDecks)
                {
                    ret = false;
                    Console.WriteLine("Too many cards (" + Counted[(int)cards[k].Rank, (int)cards[k].Suite] +
                        " > " + numDecks + "): at location " + k + ":" + cards[k].ToString());
                }
            }

            // See if any card is missing
            for (i = Card.FirstSuite; i < Card.LastSuite + 1; i++)
                for (j = Card.FirstRank; j < Card.LastRank + 1; j++)
                    if (Counted[j, i] < numDecks)
                    {
                        ret = false;
                        Console.WriteLine("Missing card: " + (CardRank)j + " of " + (CardSuite)i);
                    }

            return ret;
        }

        // Shuffle the Deck by Cutting in two
        public void ShuffleCut()
        {
            int cut, i, j;
            Card[] tmp;

            // Cut aproximately in the middle
            cut = rnd.Next((int)(Math.Round(0.45 * numCards)), (int)(Math.Round(0.5 * numCards)));
            // Make sure it is not more than Half
            cut = Math.Min(cut, numCards / 2);

            // Debug
            Console.WriteLine("Cut at " + cut + " = " + cards[cut - 1].ToString());

            // Copy first cut cards to tmp
            tmp = new Card[cut];
            for (i = 0; i < cut; i++)
                tmp[i] = cards[i];

            for (i = 0, j = cut; j < numCards; i++, j++)
                cards[i] = cards[j];
            for (j = 0; j < cut; i++, j++)
                cards[i] = tmp[j];
        }

        // Shuffle the Deck by Cutting in strips 4-5 times
        public void ShuffleStripCut()
        {
            int i, j, k;
            int strip, strips, cut;
            Card[] tmp;

            // Strip aproximately 25%-20% of the Deck
            strip = rnd.Next((int)(Math.Round(0.20 * numCards)), (int)(Math.Round(0.25 * numCards)));
            // Cut aproximately at 75%-80% of the Deck
            cut = numCards - strip;
            strips = cut / strip + 2;

            tmp = new Card[cut];
            // Copy first cut cards to tmp
            for (i = 0; i < cut; i++)
                tmp[i] = cards[i];

            // Debug
            Console.WriteLine("Cut at " + cut + " = " + cards[cut - 1].ToString() + " strip of " + strip);
            for (j = cut - 1, k = 1; k <= strips; k++)
                for (i = k * strip - 1; i >= ((k - 1) * strip) && j >= 0; i--)
                    if (i < cut)
                    {
                        cards[j] = tmp[i];
                        j--;
                    }
        }

        // Shuffle the Deck by Riffling
        public void ShuffleRiffle()
        {
            int cut, i, j;
            Card[] tmp;

            // Cut in the middle
            cut = numCards / 2;

            // Debug
            Console.WriteLine("Cut at " + cut + " = " + cards[cut - 1].ToString());

            // Copy first cut cards to tmp
            tmp = new Card[cut];
            for (i = 0; i < cut; i++)
                tmp[i] = cards[i];

            for (i = 0, j = cut; j < numCards; i += 2, j++)
                cards[i] = cards[j];
            for (i = 1, j = 0; j < cut; i += 2, j++)
                cards[i] = tmp[j];
        }

        // Standard Shuffle for Deck
        public void ShuffleStandard()
        {
            // The standard Vegas Shuffle
            ShuffleRiffle();
            ShuffleRiffle();
            ShuffleStripCut();
            ShuffleRiffle();
            ShuffleCut();
        }

        // Draw Card
        public Card Draw()
        {
            numCards--;

            if (numCards >= 0)
                return cards[numCards];
            else
            {
                Card card = new();

                Console.WriteLine($"{this.GetType()} is Empty - Randomized {card}");
                return card;
            }
        }
    }

    public class Shoe : Deck
    {
        private int numDecks;
        private Deck[] decks;

        public int NumDecks
        {
            get { return numDecks; }
            // set {  numDecks = value; }
        }

        public Deck[] Decks
        {
            get { return decks; }
            // set { decks = value; }
        }

        // Regular Constructor
        public Shoe(int numDecks)
        {
            int i, j, cnt;

            // Set the Number of Decks
            this.numDecks = numDecks;

            decks = new Deck[numDecks];

            // Calculate the nuber of cards
            NumCards = numDecks * Card.MaxRank * Card.MaxSuite;
            Cards = new Card[NumCards];

            // Create the Decks and move their cards to the Shoe
            for (i = 0, cnt = 0; i < numDecks; i++)
            {
                decks[i] = new();
                for (j = 0; j < decks[i].NumCards; j++, cnt++)
                    this.Cards[cnt] = decks[i].Cards[j];
            }
        }
    }

    public class Hand : Deck
    {
        public const int Card1 = 0;
        public const int Card2 = 1;
        public const int CardOpen = Card1;

        public const int MaxCards = 11;

        private int handValue;

        public int HandValue
        {
            get { return handValue; }
            // set { handValue = value; }
        }
        
        // Regular constructor
        public Hand(Card C1, Card C2)
        {
            // Initialize the cards
            NumCards = 2;
            Cards = new Card[MaxCards];
            Cards[0] = C1;
            Cards[1] = C2;

            handValue = C1.Value() + C2.Value();
        }

        public bool IsDbl()
        {
            if (NumCards != 2)
                return false;

            return Cards[0].Rank == Cards[1].Rank;
        }

        public bool IsSoft()
        {
            for(int i = 0; i < this.NumCards; i++)
                if (this.Cards[i].Rank == CardRank.Ace)
                    return true;

            return false;
        }

        public override String ToString()
        {
            String s;

            return (base.ToString() + " HandValue = " + HandValue);
        }

    }
}
