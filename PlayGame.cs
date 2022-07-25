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
        public static bool PlayGameMain(Shoe BJshoe, Hand Dlr, Player Plr, Shoe DiscShoe)
        {
            bool StillPlaying;

            int i;
            String input = "";               // Input

            Move NxtMove;

            // 1. Place bets

            // 2. Initial deal - Players, Dealer, Flip Up Card
            if (!GameInitialDeal(BJshoe, Dlr, Plr))
            {
                BlackJack.MyWrite(ConsoleColor.Red, "Stopping\n");
                return true;
            }

            // 3. Game Play loop
            do
            {
                // 3.1 Show Table Status
                ShowGameStatus(BJshoe, Dlr, Plr, DiscShoe);

                // 3.2 For each Player Hand
                for (i = Player.FirstPHand; i < Plr.NumPHands; i++)
                {
                    String inputMsg = "Your play - Hit, sTand, Double, sPlit, suRRender, Quit, Show";

                    // Skip non playing hands: HStat.Bust || HStat.sTand || HStat.suRRender
                    if (!Plr.Hands[i].IsPlaying())
                        continue;

                    // 3.2.1 Show Dlr vs. Specific Hand
                    NxtMove = BJstrat.NextMove(Dlr.Cards[Hand.DlrCardUp], Plr.Hands[i]);
                    WriteHandVsHand(ConsoleColor.White, Dlr, Plr.Hands[i], false, NxtMove);

                    // 3.2.2 Get Player Action for this hand - Hit, Stand, Double, Split, Surrender, Bust, Quit
                    if (Dlr.Cards[Hand.DlrCardUp].Rank == CardRank.Ace)
                    {
                        // If Dealer shows Ace offer Insurance
                        BlackJack.MyWrite(ConsoleColor.Yellow, "Dealer shows an Ace - you can use Insurance\n");
                        inputMsg += ", Insurance";
                    }
                    inputMsg += "? ";
                    intk.Cr<String>(inputMsg, out input);

                    // If Abort - Stop Game
                    if (input == "")
                    {
                        MyWrite(ConsoleColor.Red,"Abort Requested\n");
                        return true;
                    }

                    // 3.2.3 Execute action for this hand
                    if (!ExecutePlay(input, BJshoe, Dlr, Plr, i, DiscShoe))
                        return true;
                }

                // 3.3 Untill all hands Stand, Surrender, or Bust and NONE of the hands are Playing
                StillPlaying = Plr.IsStillPlaying();
            } while (StillPlaying);

            MyWrite(ConsoleColor.Green,"\nAll Hands finished playing.\n");

            // 4. Dealer Flip
            DealerFlip(BJshoe, Dlr);

            // 5. Win/Lose Accounting - Handle Insurance, Push = Tie, etc
            Plr.WinLoseTie(Dlr);
            
            // 6. Discard all used cards from Dealer and Player hands
            Dlr.DiscardAll(DiscShoe);
            Plr.DiscardAll(DiscShoe);

            return false;
        }

        static bool GameInitialDeal(Shoe BJshoe, Hand Dlr, Player Plr)
        {
            Card? C1, C2, NewC;

            // Initial deal - Players, Dealer, Flip Up Card

            // Deal two cards to the first hand of the Player
            C1 = BJshoe.Draw();
            C2 = BJshoe.Draw();
            if(!Plr.AddPHand(C1, C2))       // Create Players first hand and place a Bet on it
            {
                BlackJack.MyWrite(ConsoleColor.Red, "Could not create a hand or place a bet\n");
                return false;
            }

            // Deal two cards to the Dealer
            NewC = BJshoe.Draw();
            Dlr.Insert(NewC);
            NewC = BJshoe.Draw(); // Open Card
            Dlr.Insert(NewC);

            return true;
        }

        static bool ExecutePlay(String input, Shoe BJshoe, Hand Dlr, Player Plr, int HandI, Shoe DiscShoe)
        {
            Card C1, C2, NewC;

            // 4. Execute action
            switch (input)
            {
                case "h":
                case "H":   // Hit
                    MyWrite(ConsoleColor.Cyan,"Action is Hit\n");
                    NewC = BJshoe.Draw();
                    Plr.Hands[HandI].Insert(NewC);

                    if (Plr.Hands[HandI].IsBust())
                        MyWrite(ConsoleColor.Red,"Sorry, you are Bust: " + Plr.Hands[HandI].ToString() + "\n");
                    break;
                
                case "t":
                case "T":   // sTand
                    MyWrite(ConsoleColor.Cyan,"Action is Stand\n");
                    Plr.Hands[HandI].Hstat = HStat.sTand;
                    break;
                
                case "d":
                case "D":   // Double
                    MyWrite(ConsoleColor.Cyan,"Action is Double\n");
                    if (Plr.Hands[HandI].NumCards != 2)
                    {
                        MyWrite(ConsoleColor.Red, "You can Double only if you have exactly two cards\n");
                        break;
                    }
                    if (Plr.PBet > Plr.PCash)
                    {
                        MyWrite(ConsoleColor.Red, "You do not have enough Cash to Double\n");
                        break;
                    }

                    Plr.PCash -= Plr.PBet;
                    Plr.Hands[HandI].HBet += Plr.PBet;

                    NewC = BJshoe.Draw();
                    Plr.Hands[HandI].Insert(NewC);

                    if (Plr.Hands[HandI].IsBust())
                        MyWrite(ConsoleColor.Red, "Sorry, you are Bust: " + Plr.Hands[HandI].ToString() + "\n");
                    else
                    {
                        MyWrite(ConsoleColor.Cyan, "Your new hand is:" + Plr.Hands[HandI].ToString() + "\n");
                        MyWrite(ConsoleColor.Cyan, "You doubled so you now must Stand\n");
                        Plr.Hands[HandI].Hstat = HStat.sTand;
                    }
                    break;
                
                case "p":
                case "P":   // sPlit
                    MyWrite(ConsoleColor.Cyan,"Action is Split\n");
                    if (!Plr.Hands[HandI].IsDbl())
                    {
                        MyWrite(ConsoleColor.Red, "You can only Split if you have exactly two identical cards\n");
                        break;
                    }

                    if (Plr.PBet > Plr.PCash)
                    {
                        MyWrite(ConsoleColor.Red, "You do not have enough Cash to Split\n");
                        break;
                    }

                    C1 = Plr.Hands[HandI].Draw();       // Take one of the twin cards into C1
                    NewC = BJshoe.Draw();               // Draw another card into NewC
                    Plr.Hands[HandI].Insert(NewC);      // Insert it into the original hand
                    C2 = BJshoe.Draw();                 // Draw another card into C2
                    if(Plr.AddPHand(C1, C2))            // Create Players new hand from C1 and C2
                    {
                        MyWrite(ConsoleColor.Cyan, "Your two new hands are:" + Plr.Hands[HandI].ToString() + "\n");
                        MyWrite(ConsoleColor.Cyan, "AND                    " + Plr.Hands[Plr.NumPHands - 1].ToString() + "\n");
                        MyWrite(ConsoleColor.Cyan, "You will play them individualy\n");
                    } else
                    {
                        DiscShoe.Insert(C1);    // Discard the redundant cards
                        DiscShoe.Insert(C2);
                    }
                    break;

                case "i":
                case "I":
                    MyWrite(ConsoleColor.Yellow, "Action is Insurance\n");
                    if (Dlr.Cards[Hand.DlrCardUp].Rank != CardRank.Ace)
                    {
                        // If Dealer does not show Ace cannot do Insurance
                        BlackJack.MyWrite(ConsoleColor.Yellow, "Dealer does NOT show an Ace - you can NOT use Insurance\n");
                        break;
                    }
                    Plr.Hands[HandI].Insured = true;
                    break;

                case "r":
                case "R":   // suRRender - Still not implemented
                    MyWrite(ConsoleColor.Red, "Action is Surrender\n");
                    if (!Plr.Surrender(HandI))
                    {
                        MyWrite(ConsoleColor.Red, "You can Surrender only if you have exactly two cards\n");
                        break;
                    }
                    break;

                case "q":
                case "Q":   // Quit
                    MyWrite(ConsoleColor.Red,"Action is Quit\n");
                    return false;

                case "s":
                case "S":
                    // Show Status
                    ShowGameStatus(BJshoe, Dlr, Plr, DiscShoe);
                    break;

                default:
                    MyWrite(ConsoleColor.Red,"Do not understand \"" + input + "\"\n");
                    break;
            }

            return true;
        }

        static void DealerFlip(Shoe BJshoe, Hand Dlr)
        {
            Card NewC;

            MyWrite(ConsoleColor.Green,"Dealer flip: \n");
            MyWrite(ConsoleColor.Cyan,Dlr.ToString(true) + "\n\n");

            if (Dlr.IsBJDlrStand())
                MyWrite(ConsoleColor.Green, "Dealer Stands!\n");

            // Dealer Actions - Flip and Draw cards until 17 or more
            while (!Dlr.IsBJDlrStand() && !Dlr.IsBust())
            {
                NewC = BJshoe.Draw();
                Dlr.Insert(NewC);
                MyWrite(ConsoleColor.Cyan,"Dealer flip: " + Dlr.ToString(true) + "\n");
            }
            if (Dlr.IsBust())
                MyWrite(ConsoleColor.Red,"Dealer Bust!\n");
            MyWrite(ConsoleColor.Red, "\n");
        }
    }
}