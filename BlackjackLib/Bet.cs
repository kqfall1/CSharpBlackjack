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

        public decimal DoubleDownChipAmount
        {
            get
            {
                return ChipAmount * 2; 
            }
        }

        public decimal InsuranceBetChipAmount
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
            ChipAmount *= 2;
            Pot.ChipAmount = ChipAmount;
        }

        internal decimal Payout(Dealer dealer, PayoutRatio payoutRatio) //USE THIS WHETHER THE PLAYER WINS OR NOT!!! I MUST CREATE A STATIC READONLY PAYOUTRATIO FIRST
        {
            decimal potAmount = Pot.Scoop();
            decimal payout = payoutRatio.PayoutMultiplier * potAmount;
            decimal dealerContribution = payout - potAmount; 
            dealer.ChipAmount -= dealerContribution;
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