// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    partial class BlackJack
    {
        static void TestStrategy()
        {
            int i, j, v;
            Card P1, P2, D;
            Hand PHand;
            Move NxtMove;
            Strategy BJstrat = new();

            P1 = new Card();
            P2 = new Card();
            D = new Card();

            // Test Double Card Strategy
            MyWrite(ConsoleColor.Green, "Test Double Card Strategy\n");
            for (i = Card.FirstRank; i < Card.LastRank + 1; i++)
            {
                PHand = new(10);
                P1.Rank = (CardRank)i;
                PHand.Insert(P1);
                P2.Rank = (CardRank)i;
                PHand.Insert(P2);
                MyWrite(ConsoleColor.Green, PHand.ToString() + "\n");

                for (j = Card.FirstRank; j < Card.LastRank + 1; j++)
                {
                    D.Rank = (CardRank)j;
                    NxtMove = BJstrat.NextMove(D, PHand);
                    MyWrite(ConsoleColor.Cyan, D.ToString() + "(" + NxtMove + ")\t");

                    if ((j - Card.FirstRank + 1) % 7 == 0)
                        MyWrite(ConsoleColor.Cyan,"\n");
                }
                MyWrite(ConsoleColor.Cyan,"\n");
            }
            MyWrite(ConsoleColor.Cyan, "\n");

            // Test Soft Hand Strategy
            MyWrite(ConsoleColor.Green, "Test Soft Hand Strategy\n");
            for (i = Card.FirstRank; i < Card.LastRank + 1; i++)
            {
                PHand = new(10);
                P1.Rank = CardRank.Ace;
                PHand.Insert(P1);
                P2.Rank = (CardRank)i;
                PHand.Insert(P2);
                MyWrite(ConsoleColor.Green, PHand.ToString() + "\n");

                if (PHand.IsDbl())  // If Player has Double hand - Use Double strategy
                {
                    MyWrite(ConsoleColor.Cyan, "Double Strategy\n");
                    continue;
                }

                for (j = Card.FirstRank; j < Card.LastRank + 1; j++)
                {
                    D.Rank = (CardRank)j;
                    NxtMove = BJstrat.NextMove(D, PHand);
                    MyWrite(ConsoleColor.Cyan, D.ToString() + "(" + NxtMove + ")\t");

                    if ((j - Card.FirstRank + 1) % 7 == 0)
                        MyWrite(ConsoleColor.Cyan,"\n");
                }
                MyWrite(ConsoleColor.Cyan,"\n");
            }
            MyWrite(ConsoleColor.Cyan,"\n");

            // Test Basic Strategy
            MyWrite(ConsoleColor.Green, "Test Basic Strategy\n");

            for (v = Strategy.MinBasicValue; v < Strategy.MaxBasicValue + 1; v++)
            {
                PHand = new(10);
                P1.Rank = (CardRank)(v / 2);
                if (P1.Rank > CardRank.Two && P1.Rank < CardRank.Ten && v < (Strategy.MaxBasicValue - 1))
                    P1.Rank--;
                PHand.Insert(P1);
                P2.Rank = (CardRank)(v - (int)P1.Rank);
                if (v == Strategy.MaxBasicValue)
                    P2.Rank++;
                PHand.Insert(P2);
                MyWrite(ConsoleColor.Green, PHand.ToString() + "\n");

                if (PHand.IsDbl())  // If Player has Double hand - Use Double strategy
                {
                    MyWrite(ConsoleColor.Cyan, "Double Strategy\n");
                    continue;
                }

                if (PHand.IsInitialSoft())  // If Player has Soft hand - Use Soft Hand strategy
                {
                    MyWrite(ConsoleColor.Cyan, "Soft hand Strategy\n");
                    continue;
                }

                for (j = Card.FirstRank; j < Card.LastRank + 1; j++)
                {
                    D.Rank = (CardRank)j;
                    NxtMove = BJstrat.NextMove(D, PHand);
                    MyWrite(ConsoleColor.Cyan, D.ToString() + "(" + NxtMove + ")\t");

                    if ((j - Card.FirstRank + 1) % 7 == 0)
                        MyWrite(ConsoleColor.Cyan, "\n");
                }
                MyWrite(ConsoleColor.Cyan, "\n");
            }
            MyWrite(ConsoleColor.Cyan, "\n");
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

            MyWrite(ConsoleColor.Green,"Test Cards - Hello - Igal Sapir\n");

            MyWrite(ConsoleColor.Cyan,"C1 = " + C1.ToString() + "\n");
            MyWrite(ConsoleColor.Cyan,"C2 = " + C2.ToString() + "\n");
            MyWrite(ConsoleColor.Cyan,"C3 = " + C3.ToString() + "\n\n");

            MyWrite(ConsoleColor.Cyan,C1.ToString() + " is " + ((C1.Equals(C2)) ? "" : "not ") + "equal to " + C2.ToString() + "\n");
            MyWrite(ConsoleColor.Cyan,C3.ToString() + " is " + ((C3.Equals(C2)) ? "" : "not ") + "equal to " + C2.ToString() + "\n\n");

            for (int i = 0; i < 10; i++)
            {
                C4 = new Card();
                MyWrite(ConsoleColor.Cyan,C4.ToString() + "\t");
                if ((i + 1) % 5 == 0)
                    MyWrite(ConsoleColor.Cyan, "\n");
            }
            MyWrite(ConsoleColor.Cyan, "\n");

            MyWrite(ConsoleColor.Cyan,"C5 = " + C5.ToString() + "\n");
            MyWrite(ConsoleColor.Green,"Full Deck\n");
            for (int i = Card.FirstSuite, cnt = 0; i < Card.LastSuite + 1; i++)
            {
                for (int j = Card.FirstRank; j < Card.LastRank + 1; j++, cnt++)
                {
                    C5.Rank = (CardRank)j;
                    C5.Suite = (CardSuite)i;
                    MyWrite(ConsoleColor.Cyan,C5.ToString() + "\t");
                    if ((cnt + 1) % 5 == 0)
                        MyWrite(ConsoleColor.Cyan, "\n");
                }
                cnt = 0;
                MyWrite(ConsoleColor.Cyan, "\n");
            }
            MyWrite(ConsoleColor.Cyan, "\n");

            D1 = new Deck();
            MyWrite(ConsoleColor.Green,"D1 IsComplete is " + D1.IsComplete() + "\n");
            MyWrite(ConsoleColor.Green,"Full Deck\n");
            MyWrite(ConsoleColor.Cyan,D1.ToString() + "\n");
            MyWrite(ConsoleColor.Cyan, "\n");

            D1.ShuffleStripCut();
            MyWrite(ConsoleColor.Green,"D1 IsComplete is " + D1.IsComplete() + "\n");
            MyWrite(ConsoleColor.Green,"After StripCut\n");
            MyWrite(ConsoleColor.Cyan,D1.ToString() + "\n");
            MyWrite(ConsoleColor.Cyan, "\n");

            D1.ShuffleRiffle();
            MyWrite(ConsoleColor.Green,"D1 IsComplete is " + D1.IsComplete() + "\n");
            MyWrite(ConsoleColor.Green,"After Riffle\n");
            MyWrite(ConsoleColor.Cyan,D1.ToString() + "\n");
            MyWrite(ConsoleColor.Cyan, "\n");

            D1.ShuffleCut();
            MyWrite(ConsoleColor.Green,"D1 IsComplete is " + D1.IsComplete() + "\n");
            MyWrite(ConsoleColor.Green,"After Cut\n");
            MyWrite(ConsoleColor.Cyan,D1.ToString() + "\n");
            MyWrite(ConsoleColor.Cyan, "\n");

            D1.ShuffleStandard();
            MyWrite(ConsoleColor.Green,"D1 IsComplete is " + D1.IsComplete() + "\n");
            MyWrite(ConsoleColor.Green,"After Standard Shuffle\n");
            MyWrite(ConsoleColor.Cyan,D1.ToString() + "\n");
            MyWrite(ConsoleColor.Cyan, "\n");

            Card? C6 = D1.Draw();
            MyWrite(ConsoleColor.Green,"After Draw Card = " + C6.ToString() + "\n");
            MyWrite(ConsoleColor.Green,"D1 IsComplete is " + D1.IsComplete() + "\n");
            MyWrite(ConsoleColor.Cyan,D1.ToString() + "\n");
            MyWrite(ConsoleColor.Cyan, "\n");

            S1 = new Shoe(5);
            MyWrite(ConsoleColor.Green,"S1 IsComplete is " + S1.IsComplete(5) + "\n");
            MyWrite(ConsoleColor.Green,"Full Shoe with 5 decks\n");
            MyWrite(ConsoleColor.Cyan,S1.ToString() + "\n");
            MyWrite(ConsoleColor.Cyan, "\n");

            Deck Discarded = new(true); // Discraded Deck
            Card? C7 = S1.Draw();
            MyWrite(ConsoleColor.Green,"After Draw Card = " + C7.ToString() + "\n");
            MyWrite(ConsoleColor.Green,"S1 IsComplete is " + S1.IsComplete(5) + "\n");
            MyWrite(ConsoleColor.Cyan,S1.ToString() + "\n");
            MyWrite(ConsoleColor.Cyan, "\n");

            MyWrite(ConsoleColor.Green,"Test Cards - Good bye\n");
        }
    }

}
