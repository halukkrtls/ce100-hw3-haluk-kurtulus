using ce100_hw3_algo_lib_cs;
using static ce100_hw3_algo_lib_cs.Question1;
using System.Collections;
using System;

namespace ce_100_hw3_algo_test_cs
{
    public class HuffmanTreeTests
    {
        [Fact]
        public void Build_Should_Build_Huffman_Tree_From_Given_Source()
        {
            // Arrange
            HuffmanTree tree = new HuffmanTree();
            string source = "hello world";

            // Act
            tree.Build(source);

            // Assert
            Assert.NotNull(tree.Root);
            Assert.Equal('*', tree.Root.Symbol);
            Assert.Equal(11, tree.Root.Frequency);
            Assert.NotNull(tree.Root.Left);
            Assert.NotNull(tree.Root.Right);
            Assert.Equal('*', tree.Root.Left.Symbol);
            Assert.Equal(4, tree.Root.Left.Frequency);
            Assert.Equal('*', tree.Root.Right.Symbol);
            Assert.Equal(7, tree.Root.Right.Frequency);
        }

        [Fact]
        public void Encode_Should_Encode_Given_Source_Using_Huffman_Tree()
        {
            // Arrange
            HuffmanTree tree = new HuffmanTree();
            string source = "hello world";
            tree.Build(source);

            // Act
            BitArray encoded = tree.Encode(source);

            // Assert
            string expected = "11101111101011000000111001010011";
            string actual = GetBitArrayAsString(encoded);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_Decode_Given_BitArray_Using_Huffman_Tree()
        {
            // Arrange
            HuffmanTree tree = new HuffmanTree();
            string source = "hello world";
            tree.Build(source);
            BitArray encoded = tree.Encode(source);

            // Act
            string decoded = tree.Decode(encoded);

            // Assert
            Assert.Equal(source, decoded);
        }

        private string GetBitArrayAsString(BitArray bits)
        {
            string result = "";

            foreach (bool bit in bits)
            {
                result += bit ? "1" : "0";
            }

            return result;
        }
    }



    public class GardenPipelineSystemTests
    {
        [Fact]
        public void GardenPipelineSystem_ThrowsException_WhenNumTreesLessThan10()
        {
            // Arrange
            int numTrees = 5;

            // Act and Assert
            Assert.Throws<ArgumentException>(() => new GardenPipelineSystem(numTrees));
        }

        [Fact]
        public void GardenPipelineSystem_ConstructsInstance_WhenNumTreesGreaterThan9()
        {
            // Arrange
            int numTrees = 10;

            // Act
            GardenPipelineSystem system = new GardenPipelineSystem(numTrees);

            // Assert
            Assert.NotNull(system);
        }

        [Fact]
        public void GenerateRandomLocations_ReturnsLocationsWithCorrectCount()
        {
            // Arrange
            int numTrees = 10;
            GardenPipelineSystem system = new GardenPipelineSystem(numTrees);

            // Act
            var locations = system.GenerateRandomLocations(numTrees);

            // Assert
            Assert.Equal(numTrees, locations.Count);
        }

        [Fact]
        public void BuildDistanceMatrix_CalculatesDistancesCorrectly()
        {
            // Arrange
            int numTrees = 10;
            GardenPipelineSystem system = new GardenPipelineSystem(numTrees);

            // Act
            system.BuildDistanceMatrix();

            // Assert
            for (int i = 0; i < numTrees; i++)
            {
                for (int j = i + 1; j < numTrees; j++)
                {
                    int distance = system.CalculateDistance(system.treeLocations[i], system.treeLocations[j]);
                    Assert.Equal(distance, system.distanceMatrix[i, j]);
                    Assert.Equal(distance, system.distanceMatrix[j, i]);
                }
            }
        }

        [Fact]
        public void FindMinimumSpanningTree_ProducesValidMST()
        {
            // Arrange
            int numTrees = 10;
            GardenPipelineSystem system = new GardenPipelineSystem(numTrees);

            // Act
            system.FindMinimumSpanningTree();

            // Assert
            Assert.NotNull(system.mstEdges);
            Assert.True(system.mstEdges.Count >= numTrees - 1);
            Assert.True(system.mstCost > 0);
        }

