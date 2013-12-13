using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PegBoard
{
    public class DepthFirstSearchAlgorithm : IAlgorithm
    {
        private int recursionLevel = 0;
        private List<Solution> solutions = new List<Solution>();
        private HashSet<string> visitedNodes = new HashSet<string>();

        public int VisitedNodeCount
        {
            get { return visitedNodes.Count; }
        }

        public double EllapsedTimeInSeconds { get; private set; }        

        public IEnumerable<Solution> Solve(IProblem problem)
        {            
            recursionLevel = 0;
            visitedNodes.Clear();
            solutions.Clear();

            Board initialState = problem.GetInitialState();

            var watch = Stopwatch.StartNew();
            SolveRecursive(problem, initialState, initialState, new AssignmentCollection());
            watch.Stop();
            EllapsedTimeInSeconds = watch.Elapsed.TotalSeconds;

            return solutions;
        }

        private void SolveRecursive(IProblem problem, Board initial, Board board, IAssignmentCollection assignments)
        {
            var availableAssignments = problem.GetDomains(board).ToList();

            // if there's no more assignments the game is done, save, then go back up the chain
            if (availableAssignments.Count == 0)
            {
                solutions.Add(new Solution(initial, assignments));
                return;
            }

            foreach (var assignment in availableAssignments)
            {
                // keep a list of visited nodes. if we've seen this
                // before then skip it, no need to explore it again
                string hashCode = assignment.Board.ToString(false);
                if (visitedNodes.Contains(hashCode))
                    continue;
                else
                    visitedNodes.Add(hashCode);

                // add assignment to previous lsit
                var newAssignments = assignments.Clone();
                newAssignments.Add(assignment);

                // recurse down
                recursionLevel++;
                SolveRecursive(problem, initial, assignment.Board, newAssignments);
                recursionLevel--;
            }
        }
    }
}
