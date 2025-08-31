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

        internal bool IsPlaying
        {
            get
            {
                return MainHand.Status is HandStatus.Drawing || MainHand.Status is HandStatus.WaitingOnSplitHand; 
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
            if (dealer.ChipAmount < playerHand.Bet.DoubleDownChipAmount)
            {
                throw new InsufficientChipsException(dealer, playerHand.Bet.DoubleDownChipAmount);
            }
            else if (!playerHand.CanDoubleDown)
            {
                throw new InsufficientChipsException(this, playerHand.Bet.ChipAmount);
            }

            playerHand.DoubleDownOnBet(dealer);

            if (!playerHand.IsBusted)
            {
                playerHand.Status = HandStatus.Standing;
            }
        }

        internal PlayerHand Split(Dealer dealer)
        {
            PlayerHand mainHand = MainHand as PlayerHand;

            if (!mainHand.CanSplit)
            {
                throw new InvalidOperationException($"You cannot split hand:\n\n\t{mainHand.ToString()}");
            }

            return mainHand.Split(dealer);
        }

        public override string ToString()
        {
            return $"{base.ToString()}\n\nSplit hand: {SplitHand}\n\n";
        }
    }
}