using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegBoard
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting PegBoard solver...");

            var problem = new Problem();
            var algorithm = new DepthFirstSearchAlgorithm();
            var solutions = algorithm.Solve(problem);            

            Console.WriteLine("{0} total solutions found.", solutions.Count());
            Console.WriteLine("{0} solutions with 1 peg.", solutions.Count(s => s.PegCount == 1));
            Console.WriteLine("Min depth was {0} moves.", solutions.Min(s => s.Depth));
            Console.WriteLine("Max depth was {0} moves.", solutions.Max(s => s.Depth));
            Console.WriteLine("Ave depth was {0} moves.", solutions.Average(s => s.Depth));
            Console.WriteLine("{0} total visited nodes.", algorithm.VisitedNodeCount);
            Console.WriteLine("{0} seconds of execution time.", algorithm.EllapsedTimeInSeconds);
            Console.WriteLine("-------------------");
            Console.WriteLine(solutions.First(s => s.PegCount == 1));

            Console.ReadLine();
        }
    }
}
