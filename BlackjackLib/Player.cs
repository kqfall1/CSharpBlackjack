//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public class Player : BlackjackEntity
    {
        public bool CanSurrender
        {
            get
            {
                return MainHand.UpCards.Count == 2 &&
                       SplitHand is null;
            }
        }

        internal bool HasAnEligibleHandToInduceDealerPlay
        {
            get
            {
                Hand[] hands = HandsInShowdownOrder;

                foreach (Hand hand in hands)
                {
                    if (!hand.IsBusted)
                    {
                        return true; 
                    }
                }

                return false; 
            }
        }
        
        internal PlayerHand[] HandsInShowdownOrder
        {
            get
            {
                if (SplitHand is null)
                {
                    return [MainHand as PlayerHand];
                }
                
                return [SplitHand, MainHand as PlayerHand];
            }
        }

        internal bool HasSplit
        {
            get
            {
                return SplitHand is not null;
            }
        }

        internal bool IsPlaying
        {
            get
            {
                return MainHand.Status is HandStatus.Drawing ||
                       SplitHand?.Status is HandStatus.Drawing;
            }
        }

        private PlayerHand splitHand; 
        public PlayerHand SplitHand
        {
            get
            {
                return splitHand;
            }
            internal set
            {
                splitHand = value;
            }
        }

        internal Player() : base()
        {
            MainHand = new PlayerHand(null, HandType.Main, this);
            SplitHand = null; 
        }

        internal Bet CreateBet(Dealer dealer, decimal chipAmount)
        {
            if (dealer.ChipAmount < chipAmount)
            {
                throw new InsufficientChipsException(dealer, chipAmount);
            }
            else if (chipAmount > ChipAmount)
            {
                throw new InsufficientChipsException(this, chipAmount);
            }

            RemoveChips(chipAmount);
            return new Bet(chipAmount, new Pot() { ChipAmount = chipAmount });
        }

        internal void DoubleDownOnBet(Dealer dealer, PlayerHand playerHand)
        {
            if (!playerHand.CanDoubleDown)
            {
                throw new InsufficientChipsException(this, playerHand.Bet.ChipAmount);
            }
            else if (playerHand.Bet.PayoutAmountDoubleDown() > dealer.ChipAmount)
            {
                throw new InsufficientChipsException(dealer, playerHand.Bet.ChipAmount);
            }

            playerHand.DoubleDownOnBet(dealer);
        }

        public override string ToString()
        {
            return $"{base.ToString()}\n\nSplit hand: {SplitHand}\n\n";
        }
    }
}