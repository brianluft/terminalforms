using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

// Data structures to match the JSON
public class TypesData
{
    public string[] structNames { get; set; } = Array.Empty<string>();
    public string[] classNames { get; set; } = Array.Empty<string>();
    public Dependency[] dependencies { get; set; } = Array.Empty<Dependency>();
}

public class Dependency
{
    public string source { get; set; } = "";
    public string target { get; set; } = "";
}

class Program
{
    static void Main(string[] args)
    {
        // Read and parse the JSON file
        string jsonPath = "../types.json";
        string jsonContent = File.ReadAllText(jsonPath);
        var typesData = JsonSerializer.Deserialize<TypesData>(jsonContent);

        if (typesData == null)
        {
            Console.WriteLine("Failed to parse JSON file");
            return;
        }

        // Get all type names
        var allTypes = typesData.structNames.Concat(typesData.classNames).ToHashSet();

        // Build adjacency list (what each type depends on)
        var dependencies = new Dictionary<string, HashSet<string>>();
        
        // Initialize all types
        foreach (var type in allTypes)
        {
            dependencies[type] = new HashSet<string>();
        }

        // Process dependencies
        foreach (var dep in typesData.dependencies)
        {
            if (allTypes.Contains(dep.source) && allTypes.Contains(dep.target))
            {
                dependencies[dep.source].Add(dep.target);
            }
        }

        // Topological sort with cycle detection
        var result = TopologicalSort(allTypes, dependencies);

        // Write result to file with dependencies
        string outputPath = "../type_order.txt";
        var outputLines = result.Select(typeName => 
        {
            var deps = dependencies[typeName];
            if (deps.Count == 0)
            {
                return typeName;
            }
            else
            {
                var depsList = string.Join(", ", deps.OrderBy(x => x));
                return $"{typeName} ({depsList})";
            }
        }).ToArray();
        
        File.WriteAllLines(outputPath, outputLines);
        
        Console.WriteLine($"Topological sort complete. Result written to {outputPath}");
        Console.WriteLine($"Total types processed: {result.Count}");
    }

    static List<string> TopologicalSort(HashSet<string> allTypes, Dictionary<string, HashSet<string>> dependencies)
    {
        var result = new List<string>();
        var inDegree = new Dictionary<string, int>();
        var queue = new Queue<string>();

        // Calculate in-degrees (how many dependencies each type has)
        foreach (var type in allTypes)
        {
            inDegree[type] = dependencies[type].Count;
        }

        // Find all types with no dependencies (in-degree 0)
        foreach (var type in allTypes)
        {
            if (inDegree[type] == 0)
            {
                queue.Enqueue(type);
            }
        }

        // Process types with no dependencies first
        var processed = new HashSet<string>();
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);
            processed.Add(current);

            // For each type that depends on the current type, reduce its in-degree
            foreach (var otherType in allTypes)
            {
                if (dependencies[otherType].Contains(current))
                {
                    inDegree[otherType]--;
                    if (inDegree[otherType] == 0 && !processed.Contains(otherType))
                    {
                        queue.Enqueue(otherType);
                    }
                }
            }
        }

        // Any remaining types are part of cycles
        var cyclicTypes = allTypes.Where(t => !processed.Contains(t)).ToList();
        if (cyclicTypes.Any())
        {
            Console.WriteLine($"Found {cyclicTypes.Count} types in cycles:");
            foreach (var type in cyclicTypes.OrderBy(x => x))
            {
                Console.WriteLine($"  {type}");
            }
            
            // Add cyclic types at the end, sorted alphabetically for consistency
            result.AddRange(cyclicTypes.OrderBy(x => x));
        }

        return result;
    }
}