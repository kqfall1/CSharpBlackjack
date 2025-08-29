//Quinn Keenan, 301504914, 18/08/2025

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    internal class InsufficientChipsException : Exception
    {
        public InsufficientChipsException(decimal amount, string entityName) : base($"{entityName} doesn't have enough chips to" +
                                                                                    $" bet {amount:C}.") {}
    }
}