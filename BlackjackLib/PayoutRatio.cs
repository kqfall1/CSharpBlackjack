//Quinn Keenan, 301504914, 19/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public class PayoutRatio
    {
        internal static readonly PayoutRatio INSURANCE_BET = new PayoutRatio(2, 1);
        internal static readonly PayoutRatio MAIN_BET = new PayoutRatio(2, 1);
        internal static readonly PayoutRatio BLACKJACK = new PayoutRatio(3, 2);
        internal static readonly PayoutRatio PUSH = new PayoutRatio(1, 1);
        internal static readonly PayoutRatio SURRENDER = new PayoutRatio(1, 2); 

        internal decimal PayoutMultiplier
        {
            get
            {
                return (decimal)PayoutValue / (decimal)RiskValue; 
            }
        }
        
        internal readonly byte PayoutValue; 
        internal readonly byte RiskValue;

        internal PayoutRatio(byte payoutValue, byte riskValue)
        {
            this.PayoutValue = payoutValue;
            this.RiskValue = riskValue;
        }

        public override string ToString()
        {
            return $"{PayoutValue}:{RiskValue} - pays {(decimal) PayoutValue / (decimal) RiskValue} times the bet amount.";
        }
    }
}