namespace AdventOfCode.Year2023;
[AocDay(17)]
internal class Day17 : Day
{
    public override void PartOne()
    {
        Console.WriteLine(GetResult(0, 3));
    }

    public override void PartTwo()
    {
        Console.WriteLine(GetResult(4, 10));
    }

    int GetResult(int minimumConsecutive, int maximumConsecutive)
    {
        var grid = Input
            .Split('\n', StringSplitOptions.TrimEntries)
            .Select(f => f.Select(ff => int.Parse(ff.ToString())).ToArray())
            .ToArray();

        // The Dijkstra's heap.
        var sorted = new SortedSet<Node>(new NodeComparer());
        var graph = new Dictionary<(int i, int j, string direction, int consecutive), Node>();
        // Creating nodes.
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[0].Length; j++)
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    for (int k = 1; k <= maximumConsecutive; k++)
                    {
                        string direction = dir switch
                        {
                            0 => "right",
                            1 => "down",
                            2 => "left",
                            3 => "up",
                            _ => "notnull"
                        };
                        var node = new Node((i, j, direction, k), grid[i][j]);
                        if (i == 0 && j == 0)
                        {
                            node.Distance = 0;
                            node.Name = (i, j, direction, 1);
                        }

                        graph[node.Name] = node;
                    }
                }
            }
        }

        // Creating node connections.
        foreach (var node in graph)
        {
            var name = node.Key;
            var (i, j) = (name.i, name.j);

            IEnumerable<(int i, int j)> connections = [(i - 1, j), (i + 1, j), (i, j - 1), (i, j + 1)];
            connections = connections.Where(f => f.i >= 0 && f.j >= 0 && f.i < grid.Length && f.j < grid[0].Length);

            foreach (var connection in connections)
            {
                var di = connection.i - i;
                var dj = connection.j - j;
                string direction = (di, dj) switch
                {
                    (1, _) => "down",
                    (-1, _) => "up",
                    (_, 1) => "right",
                    (_, -1) => "left",
                    _ => "notnull"
                };

                if (ReverseDirection(name.direction) == direction)
                    continue;
                if (name.direction == direction && name.consecutive == maximumConsecutive)
                    continue;
                if (name.direction != direction && name.consecutive < minimumConsecutive)
                    continue;

                var consecutiveCount = name.direction == direction ? name.consecutive + 1 : 1;
                var otherNodeName = (connection.i, connection.j, direction, consecutiveCount);
                if (connection.i == 0 && connection.j == 0)
                    continue;
                node.Value.Relations.Add(otherNodeName);
            }
        }

        foreach (var node in graph)
            sorted.Add(node.Value);

        // Do Dijkstra's.
        while (sorted.Count > 0)
        {
            var node = sorted.Min!;
            sorted.Remove(node);
            if (node.Distance == int.MaxValue)
                continue;

            foreach (var other in node.Relations)
            {
                var otherNode = graph[other];
                if (!sorted.Contains(otherNode))
                    continue;
                if (node.Distance + otherNode.Value < otherNode.Distance)
                {
                    sorted.Remove(otherNode);
                    otherNode.Distance = node.Distance + otherNode.Value;
                    sorted.Add(otherNode);
                }
            }
        }

        var lastNodes = graph.Values
            .Where(
            f => f.Name.i == grid.Length - 1
            && f.Name.j == grid[0].Length - 1
            && f.Distance != int.MaxValue
            && f.Name.consecutive >= minimumConsecutive
            );
        var minimumLastNode = lastNodes.MinBy(f => f.Distance)!;
        return minimumLastNode.Distance;
    }


    private string? ReverseDirection(string? str)
    {
        return str switch
        {
            "right" => "left",
            "left" => "right",
            "up" => "down",
            "down" => "up",
            _ => str + "notnull"
        };
    }

    private class Node((int i, int j, string direction, int consecutive) name, int value)
    {
        public (int i, int j, string direction, int consecutive) Name { get; set; } = name;
        public int Distance { get; set; } = int.MaxValue;
        public int Value { get; set; } = value;
        public HashSet<(int i, int j, string direction, int consecutive)> Relations { get; set; } = new HashSet<(int i, int j, string direction, int consecutive)>();
    }

    private class NodeComparer : IComparer<Node>
    {
        public int Compare(Node? x, Node? y)
        {
            var value = x.Distance.CompareTo(y.Distance);
            if (value != 0)
                return value;
            else
                return x.Name.CompareTo(y.Name);
        }
    }

}
