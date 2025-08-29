//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public class Dealer : BlackjackEntity
    {
        private readonly Deck Deck;

        internal bool MustHit
        {
            get
            {
                return MainHand.Score < 17; 
            }
        }

        internal Dealer(Deck deck) : base()
        {
            Deck = deck;
            MainHand = new DealerHand(this);
        }

        internal Card DealCard()
        {
            return Deck.DrawCard(); 
        }

        internal void Shuffle()
        {
            Deck.Randomize(); 
        }

        public override string ToString()
        {
            return $"{base.ToString()}{Deck}"; 
        }
    }
}