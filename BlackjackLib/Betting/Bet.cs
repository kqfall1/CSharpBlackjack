//Quinn Keenan, 301504914, 19/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal class Bet
    {
        private decimal chipAmount;
        internal decimal ChipAmount
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

        internal decimal DoubleDownChipAmount
        {
            get
            {
                return ChipAmount * 2; 
            }
        }

        internal decimal InsuranceBetChipAmount
        {
            get
            {
                return ChipAmount / 2;
            }
        }

        internal readonly Pot Pot;

        internal Bet(decimal chipAmount, Pot pot)
        {
            ChipAmount = chipAmount; 
            Pot = pot;
            pot.ChipAmount = chipAmount; 
        }

        internal void DoubleDown()
        {
            ChipAmount = DoubleDownChipAmount;
            Pot.ChipAmount = DoubleDownChipAmount;
        }

        internal decimal Payout(Dealer dealer, PayoutRatio payoutRatio) 
        {
            decimal potAmount = Pot.Scoop();
            decimal payout = payoutRatio.PayoutMultiplier * potAmount;
            decimal dealerContribution = payout - potAmount; 
            dealer.RemoveChips(dealerContribution);
            return payout; 
        }

        public decimal PayoutAmount(PayoutRatio payoutRatio)
        {
            return payoutRatio.PayoutMultiplier * ChipAmount; 
        }
        public decimal PayoutAmountDoubleDown()
        {
            return PayoutRatio.MAIN_BET.PayoutMultiplier * DoubleDownChipAmount;
        }

        public override string ToString()
        {
            return $"{ChipAmount:C}"; 
        }
    }
}