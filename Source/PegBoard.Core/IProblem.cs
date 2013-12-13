using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PegBoard
{
    /// <summary>
    /// Encapsulates the rules of the game and what actions can 
    /// be taken on the board.
    /// </summary>
    public interface IProblem
    {
        /// <summary>
        /// Gets the initial state of the game.
        /// </summary>
        /// <returns>The board.</returns>
        Board GetInitialState();

        /// <summary>
        /// Gets the assignments that are available for the
        /// provided board.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <returns>Available assignments for the board.</returns>
        IEnumerable<Assignment> GetDomains(Board board);
    }
}
