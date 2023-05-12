using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ce100_hw3_algo_lib_cs
{   //A-(Describe a scenario where the Bellman-Ford algorithm could be useful for the ride-sharing company)
    //The Bellman-Ford algorithm could be useful for the ride-sharing company when calculating the shortest path between
    //two locations in a complex city road network with potential negative weight edges, such as in cases where there is a road
    //closure or heavy traffic, causing the time to travel along a particular road to be longer than usual. In such scenarios,
    //the Bellman-Ford algorithm can still find the shortest path between two locations by considering the potential negative weights.


    public class Edge
    {
        public int Source { get; set; }
        public int Destination { get; set; }
        public int Weight { get; set; }
    }

    public class BellmanFord
    {
        private int verticesCount;
        private int[] distances;

        public BellmanFord(int verticesCount)
        {
            this.verticesCount = verticesCount;
            this.distances = new int[verticesCount];
        }

        public void ShortestPath(List<Edge> edges, int source)
        {
            for (int i = 0; i < verticesCount; i++)
            {
                distances[i] = int.MaxValue;
            }
            distances[source] = 0;

            for (int i = 1; i < verticesCount; i++)
            {
                foreach (Edge edge in edges)
                {
                    int u = edge.Source;
                    int v = edge.Destination;
                    int weight = edge.Weight;
                    if (distances[u] != int.MaxValue && distances[u] + weight < distances[v])
                    {
                        distances[v] = distances[u] + weight;
                    }
                }
            }

            // Check for negative-weight cycles
            foreach (Edge edge in edges)
            {
                int u = edge.Source;
                int v = edge.Destination;
                int weight = edge.Weight;
                if (distances[u] != int.MaxValue && distances[u] + weight < distances[v])
                {
                    throw new Exception("Graph contains negative-weight cycle");
                }
            }
        }

        public int GetDistance(int destination)
        {
            return distances[destination];
        }
    }
}
