//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal class Pot
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

        internal Pot() {}

        internal decimal Scoop()
        {
            decimal chipAmount = ChipAmount;
            ChipAmount = 0; 
            return chipAmount;
        }

        public override string ToString()
        {
            return $"Amount of chips in pot: {ChipAmount:C}. ";
        }
    }
}