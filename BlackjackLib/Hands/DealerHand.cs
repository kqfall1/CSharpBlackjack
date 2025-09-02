//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal class DealerHand : Hand
    {
        internal override byte AceCount
        {
            get
            {
                if (DownCard is not null && DownCard.Rank is Rank.Ace)
                {
                    return (byte) (base.AceCount + 1); 
                }

                return base.AceCount;
            }
        }
        
        internal Card[] AllCards
        {
            get
            {
                if (DownCard is null)
                {
                    return UpCards.ToArray();
                }

                return new List<Card>(UpCards){DownCard}.ToArray();
            }
        }

        internal override byte CardValuesSum
        {
            get
            {
                if (DownCard is not null)
                {
                    return (byte)(base.CardValuesSum + DownCard.Value);
                }

                return base.CardValuesSum;
            }
        }

        internal override bool IsBlackjack
        {
            get
            {
                return base.IsBlackjack && 
                       AllCards.Length == 2; 
            }
        }

        internal readonly Dealer Dealer; 
        
        private Card downCard;
        private Card DownCard
        {
            get
            {
                return downCard; 
            }
            set
            {
                downCard = value;
            }
        }

        internal override bool IsBusted
        {
            get
            {
                return Score > Game.BUST_SCORE_LIMIT; 
            }
        }

        internal override Card MostRecentlyDealtCard
        {
            get
            {
                if (AllCards.Length > 2)
                {
                    return base.MostRecentlyDealtCard; 
                }

                return DownCard; 
            }
        }

        internal override byte Score
        {
            get
            {
                return AlterScoreByReducingTheValueOfAces(CardValuesSum);
            }
        }

        internal DealerHand(Dealer dealer) : base()
        {
            Dealer = dealer;
        }

        public override string ToString()
        {
            if (DownCard is null)
            {
                return base.ToString();
            }
            
            return $"Dealer's down card: {DownCard}. {base.ToString()}";
        }
    }
}