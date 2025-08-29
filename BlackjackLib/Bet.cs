//Quinn Keenan, 301504914, 19/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public class Bet
    {
        private decimal chipAmount;
        public decimal ChipAmount
        {
            get
            {
                return chipAmount;
            }
            private set
            {
                chipAmount = value;
            }
        }

        internal readonly Pot Pot;

        internal Bet(decimal chipAmount, Pot pot)
        {
            ChipAmount = chipAmount; 
            Pot = pot;
            pot.ChipAmount = chipAmount; 
        }

        internal string DoubleChips()
        {
            ChipAmount *= 2;
            Pot.ChipAmount = ChipAmount;
            return $"You have doubled your original bet of {ChipAmount / 2:C} to {ChipAmount:C}."; 
        }

        internal decimal Payout(Dealer dealer, PayoutRatio payoutRatio)
        {
            decimal potAmount = Pot.Scoop();
            decimal payout = payoutRatio.PayoutMultiplier * potAmount;
            decimal dealerContribution = payout - potAmount; 
            dealer.ChipAmount -= dealerContribution;
            return payout; 
        }

        public override string ToString()
        {
            return $"{ChipAmount:C}, {Pot}"; 
        }
    }
}