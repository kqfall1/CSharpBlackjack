//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public class Dealer : BlackjackEntity
    {
        private readonly Deck Deck;

        internal bool MustHit
        {
            get
            {
                return MainHand.Score < Game.DEALER_MINIMUM_SCORE_TO_STAND ||
                       (MainHand.CardValuesSum == 17 &&
                       MainHand.AceCount == 1); 
            }
        }

        internal Dealer() : base()
        {
            Deck = new Deck();
            MainHand = new DealerHand(this);
        }
        internal void Deal(PlayerHand playerMainHand) 
        {
            DealerHand dealerHand = MainHand as DealerHand;

            Hit(playerMainHand);
            Hit(dealerHand);
            Hit(playerMainHand);
            Hit(dealerHand);

            MainHand = dealerHand;
        }

        internal void Hit(Hand hand)
        {
            hand.UpCards.Add(Deck.DrawCard());

            if (hand.IsBlackjack)
            {
                hand.Status = HandStatus.Blackjack; 
            }
            else if (hand.IsBusted)
            {
                hand.Status = HandStatus.Busted;
            }
        }

        internal bool ShouldDealerPlay(Player player, PlayerHand playerMainHand)
        {
            return !player.IsPlaying &&
                   player.HasAnEligibleHandToInduceDealerPlay &&
                   !playerMainHand.IsBlackjack &&
                   playerMainHand.Status is not HandStatus.Surrendered && 
                   MainHand.Status == HandStatus.WaitingToDraw;
        }

        internal void Shuffle()
        {
            Deck.Shuffle(); 
        }

        public override string ToString()
        {
            return $"{base.ToString()}{Deck}"; 
        }
    }
}