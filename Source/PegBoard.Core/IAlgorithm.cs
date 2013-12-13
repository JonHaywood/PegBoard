using System.Collections.Generic;

namespace PegBoard
{
    /// <summary>
    /// Represents a class which can take a given problem and find all the resulting
    /// solutions.
    /// </summary>
    public interface IAlgorithm
    {
        IEnumerable<Solution> Solve(IProblem problem);
    }
}
