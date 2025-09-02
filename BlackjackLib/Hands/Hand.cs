//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal abstract class Hand
    {
        internal virtual byte AceCount
        {
            get
            {
                byte aceCount = 0;

                foreach (Card card in UpCards)
                {
                    switch (card.Rank)
                    {
                        case Rank.Ace:
                            aceCount++;
                            break;
                    }
                }

                return aceCount;
            }
        }

        internal virtual byte CardValuesSum
        {
            get
            {
                byte cardValuesSum = 0;

                foreach (Card upCard in UpCards)
                {
                    cardValuesSum += upCard.Value; 
                }

                return cardValuesSum;
            }
        }

        internal virtual bool IsBlackjack
        {
            get
            {
                if (Score is Game.BUST_SCORE_LIMIT && 
                    AceCount == 1)
                {
                    return true; 
                }

                return false; 
            }
        }

        internal virtual bool IsBusted
        {
            get
            {
                return Score > Game.BUST_SCORE_LIMIT; 
            }
        }

        internal virtual Card MostRecentlyDealtCard
        {
            get
            {
                return UpCards[UpCards.Count - 1];
            }
        }

        internal virtual byte Score
        {
            get
            {
                return AlterScoreByReducingTheValueOfAces(CardValuesSum);
            }
        }

        private HandStatus status; 
        internal HandStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        internal List<Card> UpCards; 

        internal Hand()
        {
            UpCards = new List<Card>();
            Status = HandStatus.WaitingToDraw;
        }

        internal byte AlterScoreByReducingTheValueOfAces (byte cardValuesSum)
        {
            byte scoreAdjustmentCount = 0;

            while (cardValuesSum > 21 && scoreAdjustmentCount < AceCount)
            {
                cardValuesSum -= Card.DIFFERENCE_BETWEEN_HIGH_AND_LOW_ACE_VALUES;
                scoreAdjustmentCount++;
            }

            return cardValuesSum; 
        }

        internal void Stand()
        {
            Status = HandStatus.Standing;
        }

        public virtual string ToString()
        {
            byte count;
            string endingCharacters = ", "; 
            string handStr = $"Up cards: ";

            for (count = 0; count < UpCards.Count; count++)
            {
                if (count == UpCards.Count - 1)
                {
                    endingCharacters = "."; 
                }

                handStr = $"{handStr}{UpCards[count]}{endingCharacters}";
            }

            return $"{handStr} Score: {Score}."; 
        }
    }
}