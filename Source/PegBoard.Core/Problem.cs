using System.Collections.Generic;
using System.Linq;

namespace PegBoard
{
    public class Problem : IProblem
    {
        // starting from the west going counter-clockwise, indices of each 
        // cardinal direction to check. Note: there are no valid SW or NW 
        // jumps in the game so just take out those indices
        private readonly int[] xs = { -1, /*-1,*/  0, +1, +1, /*+1,*/  0, -1 };
        private readonly int[] ys = { 0, /*-1,*/ -1, -1, 0, /*+1,*/ +1, +1 };

        public virtual Board GetInitialState()
        {
            return new Board(5);
        }

        public virtual IEnumerable<Assignment> GetDomains(Board board)
        {
            var assignments = new List<Assignment>();
            var pegSpaces = board.GetSpacesWithPegs();

            foreach (var peg in pegSpaces)
            {
                var jumps = GetAvailableJumps(board, peg);
                if (jumps.Count > 0)                
                    assignments.AddRange(jumps.Select(jump => 
                        new Assignment(board.ExecuteJump(jump), jump)));                
            }
            return assignments;
        }

        private List<Jump> GetAvailableJumps(Board board, Coordinate peg)
        {
            List<Jump> jumps = new List<Jump>();

            // check each direction
            for (int i = 0; i < xs.Length; i++)
            {
                var jumped = new Coordinate(peg.X + (xs[i] * 1), peg.Y + (ys[i] * 1));
                var target = new Coordinate(peg.X + (xs[i] * 2), peg.Y + (ys[i] * 2));

                // check that the next coord over and the one after that exist
                // and that next has a peg to jump and the one after is empty
                if (board.ContainsCoordinate(jumped) &&
                    board.ContainsCoordinate(target) &&
                    board[jumped].HasPeg == true &&
                    board[target].HasPeg == false)
                    jumps.Add(new Jump(peg, jumped, target));
            }

            return jumps;
        }
    }
}