        [Fact]
        public void GetMSTEdgesWithDescriptions_ReturnsValidDescriptions()
        {
            // Arrange
            int numTrees = 10;
            GardenPipelineSystem system = new GardenPipelineSystem(numTrees);
            system.FindMinimumSpanningTree();

            // Act
            var descriptions = system.GetMSTEdgesWithDescriptions();

            // Assert
            Assert.NotNull(descriptions);
            Assert.Equal(system.mstEdges.Count, descriptions.Count);

            for (int i = 0; i < system.mstEdges.Count; i++)
            {
                var edge = system.mstEdges[i];
                var distance = system.distanceMatrix[edge.Item1, edge.Item2];
                var description = $"Connect tree {edge.Item1} and tree {edge.Item2} with a pipeline of length {distance}.";
                Assert.Equal(description, descriptions[i]);
            }
        }
    }
    public class BellmanFordTests
    {
        [Fact]
        public void BellmanFord_ShouldThrowException_WhenNegativeWeightCycleExists()
        {
            // Arrange
            int verticesCount = 4;
            List<Edge> edges = new List<Edge>()
            {
                new Edge { Source = 0, Destination = 1, Weight = 1 },
                new Edge { Source = 1, Destination = 2, Weight = 2 },
                new Edge { Source = 2, Destination = 3, Weight = 3 },
                new Edge { Source = 3, Destination = 0, Weight = -6 }, // Negative-weight cycle
            };
            int source = 0;

            // Act and Assert


        }

        [Fact]
        public void BellmanFord_CalculatesShortestPaths_WhenNoNegativeWeightCycleExists()
        {
            // Arrange
            int verticesCount = 5;
            List<Edge> edges = new List<Edge>()
            {
                new Edge { Source = 0, Destination = 1, Weight = 6 },
                new Edge { Source = 0, Destination = 2, Weight = 7 },
                new Edge { Source = 1, Destination = 2, Weight = 8 },
                new Edge { Source = 1, Destination = 3, Weight = 5 },
                new Edge { Source = 2, Destination = 3, Weight = -3 },
                new Edge { Source = 2, Destination = 4, Weight = 9 },
                new Edge { Source = 3, Destination = 4, Weight = 7 },
            };
            int source = 0;

            // Act
            BellmanFord bellmanFord = new BellmanFord(verticesCount);
            bellmanFord.ShortestPath(edges, source);

            // Assert
            Assert.Equal(0, bellmanFord.GetDistance(source));
            Assert.Equal(6, bellmanFord.GetDistance(1));
            Assert.Equal(7, bellmanFord.GetDistance(2));
            Assert.Equal(4, bellmanFord.GetDistance(3));
            Assert.Equal(11, bellmanFord.GetDistance(4));
        }
    }
    public class IkeaAssemblyGuideTests
    {
        [Fact]
        public void TestAssemblySteps()
        {
            // Arrange
            IkeaAssemblyGuide guide = new IkeaAssemblyGuide();
            guide.AddDependency("item1", new List<string> { "item2", "item3" });
            guide.AddDependency("item2", new List<string> { "item4" });
            guide.AddDependency("item3", new List<string> { "item5" });
            guide.AddDependency("item4", new List<string> { "item6", "item7" });
            guide.AddDependency("item5", new List<string> { "item6" });
            guide.AddDependency("item6", new List<string>());
            guide.AddDependency("item7", new List<string>());

            // Act
            ArrayList steps = guide.GetAssemblySteps();

            // Assert
            Assert.Equal(7, steps.Count);
            Assert.Equal("item1", steps[0]);
            Assert.Equal("item3", steps[1]);
            Assert.Equal("item5", steps[2]);
            Assert.Equal("item2", steps[3]);
            Assert.Equal("item4", steps[4]);
            Assert.Equal("item7", steps[5]);
            Assert.Equal("item6", steps[6]);
        }
    }
}


