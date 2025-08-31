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
                return MainHand.Score < Game.DEALER_MINIMUM_SCORE_TO_STAND; 
            }
        }

        internal bool HasPlayed
        {
            get
            {
                DealerHand dealerMainHand = MainHand as DealerHand;
                
                return MainHand.IsBusted ||
                       dealerMainHand.AllCards.Count() > 2 ||
                       MainHand.Status is HandStatus.Standing;
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

        internal Card DealCard()
        {
            return Deck.DrawCard(); 
        }

        internal void Hit(Hand hand)
        {
            hand.UpCards.Add(DealCard());
        }

        internal bool ShouldDealerPlay(Player player, PlayerHand activePlayerHand)
        {
            return !player.IsPlaying &&
                   !activePlayerHand.IsBlackjack &&
                   !activePlayerHand.IsBusted &&
                   activePlayerHand.Status is not HandStatus.Surrendered && 
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