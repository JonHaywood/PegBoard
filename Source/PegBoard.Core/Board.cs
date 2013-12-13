using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PegBoard
{
    /// <summary>
    /// Represents a game board. Is immutable.
    /// </summary>
    /// <remarks>
    /// Conceptually the board is arranged in typical x,y coordinate space with
    /// the starting point at 0,0 and a right angle between the x and y axes with
    /// y increases upward and x increasing to the right. For example:
    /// (0,5)| [y-axis]
    ///      |
    ///      |
    ///      |
    /// (0,0)|_ _ _ _ _ (5,0) [x-axis]
    /// </remarks>
    public class Board
    {
        private readonly Vertex[] spaces;
        private string boardString;

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// </summary>
        /// <param name="rank">The rank. Must</param>
        public Board(int rank)
        {
            Check.Require(rank > 3, "Rank must be greater than 3."); // must have a rank greater than 3 to have a playable game
            Rank = rank;
            var v = new List<Vertex>();
            for (int i = 0; i < rank; i++)
            {
                for (int x = 0, y = i; x <= i; x++, y--)
                {
                    bool hasPeg = (x == 0 && y == 0) ? false : true; // only the starting space has no peg
                    v.Add(new Vertex(x, y, hasPeg));
                }
            }
            spaces = v.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class using
        /// vertices from another board.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        private Board(IEnumerable<Vertex> vertices)
        {
            Check.Require(vertices != null, "vertices is a required argument and cannot be null.");
            Check.Require(vertices.Count() % 3 == 0, "Invalid number of vertices."); // make sure we have a triangle
            this.spaces = vertices.ToArray();
            this.Rank = spaces.Length/3;
        }

        /// <summary>
        /// Gets the rank of the game board. Typical is 5.
        /// </summary>
        /// <remarks>The rank is the number of levels in the game triangle.</remarks>
        public int Rank { get; private set; }

        /// <summary>
        /// Gets the number of pegs on the board.
        /// </summary>
        /// <returns>Current peg count.</returns>
        public int PegCount
        {
            get { return spaces.Count(s => s.HasPeg); }
        }

        /// <summary>
        /// Gets the vertex at the specified coordinates.
        /// </summary>        
        public Vertex this[Coordinate coord]
        {
            get
            {
                Check.Require(coord != null, "coord is a required argument.");
                return this[coord.X, coord.Y];
            }
        }

        /// <summary>
        /// Gets the vertex at the specified coordinates.
        /// </summary>
        public Vertex this[int x, int y]
        {
            get
            {
                Check.Require(x >= 0, "x must be greater than 0.");                
                Check.Require(y >= 0, "y must be greater than 0.");
                Check.Require(x < Rank, string.Format("x must be less than {0}.", Rank));
                Check.Require(y < Rank, string.Format("y must be less than {0}.", Rank));
                return spaces.Single(s => s.Coords.X == x && s.Coords.Y == y);
            }
        }

        /// <summary>
        /// Determines whether the provided coordinate is on the board or not.
        /// </summary>
        /// <param name="coordinate">Coordinate.</param>
        /// <returns>True if the coordinate is on the board, otherwise false.</returns>
        public bool ContainsCoordinate(int x, int y)
        {
            return ContainsCoordinate(new[] { x, y });
        }

        /// <summary>
        /// Determines whether the provided coordinate is on the board or not.
        /// </summary>
        /// <param name="coordinate">Coordinate.</param>
        /// <returns>True if the coordinate is on the board, otherwise false.</returns>
        public bool ContainsCoordinate(Coordinate coordinate)
        {
            return spaces.Any(s => s.Coords == coordinate);
        }


        /// <summary>
        /// Executes the provided jump on the board. Returns a new version of the board
        /// that has the executed jump.
        /// </summary>
        /// <param name="jump">The jump to execute.</param>
        /// <returns>New board with the moved pegs.</returns>
        public Board ExecuteJump(Jump jump)
        {               
            // find index of vertices
            int currentIndex = Array.FindIndex(spaces, s => s.Coords == jump.Current);
            int jumpedIndex = Array.FindIndex(spaces, s => s.Coords == jump.Jumped);
            int targetIndex = Array.FindIndex(spaces, s => s.Coords == jump.Target);

            // make sure the move is valid
            Check.Require(spaces[currentIndex].HasPeg == true, "current must be space with a peg.");
            Check.Require(spaces[jumpedIndex].HasPeg == true, "jumped must be space with a peg.");
            Check.Require(spaces[targetIndex].HasPeg == false, "target must be an empty space.");

            // make a clone of all spaces
            var newSpaces = spaces.Select(s => s.Clone()).ToArray();

            // perform the jump and change associated values
            newSpaces[currentIndex] = newSpaces[currentIndex].SetPeg(false);
            newSpaces[jumpedIndex] = newSpaces[jumpedIndex].SetPeg(false);
            newSpaces[targetIndex] = newSpaces[targetIndex].SetPeg(true);

            // create a new board
            return new Board(newSpaces);
        }        

        /// <summary>
        /// Returns true if the boards have the same positions
        /// and peg value. Otherwise false.
        /// </summary>
        /// <param name="board">The board to compare to this one.</param>
        /// <returns>True if equivalent, otherwise false.</returns>
        public bool IsEquivalent(Board board)
        {
            Check.Require(board != null, "board is a required argument and cannot be null.");
            if (this.Rank != board.Rank)
                return false;

            for (int i = 0; i < spaces.Length; i++)
            {
                if (spaces[i] != board.spaces[i] ||
                    spaces[i].HasPeg != board.spaces[i].HasPeg)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the spaces on the board that have a peg.
        /// </summary>
        /// <returns>Peg coordinates.</returns>
        public IEnumerable<Coordinate> GetSpacesWithPegs()
        {
            return spaces.Where(s => s.HasPeg).Select(s => s.Coords);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="formatted">True to include formatting otherwise default ToString() is called.</param>
        public string ToString(bool formatted)
        {
            return formatted
                ? Regex.Replace(ToString(), @"\s+", string.Empty)
                : ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            // since we are immutable, cache the string and resuse
            if (this.boardString != null)
                return this.boardString;

            // get the total spaces for the output
            int totalSpaces = (Rank*2) - 1;
            string format = "{0,-" + totalSpaces + "}";

            // generate string 
            var builder = new StringBuilder();
            for (int i = 0; i < Rank; i++)
            {
                var row = new List<string>();

                // get items on row (starts on the last row and goes down until the first coord)
                for (int j = 0; j < Rank - i; j++)                
                    row.Add(this[j, Rank - j - i - 1].HasPeg ? "1" : "0");
                string rowStr = string.Join(" ", row.ToArray());

                // the origin needs special consideration as it is off-center
                if (i == Rank - 1)
                    rowStr = rowStr + " ";

                // this will center the string based on the total amount of spaces
                builder.AppendFormat(format,
                    string.Format("{0," + ((totalSpaces + rowStr.Length)/2).ToString() + "}{1}", rowStr,
                        Environment.NewLine));
            }
            // save in cache
            this.boardString = builder.ToString();

            return this.boardString;
        }
    }
}
