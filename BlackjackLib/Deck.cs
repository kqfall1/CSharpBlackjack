//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal class Deck : IEnumerable<Card>
    {
        private List<Card> cards;

        private const byte CARDS_PER_RANK = 4; 
        private const byte CARDS_PER_SUIT = 13;
        internal const byte FULL_DECK_CARD_COUNT = 52;
        private readonly Random randomGenerator; 

        internal Deck()
        {
            cards = new List<Card>();
            randomGenerator = new Random();
            Shuffle(); 
        }

        internal Card DrawCard()
        {
            if (cards.Count == 0)
            {
                throw new EmptyDeckException(); 
            }
            
            Card card = cards[0];
            cards.RemoveAt(0);
            return card; 
        }

        public IEnumerator<Card> GetEnumerator() => cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal void Shuffle()
        {
            Card cardToSwap; 
            byte count; 
            byte randomSwapIndex; 
            byte rankIndex;
            byte suitIndex;

            cards.Clear(); 
            
            populateCardsList();
            shuffleCards();

            void populateCardsList()
            {
                for (rankIndex = 0; rankIndex < CARDS_PER_SUIT; rankIndex++)
                {
                    for (suitIndex = 0; suitIndex < CARDS_PER_RANK; suitIndex++)
                    {
                        cards.Add(new Card((Rank)rankIndex, (Suit)suitIndex));
                    }
                }
            }

            void shuffleCards()
            {
                for (count = (byte) (cards.Count() - 1); count > 0; count--)
                {
                    randomSwapIndex = (byte)randomGenerator.Next((byte)(count));
                    cardToSwap = cards[count];
                    cards[count] = cards[randomSwapIndex];
                    cards[randomSwapIndex] = cardToSwap;
                }
            }
        }

        public override string ToString()
        {
            return $"{cards.Count} cards remaining in the deck.";
        }
    }
}