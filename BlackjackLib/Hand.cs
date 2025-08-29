//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    /* Encapsulates logic regarding the cards the player/dealer has and their game circumstances pertinent to their cards. This
    does not encapsulate behaviors that can be performed regarding a player's/dealer's cards and their game circumstances
    because this class has no object reference to the other necessary instansiations of other classes. The Game class does that
    instead. This was done to prevent tight coupling between classes and because Game should encapsulate all logic regarding the
    rules of the game. For example, one implementation could be an instance method such as Hit() in the Hand class by adding static fields
    to the Game class that Hand then refers to, but that tighly couples the two classes and undermines the purpose of the Game class. */
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

        internal virtual bool IsBlackjack
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

        internal virtual bool IsBusted
        {
            get
            {
                if (this is null)
                {
                    return false;
                }

                return Score > Game.BUST_SCORE_LIMIT; 
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

        private List<Card> upCards; 
        internal List<Card> UpCards //REMOVE THIS
        {
            get
            {
                return upCards;
            }
        }

        internal Hand()
        {
            upCards = new List<Card>();
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

        internal string Hit(Card card)
        {
            string cardStr = card.ToString();
            UpCards.Add(card);
            return cardStr; 
        }

        public virtual string ToString()
        {
            byte count; 
            string handStr = $"Up cards: ";

            for (count = 0; count < UpCards.Count; count++)
            {
                if (count == UpCards.Count - 1)
                {
                    handStr = $"{handStr}{UpCards[count]}.";
                }
                else
                {
                    handStr = $"{handStr}{UpCards[count]}, ";
                }
            }

            return $"{handStr} Score: {Score}"; 
        }
    }
}