using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PegBoard
{
    /// <summary>
    /// Represents a position on the game board. Consists of a coordinate
    /// and a boolean indicating if the coordinate has a peg or not.
    /// Is immutable.
    /// </summary>
    public class Vertex : BaseObject, ICloneable<Vertex>
    {
        public Vertex(int x, int y, bool hasPeg = false)
            : this(new Coordinate(x, y), hasPeg)
        { }

        public Vertex(Coordinate coords, bool hasPeg = false)
        {
            Coords = coords;
            HasPeg = hasPeg;
        }

        /// <summary>
        /// Gets the X and Y coordinates.
        /// </summary>
        [DomainSignature]
        public Coordinate Coords { get; private set; }

        /// <summary>
        /// Gets whether this coordinate has a peg or not.
        /// </summary>
        public bool HasPeg { get; private set; }

        /// <summary>
        /// Returns a new vertex with the peg set to the specified value.
        /// </summary>
        public Vertex SetPeg(bool hasPeg)
        {
            return new Vertex(Coords.X, Coords.Y, hasPeg);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Vertex Clone()
        {
            return new Vertex(Coords.X, Coords.Y, HasPeg);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return Coords.ToString();
        }
    }
}
