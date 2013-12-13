using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PegBoard.Core.Tests
{
    [TestClass]
    public class BoardTests
    {
        private const string Category = "PegBoard.Core - BoardTests";

        [TestMethod, TestCategory(Category)]
        public void CanPrintBoard()
        {
            Board board = NewBoard();
            Console.WriteLine(board.ToString());
        }

        [TestMethod, TestCategory(Category)]
        public void InitialBoardIsCorrect()
        {
            Board board = NewBoard();

            Assert.AreEqual(1, board.GetSpacesWithPegs().Count());
            Assert.AreEqual(14, board.PegCount);
        }

        [TestMethod, TestCategory(Category)]
        public void MovePegIsCorrect()
        {
            Board board = NewBoard();            
            Jump move = new Jump(new []{0,2}, new[]{0,1}, new []{0,0});
            Board newBoard = board.ExecuteJump(move);

            Assert.AreEqual(false, newBoard[0, 2].HasPeg);
            Assert.AreEqual(false, newBoard[0, 1].HasPeg);
            Assert.AreEqual(true, newBoard[0, 0].HasPeg);
            Assert.AreEqual(true, board.IsEquivalent(NewBoard())); // ensure original board is the same

            Console.WriteLine(newBoard.ToString());
        }

        [TestMethod, TestCategory(Category)]
        public void NewBoardsAreEquivalent()
        {
            Board b1 = NewBoard();
            Board b2 = NewBoard();

            Assert.AreEqual(true, b1.IsEquivalent(b2));
        }

        [TestMethod, TestCategory(Category)]
        public void DifferentBoardsAreNotEquivalent()
        {
            Jump jump = new Jump(new[]{2,0}, new[]{1,0}, new[]{0,0});

            Board b1 = NewBoard();
            Board b2 = NewBoard().ExecuteJump(jump);

            Assert.AreEqual(false, b1.IsEquivalent(b2));
        }

        private Board NewBoard()
        {
            return new Board(5);
        }
    }
}
