using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace PegBoard
{
    /// <summary>
    /// Class which represents a solution the board game. Contains the intial state of
    /// the board and all the assignments made to bring the board to the final state.
    /// Is immutable.
    /// </summary>
    public class Solution
    {       
        public Solution(Board initialState, IEnumerable<Assignment> assignments)
        {
            Check.Require(initialState != null, "initialState is a required argument.");
            Check.Require(assignments != null, "assignments is a required argument.");

            InitialState = initialState;
            Assignments = new ReadOnlyCollection<Assignment>(assignments.ToList());
            FinalState = Assignments.Last().Board;
            PegCount = FinalState.PegCount;
            Depth = Assignments.Count;
        }

        public Board InitialState { get; private set; }
        public ReadOnlyCollection<Assignment> Assignments { get; private set; }
        public Board FinalState { get; private set; }
        public int PegCount { get; private set; }
        public int Depth { get; private set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(InitialState.ToString());
            foreach (var assignment in Assignments)
            {
                builder.AppendLine(assignment.Jump.ToString());
                builder.AppendLine();
                builder.AppendLine(assignment.Board.ToString());
            }

            return builder.ToString();
        }
    }
}
