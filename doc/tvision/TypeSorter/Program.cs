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

        // Remove streaming types we don't care about
        var streamingTypes = new HashSet<string> { 
            "ipstream", "pstream", "TStreamable", "opstream", 
            "fpbase", "fpstream", "ifpstream", "iopstream", "ofpstream", "otstream",
            "StreamableInit"
        };
        allTypes.ExceptWith(streamingTypes);

        // Build adjacency list (what each type depends on)
        var dependencies = new Dictionary<string, HashSet<string>>();
        
        // Initialize all types
        foreach (var type in allTypes)
        {
            dependencies[type] = new HashSet<string>();
        }

        // Process dependencies (excluding self-dependencies and streaming types)
        foreach (var dep in typesData.dependencies)
        {
            if (allTypes.Contains(dep.source) && allTypes.Contains(dep.target) && 
                dep.source != dep.target && 
                !streamingTypes.Contains(dep.source) && !streamingTypes.Contains(dep.target))
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
        var stack = new Stack<string>();

        // Calculate in-degrees (how many dependencies each type has)
        foreach (var type in allTypes)
        {
            inDegree[type] = dependencies[type].Count;
        }

        // Find all types with no dependencies (in-degree 0) and add to stack
        foreach (var type in allTypes.OrderBy(x => x)) // Sort for deterministic results
        {
            if (inDegree[type] == 0)
            {
                stack.Push(type);
            }
        }

        // Process types in depth-first topological order
        var processed = new HashSet<string>();
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            result.Add(current);
            processed.Add(current);

            // Find types that were just unblocked by processing current type
            var newlyAvailable = new List<string>();
            foreach (var otherType in allTypes)
            {
                if (dependencies[otherType].Contains(current))
                {
                    inDegree[otherType]--;
                    if (inDegree[otherType] == 0 && !processed.Contains(otherType))
                    {
                        newlyAvailable.Add(otherType);
                    }
                }
            }

            // Add newly available types to stack (in reverse order for consistent processing)
            foreach (var type in newlyAvailable.OrderByDescending(x => x))
            {
                stack.Push(type);
            }
        }

        // Add any remaining unprocessed types (those with unresolved dependencies/cycles)
        var remainingTypes = allTypes.Where(t => !processed.Contains(t)).OrderBy(x => x).ToList();
        result.AddRange(remainingTypes);

        return result;
    }
}