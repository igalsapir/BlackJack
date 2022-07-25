using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Player
    {
        const int FirstId = 0;
        const int MaxPHands = 20;

        static int NextId = 0, LastId = -1;

        private int id;
        

        private String pName;   // Name
        private int pCash;      // Initial Cash deposited
        public const int MinCash = 1000;    // Minimum House Cash
        public const int MaxCash = 1000000; // Minimum House Cash
        private int pBet;       // Initial Bet for each Hand
        public const int MinBet = 10;       // Minimum House Bet
        public const int MaxBet = 100;      // Minimum House Bet

        private Hand[] pHands;
        private int numPHands;
        public const int FirstPHand = 0;

        public int Id
        {
            get { return id; }
            // set { id = value; }
        }
        public String PName
        {
            get { return pName; }
            set { pName = value; }
        }
        public int PCash
        {
            get { return pCash; }
            set { pCash = value; }
        }
        public int PBet
        {
            get { return pBet; }
            set { pBet = value; }
        }
        public Hand[] Hands
        {
            get { return pHands; }
            // set { pHands = value; }
        }
        public int NumPHands
        {
            get { return numPHands; }
            // set { numPHands = value; }
        }

        public Player(String name = "", int cash = 0, int pbet = 0)
        {
            String input;

            // Set the Deck Id advance Id counter
            id = NextId;
            NextId++;
            LastId = id;

            // Set or Read Name
            if (name != "")
                PName = name;
            else
                PName = BlackJack.intk.Cr<String>("Please enter player's name: ", out input);

            // Set or Read Cash
            if (cash != 0)
                PCash = cash;
            else
                PCash = BlackJack.intk.CrInRange<int>("Please deposit cash: ", MinCash, MaxCash, out input);

            // Set or Read Bet
            if (pbet != 0)
                PBet = pbet;
            else
                do
                {
                    PBet = BlackJack.intk.CrInRange<int>("What is your Hand Bet (Even number): ", MinBet, MaxBet, out input);
                    if ((PBet % 2) != 0)
                        BlackJack.MyWrite(ConsoleColor.Red, "Bet must be an Even number\n");
                    if (PBet > PCash)
                        BlackJack.MyWrite(ConsoleColor.Red, "Bet must be smaller than your Cash\n");
                } while ((PBet % 2) != 0 || PBet > PCash);

            // Allocate Player Hands and Last Hand index
            pHands = new Hand[MaxPHands];
            numPHands = 0;
        }

        public override String ToString()
        {
            String s;
            int i;

            s = $"Player {PName} has ${PCash}\n";
            for (i = 0; i < NumPHands; i++)
                s += $"Hand[{i,2}: " + pHands[i].ToString() + "\n";

            return s;
        }

        // Add Player Hand with Two cards
        public bool AddPHand(Card C1, Card C2)
        {
            Hand PHand;                     // Player's hand

            if (numPHands == MaxPHands)     // If MaxPHands reached return
            {
                BlackJack.MyWrite(ConsoleColor.Red, "Max Hands reached\n");
                return false;
            }

            if (PBet > PCash)
            {
                BlackJack.MyWrite(ConsoleColor.Red, "You do not have enough Cash\n");
                return false;
            }

            // Create the hand
            PHand = new(PBet);
            PCash -= PBet;

            PHand.Insert(C1);
            PHand.Insert(C2);

            Hands[numPHands] = PHand;       // Add it to the hands
            numPHands++;

            return true;
        }

        public bool IsStillPlaying()
        {
            int i;

            for (i = FirstPHand; i < NumPHands; i++)
                if (Hands[i].IsPlaying())
                    return true;
            return false;
        }

        public bool Surrender(int HandI)
        {
            if (Hands[HandI].NumCards != 2)
                return false;

            Hands[HandI].Hstat = HStat.Bust;

            PCash += Hands[HandI].HBet / 2;
            Hands[HandI].HBet /= 2;

            return true;
        }

        // Win/Lose Accounting - Handle Insurance, Push = Tie, etc
        public void WinLoseTie(Hand Dlr)
        {
            int i;

            // For each Player Hand
            for (i = FirstPHand; i < NumPHands; i++)
            {
                BlackJack.MyWrite(ConsoleColor.Green,"Results and Accounng\n");

                BlackJack.WriteHandVsHand(ConsoleColor.Cyan, Dlr, Hands[i], true, Move.No);

                if (Hands[i].IsBust() || !Dlr.IsBust() && Hands[i].HandValue < Dlr.HandValue)
                {
                    // Check if Hand is Insured
                    if (Dlr.IsBJ() && Hands[i].Insured)
                    {
                        BlackJack.MyWrite(ConsoleColor.Yellow, "This Hand was Insured - Breaks even\n");
                        PCash += Hands[i].HBet;
                    }
                    else
                        // This Hand loses.
                        BlackJack.MyWrite(ConsoleColor.Red,"This Hand Loses " + Hands[i].HBet + "\n");
                }
                else if (Dlr.IsBust() || Hands[i].HandValue > Dlr.HandValue)
                {
                    // This Hand wins.
                    BlackJack.MyWrite(ConsoleColor.Blue, "This Hand Wins " + Hands[i].HBet + "\n");
                    PCash += 2 * Hands[i].HBet;
                }
                else if (Hands[i].IsBJ())
                {
                    // This Hand is a BJ Tie - Wins 3:2
                    BlackJack.MyWrite(ConsoleColor.Blue, "This Hand is a BJ Tie - Wins " + (Hands[i].HBet / 2) + "\n");
                    PCash += Hands[i].HBet + Hands[i].HBet / 2;
                }
                else
                {
                    // Tie
                    BlackJack.MyWrite(ConsoleColor.Yellow,"This Hand is a Push (Tie)\n");
                    PCash += Hands[i].HBet;
                }
            }
        }

        public void DiscardAll(Deck Disc)
        {
            int i;

            for (i = FirstPHand; i < NumPHands; i++)
                Hands[i].DiscardAll(Disc);

            numPHands = 0;
        }

    }
}
