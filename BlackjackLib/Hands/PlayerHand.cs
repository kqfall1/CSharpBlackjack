//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal class PlayerHand : Hand
    {
        internal Bet Bet; 
        
        internal bool CanDoubleDown
        {
            get
            {
                return UpCards.Count == 2 &&
                       Status is HandStatus.Drawing &&
                       Bet.ChipAmountRequiredToDoubleDown <= Player.ChipAmount;
            }
        }

        internal bool CanSplit
        {
            get
            {
                return HandType is HandType.Main &&
                       IsPocketPair &&
                       Bet.ChipAmount <= Player.ChipAmount; 
            }
        }

        internal readonly HandType HandType;

        internal string HandTypeString
        {
            get
            {
                return $"{HandType.ToString().ToLower()} hand";
            }
        }

        internal override bool IsBlackjack
        {
            get
            {
                return base.IsBlackjack &&
                       UpCards.Count == 2 &&
                       !Player.HasSplit; 
            }
        }

        private Bet insuranceBet;
        internal Bet InsuranceBet
        {
            get
            {
                return insuranceBet;
            }
            set
            {
                insuranceBet = value;
            }
        }

        internal bool IsPocketPair
        { 
            get
            {
                return UpCards.Count == 2 && UpCards[0].Rank == UpCards[1].Rank;
            }
        }

        internal readonly Player Player;

        internal PlayerHand(Bet bet, HandType handType, Player player) : base()
        {
            Bet = bet;
            this.HandType = handType;
            this.Player = player;
        }

        internal void DoubleDownOnBet(Dealer dealer)
        {
            if (!CanDoubleDown)
            {
                throw new InsufficientChipsException(Player, Bet.ChipAmount);
            }
            else if (Bet.DealerContributionAmount(Bet.ChipAmount * 2, PayoutRatio.MAIN_BET) > dealer.ChipAmount)
            {
                throw new InsufficientChipsException(dealer, Bet.ChipAmount * 2);
            }

            Player.RemoveChips(Bet.ChipAmountRequiredToDoubleDown);
            Bet.DoubleDown();
            dealer.Hit(this);
        }

        internal void PlaceMainBet(decimal chipAmount, Dealer dealer)
        {
            Status = HandStatus.Drawing;
            Bet = Player.CreateBet(dealer, chipAmount);
        }

        internal PlayerHand Split(Dealer dealer)
        {
            Card splitCard;
            PlayerHand splitHand;

            if (!CanSplit)
            {
                throw new InvalidOperationException($"You cannot split hand:\n\n\t{this.ToString()}\n");
            }
            else if (Bet.DealerContributionAmount(Bet.ChipAmount, PayoutRatio.MAIN_BET) > dealer.ChipAmount)
            {
                throw new InsufficientChipsException(dealer, Bet.ChipAmount);
            }

            splitCard = UpCards[1]; 
            UpCards.RemoveAt(1);
            dealer.Hit(this);
            Status = HandStatus.WaitingOnSplitHand; 

            splitHand = new PlayerHand(Player.CreateBet(dealer, Bet.ChipAmount), HandType.Split, Player);
            splitHand.UpCards.Add(splitCard);
            dealer.Hit(splitHand);
            splitHand.Status = HandStatus.Drawing;
            return splitHand; 
        }

        internal void Surrender()
        {
            if (!Player.CanSurrender)
            {
                throw new InvalidOperationException($"You cannot currently surrender your {HandTypeString}.");
            }

            Status = HandStatus.Surrendered;
        }

        public override string ToString()
        {
            return $"{base.ToString()}";
        }
    }
}