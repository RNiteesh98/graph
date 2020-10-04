using GraphCore;
using GraphCore.Algorithms;
using GraphCore.Vertices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GraphTests.AlgorithmTests
{
    [TestFixture]
    public class AlgorithmLibraryTests
    {
        [Test]
        public void CreateAlgorithmLibraryTest()
        {
            Graph graph = new Graph();

            Assert.IsNotNull(graph.AlgorithmLibrary);
        }

        [Test]
        public void InBuiltAlgorithmsAreRegisteredTest()
        {
            Graph graph = new Graph();

            string[] algorithmNames = this.GetAlgorithmNames();
            
            foreach(string name in algorithmNames)
            {
                IAlgorithm algorithm = graph.AlgorithmLibrary.GetAlgorithm(name);
                Assert.IsNotNull(algorithm);
            }
        }

        [Test]
        public void RegisterAndUnregisterCustomAlgorithmTest()
        {
            Graph graph = new Graph();

            IAlgorithm testAlgorithm = new TestAlgorithm();
            graph.AlgorithmLibrary.RegisterAlgorithm(testAlgorithm);

            Assert.IsTrue(graph.AlgorithmLibrary.ContainsAlgorithm(testAlgorithm.Name));

            IAlgorithm retrievedAlgorithm = graph.AlgorithmLibrary.GetAlgorithm(testAlgorithm.Name);
            Assert.AreEqual(testAlgorithm, retrievedAlgorithm);

            bool result = graph.AlgorithmLibrary.UnregisterAlgorithm(testAlgorithm);
            Assert.IsTrue(result);
            Assert.IsFalse(graph.AlgorithmLibrary.ContainsAlgorithm(testAlgorithm.Name));
        }

        [Test]
        public void RegisterExistingAlgorithmTest()
        {
            Graph graph = new Graph();

            IAlgorithm dijkstra = new DijkstraCompleteTraversalAlgorithm();

            Assert.Throws<ArgumentException>(() =>
            {
                graph.AlgorithmLibrary.RegisterAlgorithm(dijkstra);
            });
        }

        [Test]
        public void GetNonExistingAlgorithmTest()
        {
            Graph graph = new Graph();

            Assert.Throws<ArgumentException>(() =>
            {
                graph.AlgorithmLibrary.GetAlgorithm("not registered");
            });
        }

        [Test]
        public void UnregisterNonExistingAlgorithmTest()
        {
            Graph graph = new Graph();

            TestAlgorithm testAlgorithm = new TestAlgorithm();
            bool result = graph.AlgorithmLibrary.UnregisterAlgorithm(testAlgorithm);

            Assert.IsFalse(result);
        }

        [Test]
        public void ExecuteAlgorithmTest()
        {
            Graph graph = new Graph();
            Vertex one = graph.GraphStructure.AddVertex(1);
            Vertex two = graph.GraphStructure.AddVertex(2);
            graph.GraphStructure.AddLine(one, two);

            TestAlgorithm testAlgorithm = new TestAlgorithm();
            graph.AlgorithmLibrary.RegisterAlgorithm(testAlgorithm);

            Vertex result = graph.AlgorithmLibrary.Execute<Vertex, Vertex>(testAlgorithm.Name, one);
            Assert.AreEqual(two, result);

            object resultAsObject = graph.AlgorithmLibrary.ExecuteBase(testAlgorithm.Name, one);
            Assert.AreEqual(two, resultAsObject);
        }

        [Test]
        public void ExecuteSeparateAlgorithmTest()
        {
            Graph graph = new Graph();
            Vertex one = graph.GraphStructure.AddVertex(1);
            Vertex two = graph.GraphStructure.AddVertex(2);
            graph.GraphStructure.AddLine(one, two);

            TestAlgorithm testAlgorithm = new TestAlgorithm();
            graph.AlgorithmLibrary.RegisterAlgorithm(testAlgorithm);

            Vertex result = testAlgorithm.Execute(graph.GraphStructure, one);
            Assert.AreEqual(two, result);

            object resultAsObject = testAlgorithm.ExecuteBase(graph.GraphStructure, one);
            Assert.AreEqual(two, resultAsObject);
        }

        [Test]
        public void ExecuteNonExistingAlgorithmTest()
        {
            Graph graph = new Graph();
            Vertex one = graph.GraphStructure.AddVertex(1);

            Assert.Throws<ArgumentException>(() =>
            {
                graph.AlgorithmLibrary.Execute<Vertex, Vertex>("Not registered", one);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                graph.AlgorithmLibrary.ExecuteBase("Not registered", one);
            });
        }

        [Test]
        public void ClearSetAttributesTest()
        {
            Graph graph = new Graph();
            Vertex one = graph.GraphStructure.AddVertex(1);
            Vertex two = graph.GraphStructure.AddVertex(2);
            graph.GraphStructure.AddLine(one, two);

            TestAlgorithm test = new TestAlgorithm();
            test.SetDynamicAttributesInStructure = true;
            graph.AlgorithmLibrary.RegisterAlgorithm(test);
            string visistedAttributeName = test.ReservedDynamicAttributeNames.First();

            graph.AlgorithmLibrary.ExecuteBase(test.Name, one);

            TestHelper.AssertAttributeValueForVertexIsSet(graph, one, visistedAttributeName, true);
            TestHelper.AssertAttributeValueForVertexIsSet(graph, two, visistedAttributeName, true);

            graph.AlgorithmLibrary.ClearDynamicAttributesSetByAlgorithm(test.Name);

            TestHelper.AssertAttributeValueForVertexIsNotSet(graph, one, visistedAttributeName);
            TestHelper.AssertAttributeValueForVertexIsNotSet(graph, two, visistedAttributeName);
        }

        [Test]
        public void ToggleSetAttributesTest()
        {
            Graph graph = new Graph();
            Vertex one = graph.GraphStructure.AddVertex(1);
            Vertex two = graph.GraphStructure.AddVertex(2);
            graph.GraphStructure.AddLine(one, two);

            TestAlgorithm test = new TestAlgorithm();
            graph.AlgorithmLibrary.RegisterAlgorithm(test);
            graph.AlgorithmLibrary.ToggleAlgorithmsShouldSetDynamicAttributesToItems(true);
            string visistedAttributeName = test.ReservedDynamicAttributeNames.First();

            graph.AlgorithmLibrary.ExecuteBase(test.Name, one);

            TestHelper.AssertAttributeValueForVertexIsSet(graph, one, visistedAttributeName, true);
            TestHelper.AssertAttributeValueForVertexIsSet(graph, two, visistedAttributeName, true);
        }

        private string[] GetAlgorithmNames()
        {
            Type algorithmNamesType = typeof(AlgorithmNames);

            List<string> names = new List<string>();

            FieldInfo[] fieldInfos = algorithmNamesType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo fi in fieldInfos)
            {
                if (fi.IsLiteral && !fi.IsInitOnly)
                {
                    names.Add(fi.GetValue(null).ToString());
                }
            }

            return names.ToArray();
        }
    }
}
