using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PegBoard.Core.Tests
{
    [TestClass]
    public class EquatableTests
    {
        private const string Category = "PegBoard.Core - EquatableTests";

        [TestMethod, TestCategory(EquatableTests.Category)]
        public void CanCompareVertexes()
        {
            var v1 = new Vertex(0, 1);
            var v2 = new Vertex(0, 1);
            Assert.AreEqual(v1, v2);
            Assert.IsTrue(v1 == v2);

            var v3 = new Vertex(1, 2);
            Assert.AreNotEqual(v1, v3);
            Assert.IsTrue(v1 != v3);
        }

        [TestMethod, TestCategory(EquatableTests.Category)]
        public void CanHashVertexes()
        {
            var dictionary = new Dictionary<Vertex, string>();
            dictionary[new Vertex(0, 1)] = "test1";
            dictionary[new Vertex(0, 2)] = "test2";

            var result = dictionary[new Vertex(0, 1)];
            Assert.AreEqual("test1", result);
        }
    }
}
