using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace BlackJack
{
    public class Deck
    {
        const int FirstId = 0;
        static int NextId = 0, LastId = -1;

        private int id;
        private int maxCards;
        private int numCards;
        private Card[] cards;

        public int Id
        {
            get { return id; }
            // set { id = value; }
        }
        public int MaxCards
        {
            get { return maxCards; }
            set { maxCards = value; }
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
        public Deck(bool DiscardDeck = false)
        {
            int i, j, cnt;

            // Set the Deck Id advance Id counter
            id = NextId;
            NextId++;
            LastId = id;

            // Allocate and Initialize the cards holding array
            maxCards = Card.MaxRank * Card.MaxSuite;
            cards = new Card[MaxCards];
            numCards = 0;

            // If Discard Deck - It is empty,  so no need to allocate end intialize the cards
            if (DiscardDeck)
                return;

            // Allocate and Initialize the cards
            for (i = Card.FirstSuite, cnt = 0; i < Card.LastSuite + 1; i++)
                for (j = Card.FirstRank; j < Card.LastRank + 1; j++, cnt++)
                    cards[cnt] = new Card((CardRank)j, (CardSuite)i);

            numCards = cnt;
        }

        // Maybe don't need one like this
        // Constructor from another Deck
        // public Deck(Deck other)
        // {
        // }
        //

        public virtual String ToString(bool Show = true)
        {
            String s;

            if (!Show)
                s = $"{this.GetType()}: {id,2} , Num of cards: {numCards,2} - No Show\n";
            else
            {
                s = $"{this.GetType()}: {id,2} , Num of cards: {numCards,2}\n";
                for (int i = 0; i < numCards; i++)
                {
                    s += cards[i].ToString() + "\t";
                    if ((i + 1) % 5 == 0)
                        s += "\n";
                }

            }

            return s;
        }

        public bool Equals(Deck other)
        {
            return this.id == other.id;
        }

        const int IsCompleteInit = 0;
        const int IsCompleteCount = 1;
        const int IsCompleteCheck = 2;

        static int[,] Counted = new int[Card.MaxRank + 1, Card.MaxSuite + 1];

        public bool IsCompleteStep(int numDecks, int Step, Card[] cs, int NCards)
        {
            bool ret = true;
            int i, j, k;

            switch (Step)
            {
                case IsCompleteInit:
                    for (i = Card.FirstSuite; i < Card.LastSuite + 1; i++)
                        for (j = Card.FirstRank; j < Card.LastRank + 1; j++)
                            Counted[j, i] = 0;

                    break;

                case IsCompleteCount:
                    for (k = 0; k < NCards; k++)
                    {
                        Counted[(int)cs[k].Rank, (int)cs[k].Suite]++;
                        if (Counted[(int)cs[k].Rank, (int)cs[k].Suite] > numDecks)
                        {
                            ret = false;
                            BlackJack.MyWrite(ConsoleColor.Red,"Too many cards (" + Counted[(int)cs[k].Rank, (int)cs[k].Suite] +
                                " > " + numDecks + "): at location " + k + ":" + cs[k].ToString() + "\n");
                        }
                    }
                    break;

                case IsCompleteCheck:
                    for (i = Card.FirstSuite; i < Card.LastSuite + 1; i++)
                        for (j = Card.FirstRank; j < Card.LastRank + 1; j++)
                            if (Counted[j, i] < numDecks)
                            {
                                ret = false;
                                BlackJack.MyWrite(ConsoleColor.Red,"Missing card: " + (CardRank)j + " of " + (CardSuite)i + "\n");
                            }
                    break;

                default:
                    break;
            }
            return ret;
        }


        public bool IsComplete(int numDecks = 1, Deck? Dlr = null, Player? Plr = null, Deck? Disc = null)
        {
            bool ret = true;
            int i;

            // Initialize Counted
            IsCompleteStep(numDecks, IsCompleteInit, Cards, NumCards);

            // Count all cards
            // Remaining Deck
            IsCompleteStep(numDecks, IsCompleteCount, Cards, NumCards);

            // Dealer
            if (Dlr != null)
                IsCompleteStep(numDecks, IsCompleteCount, Dlr.Cards, Dlr.NumCards);

            // All Player Hands
            if (Plr != null)
            {
                for (i = Player.FirstPHand; i < Plr.NumPHands; i++)
                    if (Plr.Hands[i] != null)
                        IsCompleteStep(numDecks, IsCompleteCount, Plr.Hands[i].Cards, Plr.Hands[i].NumCards);
            }

            // Discarded Cards
            if (Disc != null)
                IsCompleteStep(numDecks, IsCompleteCount, Disc.Cards, Disc.NumCards);

            // See if any card is missing
            ret = IsCompleteStep(numDecks, IsCompleteCheck, Cards, NumCards);

            return ret;
        }

        // Shuffle the Deck by Cutting in two
        public void ShuffleCut()
        {
            int cut, i, j;
            Card[] tmp;

            BlackJack.MyWrite(ConsoleColor.Green, "Shuffle Cut\n");

            // Cut aproximately in the middle
            cut = BlackJack.rnd.Next((int)(Math.Round(0.45 * numCards)), (int)(Math.Round(0.5 * numCards)));
            // Make sure it is not more than Half
            cut = Math.Min(cut, numCards / 2);

            // Debug
            BlackJack.MyWrite(ConsoleColor.Green,"Cut at " + cut + " = " + cards[cut - 1].ToString() + "\n");

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

            BlackJack.MyWrite(ConsoleColor.Green, "Shuffle Strip Cut\n");

            // Strip aproximately 25%-20% of the Deck
            strip = BlackJack.rnd.Next((int)(Math.Round(0.20 * numCards)), (int)(Math.Round(0.25 * numCards)));
            // Cut aproximately at 75%-80% of the Deck
            cut = numCards - strip;
            strips = cut / strip + 2;

            tmp = new Card[cut];
            // Copy first cut cards to tmp
            for (i = 0; i < cut; i++)
                tmp[i] = cards[i];

            // Debug
            BlackJack.MyWrite(ConsoleColor.Green,"Cut at " + cut + " = " + cards[cut - 1].ToString() + " strip of " + strip + "\n");
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

            BlackJack.MyWrite(ConsoleColor.Green, "Shuffle Riffle\n");

            // Cut in the middle
            cut = numCards / 2;

            // Debug
            BlackJack.MyWrite(ConsoleColor.Green,"Cut at " + cut + " = " + cards[cut - 1].ToString() + "\n");

            // Copy first cut cards to tmp
            tmp = new Card[cut];
            for (i = 0; i < cut; i++)
                tmp[i] = cards[i];

            for (i = 0, j = cut; j < numCards; i += 2, j++)
                cards[i] = cards[j];
            for (i = 1, j = 0; j < cut; i += 2, j++)
                cards[i] = tmp[j];
        }

        // Shuffle Sound
        [DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true, CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        private static extern bool PlaySound(string szSound, System.IntPtr hMod, PlaySoundFlags flags);

        [System.Flags]
        public enum PlaySoundFlags : int
        {
            SND_SYNC = 0x0000,
            SND_ASYNC = 0x0001,
            SND_NODEFAULT = 0x0002,
            SND_LOOP = 0x0008,
            SND_NOSTOP = 0x0010,
            SND_NOWAIT = 0x00002000,
            SND_FILENAME = 0x00020000,
            SND_RESOURCE = 0x00040004
        }

        private static String SoundFNamePath()
        {
            const String Fname = "\\Shuffle.wav";
            String CurDir = Directory.GetCurrentDirectory();

            String Dir, FnamePath;
            System.IO.DirectoryInfo directoryInfo;

            for (Dir = CurDir; Dir != "\\";)
            {
                FnamePath = Dir + Fname;
                if (File.Exists(FnamePath))
                    return FnamePath;

                directoryInfo = System.IO.Directory.GetParent(Dir);
                if (directoryInfo == null)
                    return (CurDir);

                Dir = directoryInfo.FullName;
            }
            return (CurDir);
        }

        // Standard Shuffle for Deck
        public void ShuffleStandard()
        {
            BlackJack.MyWrite(ConsoleColor.Green, "Shuffle Vegas Standard\n");

            // Play Shuffle Sound
            String SoundFname = SoundFNamePath();
            PlaySound(SoundFname, new System.IntPtr(), PlaySoundFlags.SND_SYNC);

            // The standard Vegas Shuffle
            ShuffleRiffle();
            ShuffleRiffle();
            ShuffleStripCut();
            ShuffleRiffle();
            ShuffleCut();

            BlackJack.MyWrite(ConsoleColor.Green, "End of Shuffle Standard\n\n");
        }

        // Draw Card
        public Card? Draw()
        {
            if (numCards <= 0)
            {
                BlackJack.MyWrite(ConsoleColor.Red,$"{this.GetType()} Trying to draw on Empty!\n");
                return null;
            }

            numCards--;
            return cards[numCards];
        }

        public virtual bool Insert(Card NewCard)
        {
            if (NumCards == MaxCards)
                return false;

            cards[NumCards] = NewCard;
            NumCards++;

            return true;
        }

        public virtual void DiscardAll(Deck Disc)
        {
            int i;

            for (i = 0; i < NumCards; i++)
                Disc.Insert(Cards[i]);

            NumCards = 0;
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
        public Shoe(int numDecks, bool DiscardShoe = false)
        {
            int i, j, cnt;

            // Set the Number of Decks
            this.numDecks = numDecks;

            decks = new Deck[numDecks];

            // Calculate the nuber of cards
            MaxCards = numDecks * Card.MaxRank * Card.MaxSuite;
            Cards = new Card[MaxCards];
            NumCards = 0;

            // Create the Decks and move their cards to the Shoe
            for (i = 0, cnt = 0; i < numDecks; i++)
            {
                decks[i] = new(DiscardShoe);
                if (DiscardShoe)
                    continue;
                for (j = 0; j < decks[i].NumCards; j++, cnt++)
                    this.Cards[cnt] = decks[i].Cards[j];
            }
            NumCards = cnt;
        }
    }

    public class Hand : Deck
    {
        public const int Card1 = 0;
        public const int Card2 = 1;
        public const int DlrCardUp = Card2;

        public const int MaxInHand = 21;

        private int handValue;
        private bool isDlr;

        private HStat hstat;    // Playing, sTand, suRRender, Bust
        private int hbet;
        private bool insured;   // Hand insured against Dealer's BJ

        public int HandValue
        {
            get { return handValue; }
            // set { handValue = value; }
        }
        public bool IsDlr
        {
            get { return isDlr; }
            // set { isDlr = value; }
        }
        public HStat Hstat
        {
            get { return hstat; }
            set { hstat = value; }
        }
        public int HBet
        {
            get { return hbet; }
            set { hbet = value; }
        }
        public bool Insured
        {
            get { return insured; }
            set { insured = value; }
        }

        // Regular constructor with no cards
        public Hand(int bet, bool DlrFlag = false)
        {
            // Reset Insurance
            Insured = false;

            // Store Dealer Flag
            isDlr = DlrFlag;
            Hstat = HStat.Playing;

            // Place bet
            HBet = bet;

            // Initialize the cards
            NumCards = 0;
            Cards = new Card[MaxInHand];

            handValue = 0;
        }

        public bool IsDbl()
        {
            if (NumCards != 2)
                return false;

            return Cards[0].Rank == Cards[1].Rank;
        }

        public bool IsInitialSoft()
        {
            if (NumCards != 2)
                return false;

            for (int i = 0; i < this.NumCards; i++)
                if (this.Cards[i].Rank == CardRank.Ace)
                    return true;

            return false;
        }

        public bool IsSoft()
        {
            for (int i = 0; i < this.NumCards; i++)
                if (this.Cards[i].Rank == CardRank.Ace)
                    return true;

            return false;
        }
        public bool IsBust()
        {
            return HandValue > BlackJack.BJMax;
        }
        public bool IsBJ()
        {
            return HandValue == BlackJack.BJMax;
        }
        public bool IsPlaying()
        {
            return Hstat == HStat.Playing;
        }
        public bool IsBJDlrStand()
        {
            return HandValue >= BlackJack.BJDlrMin;
        }
        public bool IsInsured()
        {
            return Insured;
        }

        public override String ToString(bool Show = false)
        {
            if (isDlr && !Show)
                return base.ToString(false) + "Dealer Up Card = " + Cards[DlrCardUp].ToString()
                    + " Card Value=" + Cards[DlrCardUp].Value();

            return (base.ToString(true) + " HandValue=" + HandValue + " Hstat=" + Hstat + " Bet=" + HBet +
                ((Insured) ? "  Hand is Insured" : ""));
        }

        public override bool Insert(Card C)
        {
            bool soft = false;

            if (!base.Insert(C))
                return false;

            int i, v = 0;

            for (i = 0; i < NumCards; i++)
            {
                v += Cards[i].Value();
                if (Cards[i].Rank == CardRank.Ace)
                    soft = true;
            }

            if (!soft || (soft && (v + 10) > BlackJack.BJMax))
                handValue = v;
            else
                handValue = v + 10;

            if (handValue > BlackJack.BJMax)
                Hstat = HStat.Bust;
            else if (handValue == BlackJack.BJMax)
                Hstat = HStat.sTand;
            else
                Hstat = HStat.Playing;

            return true;
        }

        public override void DiscardAll(Deck Disc)
        {
            base.DiscardAll(Disc);

            handValue = 0;
            Hstat = HStat.Playing;
            HBet = 0;
            Insured = false;
        }
    }
}
