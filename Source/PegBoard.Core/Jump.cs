using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PegBoard
{
    /// <summary>
    /// Represents a potential move on the board.
    /// </summary>
    public class Jump : BaseObject
    {
        public Jump(Coordinate current, Coordinate jumped, Coordinate target)
        {
            Check.Require(current != null, "current is a required argument.");
            Check.Require(jumped != null, "jumped is a required argument.");
            Check.Require(target != null, "target is a required argument.");

            Current = current;
            Jumped = jumped;
            Target = target;
        }
        
        [DomainSignature]
        public Coordinate Current { get; private set; }
        [DomainSignature]
        public Coordinate Jumped { get; private set; }
        [DomainSignature]
        public Coordinate Target { get; private set; }

        public override string ToString()
        {
            return string.Format("Peg at {0} jumps peg {1} to space {2}", Current, Jumped, Target);
        }
    }
}
