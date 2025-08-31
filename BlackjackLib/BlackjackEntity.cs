//Quinn Keenan, 301504914, 19/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public abstract class BlackjackEntity
    {  
        private decimal chipAmount;
        public decimal ChipAmount
        {
            get
            {
                return chipAmount;
            }
            internal set
            {
                chipAmount = value;
            }
        }

        private Hand mainHand;
        public Hand MainHand
        {
            get
            {
                return mainHand;
            }
            internal set
            {
                mainHand = value;
            }
        }

        protected BlackjackEntity()
        {
            ChipAmount = Game.INITIAL_CHIP_AMOUNT; 
        }

        internal void AddChips(decimal amount)
        {
            ChipAmount += amount; 
        }

        internal void RemoveChips(decimal chipAmount)
        {
            ChipAmount -= chipAmount; 
        }

        public virtual string ToString()
        {
            return $"{GetType().Name}. Chip amount: {ChipAmount}.\n\nMain hand:\n\n{MainHand}\n\n";
        }
    }
}