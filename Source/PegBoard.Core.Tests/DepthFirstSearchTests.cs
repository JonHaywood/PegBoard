using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PegBoard.Core.Tests
{
    [TestClass]
    public class DepthFirstSearchTests
    {
        private const string Category = "PegBoard.Core - DepthFirstSearchTests";

        [TestMethod, TestCategory(Category)]
        public void SolveReturnsAllSolutions()
        {
            var problem = new Problem();
            var algorithm = new DepthFirstSearchAlgorithm();

            var solutions = algorithm.Solve(problem);
        }
    }
}
