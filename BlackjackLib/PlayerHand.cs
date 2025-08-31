//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public class PlayerHand : Hand
    {
        internal Bet Bet; 
        
        internal bool CanDoubleDown
        {
            get
            {
                return Bet.ChipAmount <= Player.ChipAmount; 
            }
        }

        internal bool CanSplit
        {
            get
            {
                return HandType is HandType.Main && IsPocketPair; 
            }
        }

        public bool CanSurrender
        {
            get
            {
                return UpCards.Count == 2; 
            }
        }

        public readonly HandType HandType;
        public string HandTypeString
        {
            get
            {
                return $"{HandType.ToString().ToLower()} hand";
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

        internal bool IsValid
        {
            get
            {
                return UpCards.Count > 0; 
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
            Player.RemoveChips(Bet.ChipAmount);
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

            splitCard = UpCards[1]; 
            UpCards.RemoveAt(1);
            dealer.Hit(this);
            Status = HandStatus.WaitingOnSplitHand; 

            splitHand = new PlayerHand(Player.CreateBet(dealer, Bet.ChipAmount), HandType.Split, Player);
            dealer.Hit(splitHand);
            splitHand.Status = HandStatus.Drawing;
            return splitHand; 
        }

        internal void Surrender()
        {
            if (!CanSurrender)
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