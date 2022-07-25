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
        public const int BJMax = 21;
        public const int BJDlrMin = 17;
        public const int BJShoeDecks = 5;
        public const int BJMinShoeCards = 20;

        public static ConsoleColor OrigCol;

        public static Intake intk = new();
        public static readonly Random rnd = new();
        public static Strategy BJstrat = new();

        public static int HouseCash = 1000000;   // Starting House Cash is 1,000,000

        static void Main(String[] argv)
        {
            bool StopGame = false;      // Stop game flag

            String input;

            Shoe BJshoe;                    // The Shoe for the BJ game
            Shoe DiscShoe;                  // Discraded Shoe

            Player Plr;          // The Player
            Hand Dlr;           // The Dealer

            OrigCol = Console.ForegroundColor;

            MyWrite(OrigCol,"BlackJack - Hello - Igal Sapir\n");

            // Testing
            // TestCards();
            // TestStrategy();

            // Admit the player
            Plr = new("Igal", 1000, 10);
            // Define the Dealer
            Dlr = new(0,true);  // Dealer does not need to bet

            // Initiate the BJ shoes
            InitiateShoe(out BJshoe, out DiscShoe);
            
            // Game Main loop
            while (!StopGame)
            {
                // Renew BJshoe and DiscShoe if needed
                if (BJshoe.NumCards < BJMinShoeCards)
                {
                    MyWrite(ConsoleColor.White, "Black Jack Shoe has only " + BJshoe.NumCards + " left\n");
                    MyWrite(ConsoleColor.White, "Preparing a new Black Jack Shoe from " + BJShoeDecks + " fresh decks\n\n");

                    // Initiate the BJ shoes
                    InitiateShoe(out BJshoe, out DiscShoe);
                }

                StopGame = PlayGameMain(BJshoe, Dlr, Plr, DiscShoe);

                MyWrite(ConsoleColor.DarkGreen, "Game Finished\n");
                SanityCheck(BJshoe, Dlr, Plr, DiscShoe);

                MyWrite(ConsoleColor.White, "Dear " + Plr.PName + ",  you have " + Plr.PCash + " and bets are " + Plr.PBet + "\n");
                intk.Cr<String>("Would you like to Stop?", out input);

                if (input == "y" || input == "Y")
                    StopGame = true;
            }

            ShowGameStatus(BJshoe, Dlr, Plr, DiscShoe, true);

            MyWrite(OrigCol,"Black Jack - Good bye\n");
        }

        static void InitiateShoe(out Shoe BJshoe, out Shoe DiscShoe)
        {
            // Initiate the BJ shoes
            BJshoe = new(BJShoeDecks);              // The BJ shoe for the BJ game - 5 Decks
            DiscShoe = new(BJShoeDecks, true);      // The Discarded shoe - 5 Decks

            // Shuffle
            BJshoe.ShuffleStandard();
        }
        static void ShowGameStatus(Shoe S, Hand Dlr, Player Plr, Shoe DiscS, bool ShowFlag = false)
        {
            int i;
            Move NxtMove;
            int PCards;

            MyWrite(ConsoleColor.Green, "Table Status\n");
            // Dealer Status
            MyWrite(ConsoleColor.Green, "Dealer: " + Dlr.ToString(ShowFlag) + "\n\n");

            // Player Status
            MyWrite(ConsoleColor.White, Plr.Id + ": " + Plr.PName + " has " + Plr.PCash + " and bets are " + Plr.PBet + "\n");
            for (i = Player.FirstPHand, PCards = 0; i < Plr.NumPHands; i++)
            {
                NxtMove = BJstrat.NextMove(Dlr.Cards[Hand.DlrCardUp], Plr.Hands[i]);
                MyWrite(ConsoleColor.Green, "Player: " + ((Plr.Hands[i] == null) ? "null" : Plr.Hands[i].ToString()) +
                    $" (Hint: {NxtMove})");
                PCards += Plr.Hands[i].NumCards;
            }
            MyWrite(ConsoleColor.Green,"\n");

            SanityCheck(S, Dlr, Plr, DiscS);
        }

        public static void MyWrite(ConsoleColor Col, String Msg)
        {
            ConsoleColor PrevColor = Console.ForegroundColor;

            Console.ForegroundColor = Col;
            Console.Write(Msg);
            Console.ForegroundColor = PrevColor;
        }

        public static void WriteHandVsHand(ConsoleColor Col, Hand Dlr, Hand PHand, bool Showflag, Move NxtMove = Move.No)
        {
            MyWrite(Col, "Dealer: " + Dlr.ToString(Showflag) + "\n");
            MyWrite(Col, "\n");
            MyWrite(Col, "Player: " + ((PHand == null) ? "null" : PHand.ToString()) + 
                ((NxtMove == Move.No) ? "" : $" (Hint: {NxtMove})") + "\n");
        }

        public static void SanityCheck(Shoe S, Hand Dlr, Player Plr, Shoe DiscS)
        {
            int i, PCards;

            for (i = Player.FirstPHand, PCards = 0; i < Plr.NumPHands; i++)
                PCards += Plr.Hands[i].NumCards;

            MyWrite(ConsoleColor.DarkGreen, $"Shoe({S.NumCards}) + Dealer({Dlr.NumCards}) + Player({PCards})" +
                $" Discarded({DiscS.NumCards}) IsComplete is ");
            MyWrite(ConsoleColor.Green, "" + S.IsComplete(S.NumDecks, Dlr, Plr, DiscS) + "\n\n");
        }
    }

    // Suits: clubs ♣, diamonds ♦, hearts ♥, spades ♠
    public enum CardSuite { None = -1, Club = 1, Diamond, Heart, Spade };

    public enum CardRank { None = -1, Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };

    public enum Move { No = -3, Lose = -2, Win = -1, sTnd = 1, Dbl, sPlt, Hit };

    public enum HStat { Playing, sTand, suRRender, Bust };
}
