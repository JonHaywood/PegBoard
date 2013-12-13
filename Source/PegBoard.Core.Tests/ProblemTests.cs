using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PegBoard.Core.Tests
{
    [TestClass]
    public class ProblemTests
    {
        private const string Category = "PegBoard.Core - ProblemTests";

        [TestMethod, TestCategory(Category)]        
        public void GetDomainsIsCorrectForEmptyBoard()
        {            
            var problem = new Problem();
            var board = problem.GetInitialState();
            var domains = problem.GetDomains(board).ToList();

            Assert.AreEqual(2, domains.Count);

            Console.WriteLine(domains[0].Board);
            Console.WriteLine(domains[1].Board);
        }
    }
}
