//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public class Player : BlackjackEntity
    {
        internal List<PlayerHand> Hands
        {  
            get
            {
                return new List<PlayerHand>() { MainHand as PlayerHand, SplitHand };
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

        private bool insuranceBetWon; 
        internal bool InsuranceBetWon
        {
            get
            {
                return insuranceBetWon;
            }
            set
            {
                insuranceBetWon = value; 
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
            MainHand = new PlayerHand(null, HandType.Dealt, this);
        }

        internal Bet PlaceBet(decimal chipAmount, Pot pot)
        {
            if (chipAmount > ChipAmount)
            {
                throw new InsufficientChipsException(chipAmount, Game.BlackjackEntityToClassName(this));
            }

            ChipAmount -= chipAmount;
            return new Bet(chipAmount, pot);
        }
        internal string PlaceDoubleDownBet(PlayerHand playerHand)
        {
            if (!playerHand.CanDoubleDown)
            {
                throw new InsufficientChipsException(playerHand.Bet.ChipAmount, Game.BlackjackEntityToClassName(this));
            }

            ChipAmount -= playerHand.Bet.ChipAmount;
            return playerHand.Bet.DoubleChips();
        }

        public override string ToString()
        {
            return $"{base.ToString()}\n\nSplit hand: {SplitHand}\n\n";
        }
    }
}