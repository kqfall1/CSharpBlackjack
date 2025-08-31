//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public abstract class Hand
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

        public virtual bool IsBlackjack
        {
            get
            {
                if (Score is Game.BUST_SCORE_LIMIT && AceCount == 1 && UpCards.Count == 2)
                {
                    return true; 
                }

                return false; 
            }
        }

        public virtual bool IsBusted
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
        public HandStatus Status
        {
            get
            {
                return status;
            }
            internal set
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
                cardValuesSum -= Card.ACE_VALUES_DIFFERENCE;
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