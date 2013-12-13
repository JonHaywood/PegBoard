using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PegBoard
{
    public class Coordinate : BaseObject
    {
        private readonly int[] coords;

        public Coordinate(int x, int y) : this(new[]{x,y})
        { }

        public Coordinate(int[] coords)
        {
            Check.Require(coords != null, "coords is a required argument.");
            Check.Require(coords.Length == 2, "coords must be an array of 2 elements.");
            //Check.Require(coords[0] >= 0, "x coordinate must be greater than 0.");
            //Check.Require(coords[1] >= 0, "y coordinate must be greater than 0.");

            this.coords = coords;
        }

        [DomainSignature]
        public int X { get { return coords[0]; } }
        [DomainSignature]
        public int Y { get { return coords[1]; } }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }

        public static implicit operator int[](Coordinate coordinate)
        {
            return coordinate.coords;
        }

        public static implicit operator Coordinate(int[] coordinates)
        {
            return new Coordinate(coordinates);
        }
    }
}
