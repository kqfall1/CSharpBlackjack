//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public class DealerHand : Hand
    {
        internal override byte AceCount
        {
            get
            {
                byte aceCount = base.AceCount; 

                if (DownCard is not null && DownCard.Rank is Rank.Ace)
                {
                    aceCount++; 
                }

                return aceCount;
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

                return new List<Card>(UpCards) { DownCard }.ToArray();
            }
        }

        internal override byte CardValuesSum
        {
            get
            {
                if (DownCard is null)
                {
                    return base.CardValuesSum;
                }
                
                return (byte) (base.CardValuesSum + DownCard.Value); 
            }
        }

        public override bool IsBlackjack
        {
            get
            {
                return Score is Game.BUST_SCORE_LIMIT && AceCount == 1 && AllCards.Length == 2; 
            }
        }

        internal readonly Dealer Dealer; 
        
        private Card downCard;
        internal Card DownCard
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

        public override bool IsBusted
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