// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class Program
    {
        static void Main(String[] argv)
        {
            Shoe BJshoe = new(5);       // The Shoe for the BJ game - 5 Decks

            Hand Dealer, Player;        // Dealer and Player
            Card C1, C2, NewCard;       // The 

            Console.WriteLine("BlackJack - Hello - Igal Sapir");

            // TestCards();

            // Deal two cards to the Player
            C1 = BJshoe.Draw();
            C2 = BJshoe.Draw();
            Player = new(C1, C2);

            // Deal two cards to the Dealer
            C1 = BJshoe.Draw(); // Open Card
            C2 = BJshoe.Draw();
            Dealer = new(C1, C2);

            Strategy BJstrat = new();
            Move NxtMove;

            Console.WriteLine("Player: " + Player.ToString());
            Console.WriteLine("Dealer: " + Dealer.ToString());
            Console.WriteLine();

            NxtMove = BJstrat.NextMove(Dealer.Cards[Hand.CardOpen], Player);
            Console.WriteLine("Next Move: " + NxtMove);
            Console.WriteLine();

            TestStrategy();

            Console.WriteLine("Black Jack - Good bye");
        }

        static void TestStrategy()
        {
            int i, j, v;
            Card P1, P2, D;
            Hand Player;
            Move NxtMove;
            Strategy BJstrat = new();

            P1 = new Card();
            P2 = new Card();
            D = new Card();

            // Test Double Card Strategy
            Console.WriteLine("Test Double Card Strategy");
            for (i = Card.FirstRank; i < Card.LastRank + 1; i++)
            {
                P1.Rank = (CardRank)i;
                P2.Rank = (CardRank)i;
                Player = new Hand(P1, P2);
                Console.WriteLine(Player.ToString());
                
                for (j = Card.FirstRank; j < Card.LastRank + 1; j++)
                {
                    D.Rank = (CardRank)j;
                    NxtMove = BJstrat.NextMove(D, Player);
                    Console.Write(D.ToString() + "(" + NxtMove + ")\t");

                    if ((j - Card.FirstRank + 1) % 7 == 0)
                        Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            // Test Soft Hand Strategy
            Console.WriteLine("Test Soft Hand Strategy");
            for (i = Card.FirstRank; i < Card.LastRank + 1; i++)
            {
                P1.Rank = CardRank.Ace;
                P2.Rank = (CardRank)i;
                Player = new Hand(P1, P2);
                Console.WriteLine(Player.ToString());

                if (Player.IsDbl())  // If Player has Double hand - Use Double strategy
                {
                    Console.WriteLine("Double Strategy");
                    continue;
                }

                for (j = Card.FirstRank; j < Card.LastRank + 1; j++)
                {
                    D.Rank = (CardRank)j;
                    NxtMove = BJstrat.NextMove(D, Player);
                    Console.Write(D.ToString() + "(" + NxtMove + ")\t");

                    if ((j - Card.FirstRank + 1) % 7 == 0)
                        Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            // Test Basic Strategy
            Console.WriteLine("Test Basic Strategy");
            /*
            for (i = Card.FirstRank; i < Card.LastRank + 1; i++)
            {
                P1.Rank = CardRank.Ace;
                P2.Rank = (CardRank)i;
                Player = new Hand(P1, P2);
                Console.WriteLine(Player.ToString());

                if (Player.IsDbl())  // If Player has Double hand - Use Double strategy
                {
                    Console.WriteLine("Double Strategy");
                    continue;
                }

                for (j = Card.FirstRank; j < Card.LastRank + 1; j++)
                {
                    D.Rank = (CardRank)j;
                    NxtMove = BJstrat.NextMove(D, Player);
                    Console.Write(D.ToString() + "(" + NxtMove + ")\t");

                    if ((j - Card.FirstRank + 1) % 7 == 0)
                        Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            */
        }
        static void TestCards()
        {
            Card C1 = new(CardRank.Queen, CardSuite.Heart);
            Card C2 = new();
            Card C3 = new(C2);
            Card C4;
            Card C5 = new();

            Deck D1;

            Shoe S1;

            Console.WriteLine("Test Cards - Hello - Igal Sapir");

            Console.WriteLine("C1 = " + C1.ToString());
            Console.WriteLine("C2 = " + C2.ToString());
            Console.WriteLine("C3 = " + C3.ToString());
            Console.WriteLine();

            Console.WriteLine(C1.ToString() + " is " + ((C1.Equals(C2)) ? "" : "not ") + "equal to " + C2.ToString());
            Console.WriteLine(C3.ToString() + " is " + ((C3.Equals(C2)) ? "" : "not ") + "equal to " + C2.ToString());
            Console.WriteLine();

            for (int i = 0; i < 10; i++)
            {
                C4 = new Card();
                Console.Write(C4.ToString() + "\t");
                if ((i + 1) % 5 == 0)
                    Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("C5 = " + C5.ToString());
            Console.WriteLine("Full Deck");
            for (int i = Card.FirstSuite, cnt = 0; i < Card.LastSuite + 1; i++)
            {
                for (int j = Card.FirstRank; j < Card.LastRank + 1; j++, cnt++)
                {
                    C5.Rank = (CardRank)j;
                    C5.Suite = (CardSuite)i;
                    Console.Write(C5.ToString() + "\t");
                    if ((cnt + 1) % 5 == 0)
                        Console.WriteLine();
                }
                cnt = 0;
                Console.WriteLine();
            }
            Console.WriteLine();

            D1 = new Deck();
            Console.WriteLine("D1 IsFull is " + D1.IsFull());
            Console.WriteLine("Full Deck");
            Console.WriteLine(D1.ToString());
            Console.WriteLine();

            D1.ShuffleStripCut();
            Console.WriteLine("D1 IsFull is " + D1.IsFull());
            Console.WriteLine("After StripCut");
            Console.WriteLine(D1.ToString());
            Console.WriteLine();

            D1.ShuffleRiffle();
            Console.WriteLine("D1 IsFull is " + D1.IsFull());
            Console.WriteLine("After Riffle");
            Console.WriteLine(D1.ToString());
            Console.WriteLine();

            D1.ShuffleCut();
            Console.WriteLine("D1 IsFull is " + D1.IsFull());
            Console.WriteLine("After Cut");
            Console.WriteLine(D1.ToString());
            Console.WriteLine();

            D1.ShuffleStandard();
            Console.WriteLine("D1 IsFull is " + D1.IsFull());
            Console.WriteLine("After Standard Shuffle");
            Console.WriteLine(D1.ToString());
            Console.WriteLine();

            Card C6 = D1.Draw();
            Console.WriteLine("After Draw Card = " + C6.ToString());
            Console.WriteLine("D1 IsFull is " + D1.IsFull());
            Console.WriteLine(D1.ToString());
            Console.WriteLine();

            S1 = new Shoe(5);
            Console.WriteLine("S1 IsFull is " + S1.IsFull(5));
            Console.WriteLine("Full Shoe with 5 decks");
            Console.WriteLine(S1.ToString());
            Console.WriteLine();

            Card C7 = S1.Draw();
            Console.WriteLine("After Draw Card = " + C7.ToString());
            Console.WriteLine("S1 IsFull is " + S1.IsFull(5));
            Console.WriteLine(S1.ToString());
            Console.WriteLine();

            Console.WriteLine("Test Cards - Good bye");
        }
    }


    // Suits: clubs ♣, diamonds ♦, hearts ♥, spades ♠
    public enum CardSuite { None = -1, Club = 1, Diamond, Heart, Spade };

    public enum CardRank { None = -1, Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };

    public enum Move { No = -1, Win = 0, sTnd = 1, Dbl, sPlt, Hit };
}
