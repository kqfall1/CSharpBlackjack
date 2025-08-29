//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                //WHAT IF IT IS NULL????
                return Bet.ChipAmount <= Player.ChipAmount; 
            }
        }
        
        //ADD AN "IS-POCKET-PAIR" PROPERTY???

        internal bool CanSplit
        {
            get
            {
                return HandType is HandType.Dealt && UpCards.Count == 2 && UpCards[0].Rank == UpCards[1].Rank; 
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
    }
}