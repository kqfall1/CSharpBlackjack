//Quinn Keenan, 301504914, 19/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal class Game
    {
        internal const byte BUST_SCORE_LIMIT = 21;
        internal const byte DEALER_MINIMUM_SCORE_TO_STAND = 17;
        internal readonly BlackjackEntitiesController EntitiesController;

        internal bool IsActive
        {
            get
            {
                return EntitiesController.Dealer.ChipAmount > 0 && EntitiesController.Player.ChipAmount > 0;
            }
        }

        internal Game()
        {
            EntitiesController = new BlackjackEntitiesController();
        }

        internal void ResetAfterBettingRound()
        {
            EntitiesController.Dealer.MainHand = new DealerHand(EntitiesController.Dealer);
            EntitiesController.Player.MainHand = new PlayerHand(null, HandType.Main, EntitiesController.Player);
            EntitiesController.ActivePlayerHand = EntitiesController.Player.MainHand as PlayerHand;
            EntitiesController.Player.SplitHand = null; 
            EntitiesController.Dealer.Shuffle();
        }

        public override string ToString()
        {
            return $"{EntitiesController.Dealer}\n\n{EntitiesController.Player}";
        }
    }
}