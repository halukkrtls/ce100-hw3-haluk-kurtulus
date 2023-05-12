using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ce100_hw3_algo_lib_cs
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class IkeaAssemblyGuide
    {
        private Dictionary<string, List<string>> itemDependencies;
        private HashSet<string> visited;
        private List<string> assemblySteps;

        public IkeaAssemblyGuide()
        {
            itemDependencies = new Dictionary<string, List<string>>();
            visited = new HashSet<string>();
            assemblySteps = new List<string>();
        }

        public void AddDependency(string item, List<string> dependencies)
        {
            itemDependencies[item] = dependencies;
        }

        public void BuildConnections()
        {
            foreach (var item in itemDependencies.Keys)
            {
                if (!visited.Contains(item))
                {
                    DFS(item);
                }
            }
        }
        private void DFS(string item)
        {
            // Create a HashSet to keep track of visited items
            visited.Add(item);

            // If the current item has dependencies, iterate through them
            if (itemDependencies.ContainsKey(item))
            {
                foreach (var dependency in itemDependencies[item])
                {
                    // If the dependency has not been visited, call the DFS method recursively with the dependency
                    if (!visited.Contains(dependency))
                    {
                        DFS(dependency);
                    }
                }
            }

            // Once all dependencies have been visited, add the current item to the assemblySteps list
            assemblySteps.Add(item);
        }

        public List<string> PerformTopologicalSorting()
        {
            BuildConnections();
            assemblySteps.Reverse();
            return assemblySteps;
        }

        public ArrayList GetAssemblySteps()
        {
            var steps = PerformTopologicalSorting();
            var assemblyStepsList = new ArrayList(steps);
            return assemblyStepsList;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Example usage
            IkeaAssemblyGuide guide = new IkeaAssemblyGuide();

            // Add item dependencies
            guide.AddDependency("item1", new List<string> { "item2", "item3" });
            guide.AddDependency("item2", new List<string> { "item4" });
            guide.AddDependency("item3", new List<string> { "item5" });
            guide.AddDependency("item4", new List<string> { "item6", "item7" });
            guide.AddDependency("item5", new List<string> { "item6" });
            guide.AddDependency("item6", new List<string>());
            guide.AddDependency("item7", new List<string>());

            // Get assembly steps
            ArrayList steps = guide.GetAssemblySteps();

            // Print assembly steps
            Console.WriteLine("Assembly Steps:");
            foreach (var step in steps)
            {
                Console.WriteLine(step);
            }
        }
    }

}
