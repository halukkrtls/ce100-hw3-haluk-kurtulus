using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ce100_hw3_algo_lib_cs
{
   public class GardenPipelineSystem
    {
        public int numTrees;
        public int[,] distanceMatrix;
        public List<Tuple<int, int>> mstEdges;
        public int mstCost;

        public List<Tuple<int, int>> treeLocations;

        public GardenPipelineSystem(int numTrees)
        {
            if (numTrees < 10)
                throw new ArgumentException("Minimum number of trees should be 10 or more.");

            this.numTrees = numTrees;
            this.distanceMatrix = new int[numTrees, numTrees];
            this.treeLocations = GenerateRandomLocations(numTrees);
            BuildDistanceMatrix();
            FindMinimumSpanningTree();
        }

        public List<Tuple<int, int>> GenerateRandomLocations(int numTrees)
        {
            List<Tuple<int, int>> locations = new List<Tuple<int, int>>();
            Random rnd = new Random();
            for (int i = 0; i < numTrees; i++)
            {
                int x = rnd.Next(100);
                int y = rnd.Next(100);
                Tuple<int, int> location = new Tuple<int, int>(x, y);
                locations.Add(location);
            }
            return locations;
        }

        public void BuildDistanceMatrix()
        {
            // Loop through each pair of trees in the list
            for (int i = 0; i < numTrees; i++)
            {
                for (int j = i; j < numTrees; j++)
                {
                    // Calculate the distance between the two trees using the CalculateDistance method
                    int distance = CalculateDistance(treeLocations[i], treeLocations[j]);

                    // Store the distance in both entries of the distance matrix
                    distanceMatrix[i, j] = distance;
                    distanceMatrix[j, i] = distance;
                }
            }
        }

        // This method calculates the Euclidean distance between two points in two-dimensional space
        public int CalculateDistance(Tuple<int, int> loc1, Tuple<int, int> loc2)
        {
            // Calculate the difference in x-coordinates and y-coordinates
            int xDiff = loc1.Item1 - loc2.Item1;
            int yDiff = loc1.Item2 - loc2.Item2;

            // Calculate the Euclidean distance between the two points
            // by taking the square root of the sum of squares of the x and y differences
            return (int)Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }

        public void FindMinimumSpanningTree()
        {
            // Create a list of all edges in the graph
            List<Tuple<int, int>> edges = new List<Tuple<int, int>>();
            for (int i = 0; i < numTrees; i++)
            {
                for (int j = i + 1; j < numTrees; j++)
                {
                    int weight = distanceMatrix[i, j];
                    if (weight > 0)
                    {
                        edges.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            // Sort the edges by weight in ascending order
            edges.Sort((x, y) => distanceMatrix[x.Item1, x.Item2] - distanceMatrix[y.Item1, y.Item2]);

            // Create an array to keep track of the ID of the tree that each node belongs to
            int[] treeIds = new int[numTrees];
            for (int i = 0; i < numTrees; i++)
            {
                treeIds[i] = i;
            }

            // Initialize an empty list of edges and cost of minimum spanning tree
            mstEdges = new List<Tuple<int, int>>();
            mstCost = 0;

            // Iterate through the edges, adding them to the MST if they do not create a cycle
            foreach (Tuple<int, int> edge in edges)
            {
                int u = edge.Item1;
                int v = edge.Item2;
                int uId = treeIds[u];
                int vId = treeIds[v];
                if (uId != vId)
                {
                    // Add the edge to the MST and update the total cost
                    mstEdges.Add(edge);
                    mstCost += distanceMatrix[u, v];

                    // Update the tree IDs of all nodes in the connected component
                    for (int i = 0; i < numTrees; i++)
                    {
                        if (treeIds[i] == vId)
                        {
                            treeIds[i] = uId;
                        }
                    }
                }
            }
        }
        public ArrayList GetMSTEdgesWithDescriptions()
        {
            ArrayList mstEdgesDescriptions = new ArrayList();
            foreach (Tuple<int, int> edge in mstEdges)
            {
                int u = edge.Item1;
                int v = edge.Item2;
                int distance = distanceMatrix[u, v];
                string description = $"Connect tree {u} and tree {v} with a pipeline of length {distance}.";
                mstEdgesDescriptions.Add(description);
            }
            return mstEdgesDescriptions;
        }
    }
}
