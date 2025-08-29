using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLib
{
    public class EmptyDeckException : Exception
    {
        public EmptyDeckException() : base("Cannot draw cards from an empty deck!") {} 
    }
}