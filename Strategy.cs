using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    internal class Strategy
    {
        readonly private Move[][] DblStrat =
        {
new Move[] { Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No },                     // 0 Fake
new Move[] { Move.No,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt }, // A
new Move[] { Move.No,Move.Hit,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.Hit,Move.Hit,Move.Hit },     // 2
new Move[] { Move.No,Move.Hit,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.Hit,Move.Hit,Move.Hit },     // 3
new Move[] { Move.No,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.sPlt,Move.sPlt,Move.Hit,Move.Hit,Move.Hit,Move.Hit },         // 4
new Move[] { Move.No,Move.Hit,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Hit },           // 5
new Move[] { Move.No,Move.Hit,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.Hit,Move.Hit,Move.Hit,Move.Hit },      // 6
new Move[] { Move.No,Move.Hit,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.Hit,Move.Hit,Move.Hit },     // 7
new Move[] { Move.No,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt }, // 8
new Move[] { Move.No,Move.sTnd,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sPlt,Move.sTnd }, // 9
new Move[] { Move.No,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd }, // 10
new Move[] { Move.No,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd }, // J
new Move[] { Move.No,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd }, // Q
new Move[] { Move.No,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd }  // K
        };

        readonly private Move[][] SoftStrat =
        {
new Move[] { Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No },                     // 0 Fake
new Move[] { Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No },                     // A Fake
new Move[] { Move.No,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Dbl,Move.Dbl,Move.Hit,Move.Hit,Move.Hit,Move.Hit },           // 2
new Move[] { Move.No,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Dbl,Move.Dbl,Move.Hit,Move.Hit,Move.Hit,Move.Hit },           // 3
new Move[] { Move.No,Move.Hit,Move.Hit,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Hit,Move.Hit,Move.Hit,Move.Hit },           // 4
new Move[] { Move.No,Move.Hit,Move.Hit,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Hit,Move.Hit,Move.Hit,Move.Hit },           // 5
new Move[] { Move.No,Move.Hit,Move.Hit,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Hit,Move.Hit,Move.Hit,Move.Hit },           // 6
new Move[] { Move.No,Move.Hit,Move.sTnd,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.sTnd,Move.sTnd,Move.Hit,Move.Hit },        // 7 
new Move[] { Move.No,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd }, // 8
new Move[] { Move.No,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd }, // 9
new Move[] { Move.No,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win },           // 10
new Move[] { Move.No,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win },           // J
new Move[] { Move.No,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win },           // Q
new Move[] { Move.No,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win,Move.Win }            // K
        };

        readonly private Move[][] BasicStrat =
        {
new Move[] { Move.No, Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No },    // 0 Fake
new Move[] { Move.No, Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No },    // 1 Fake
new Move[] { Move.No, Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No },    // 2 Fake
new Move[] { Move.No, Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No },    // 3 Fake
new Move[] { Move.No, Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No,Move.No },    // 4 Fake
new Move[] { Move.No, Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit },          // 5
new Move[] { Move.No, Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit },          // 6
new Move[] { Move.No, Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit },          // 7
new Move[] { Move.No, Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit,Move.Hit },          // 8
new Move[] { Move.No, Move.Hit,Move.Hit,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Hit,Move.Hit,Move.Hit,Move.Hit },          // 9
new Move[] { Move.No, Move.Hit,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Hit },          // 10
new Move[] { Move.No, Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl,Move.Dbl },          // 11
new Move[] { Move.No, Move.Hit,Move.Hit,Move.Hit,Move.sTnd,Move.sTnd,Move.sTnd,Move.Hit,Move.Hit,Move.Hit,Move.Hit },       // 12
new Move[] { Move.No, Move.Hit,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.Hit,Move.Hit,Move.Hit,Move.Hit },     // 13
new Move[] { Move.No, Move.Hit,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.Hit,Move.Hit,Move.Hit,Move.Hit },     // 14
new Move[] { Move.No, Move.Hit,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.Hit,Move.Hit,Move.Hit,Move.Hit },     // 15
new Move[] { Move.No, Move.Hit,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.Hit,Move.Hit,Move.Hit,Move.Hit },      // 16
new Move[] { Move.No, Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd }, // 17
new Move[] { Move.No, Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd }, // 18
new Move[] { Move.No, Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd }, // 19
new Move[] { Move.No, Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd,Move.sTnd }  // 20
       };

        public Move NextMove(Card DlrOpen, Hand Player)
        {
            if (Player.IsDbl())  // If Player has Double hand - Use Double strategy
                return DblStrat[(int) Player.Cards[Hand.Card1].Rank][DlrOpen.Value()];

            CardRank SoftRank;
            if (Player.IsSoft())  // If Player has Soft hand - Use Soft strategy
            {
                SoftRank = (Player.Cards[Hand.Card1].Rank == CardRank.Ace)
                    ? Player.Cards[Hand.Card2].Rank
                    : Player.Cards[Hand.Card1].Rank;
                return SoftStrat[(int) SoftRank][DlrOpen.Value()];
            }

            int HandValue = Player.HandValue;
            return BasicStrat[HandValue][DlrOpen.Value()];  // Otherwise - Use Basic strategy
        }

    }
}
