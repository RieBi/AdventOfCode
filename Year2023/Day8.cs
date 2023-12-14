namespace AdventOfCode.Year2023;
internal class Day8 : Day
{
    public override void PartOne()
    {
        var lines = Input.Split('\n');
        var directions = lines[0].Trim();
        var pattern = @"(?<base>...) = \((?<left>...), (?<right>...)\)";

        var nodes = new Dictionary<string, (string left, string right)>();

        foreach (var line in lines.Skip(2))
        {
            var match = Regex.Match(line, pattern);
            var baseNode = match.Groups["base"].Value;
            var leftNode = match.Groups["left"].Value;
            var rightNode = match.Groups["right"].Value;

            nodes[baseNode] = (leftNode, rightNode);
        }

        var current = "AAA";
        var count = 0;
        while (current != "ZZZ")
        {
            var direction = directions[count++ % directions.Length];
            if (direction == 'L')
                current = nodes[current].left;
            else
                current = nodes[current].right;
        }

        Console.WriteLine(count);
    }

    public override void PartTwo()
    {
        var lines = Input.Split('\n');
        var directions = lines[0].Trim();
        var pattern = @"(?<base>...) = \((?<left>...), (?<right>...)\)";

        var nodes = new Dictionary<string, (string left, string right)>();
        var currentNodes = new List<string>();

        foreach (var line in lines.Skip(2))
        {
            var match = Regex.Match(line, pattern);
            var baseNode = match.Groups["base"].Value;
            var leftNode = match.Groups["left"].Value;
            var rightNode = match.Groups["right"].Value;

            nodes[baseNode] = (leftNode, rightNode);
            if (baseNode[2] == 'A')
                currentNodes.Add(baseNode);
        }

        var counts = new List<long>();
        foreach (var cur in currentNodes)
        {
            var current = cur;
            var count = 0;
            bool end = false;
            while (!end)
            {
                var direction = directions[count++ % directions.Length];
                if (direction == 'L')
                    current = nodes[current].left;
                else
                    current = nodes[current].right;

                if (current[2] == 'Z')
                    end = true;
            }

            counts.Add(count);
        }

        var totalLCM = counts[0];
        for (int i = 1; i < counts.Count; i++)
            totalLCM = LCM(totalLCM, counts[i]);

        Console.WriteLine(totalLCM);
    }

    static long GCD(long a, long b)
    {
        if (a < b)
            (a, b) = (b, a);

        while (true)
        {
            var r = a % b;
            if (r == 0)
                return b;

            (a, b) = (b, r);
        }
    }

    static long LCM(long a, long b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }
}
