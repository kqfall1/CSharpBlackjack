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
        internal bool MustHit
        {
            get
            {
                return MainHand.Score < 17; 
            }
        }

        internal Dealer() : base()
        {
            MainHand = new DealerHand(this);
        }
    }
}