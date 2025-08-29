using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string inputStr) : base($"Input \"{inputStr}\" is invalid.") {}
    }
}