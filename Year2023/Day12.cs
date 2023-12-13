using System.Text;

namespace AdventOfCode.Year2023;
internal class Day12 : Day
{
    int Calls = 0;
    Dictionary<(string, int, List<int>, List<int>), int> Cache = new Dictionary<(string, int, List<int>, List<int>), int>();

    public Day12()
    {
        this.DayNumber = 12;
    }

    public override void PartOne()
    {
        var lines = Input.Split('\n', StringSplitOptions.TrimEntries);

        var sum = 0;
        foreach (var line in lines)
        {
            var lineSum = GetArrangements(line);
            sum += lineSum;
        }

        Console.WriteLine(sum);
    }

    public override void PartTwo()
    {
        base.PartTwo();
    }

    int GetArrangements(string line)
    {
        var parts = line.Split(' ');
        var pattern = parts[0];
        var nums = parts[1].Split(',').Select(int.Parse).ToList();

        var unknowns = new List<int>();
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == '?')
                unknowns.Add(i);
        }

        var arrangementCount = CalculateArrangements(new StringBuilder(pattern), unknowns, 0, nums);
        return arrangementCount;
    }

    int CalculateArrangements(StringBuilder pattern, List<int> unknowns, int position, List<int> numbering)
    {
        var s = pattern.ToString();
        if (Cache.ContainsKey((s, position, unknowns, numbering)))
            return Cache[(s, position, unknowns, numbering)];

        if (position >= unknowns.Count)
        {
            if (GetPatternNumbering(pattern).SequenceEqual(numbering))
                return 1;
            else
                return 0;
        }

        var sum = 0;

        pattern[unknowns[position]] = '#';
        sum += CalculateArrangements(pattern, unknowns, position + 1, numbering);
        pattern[unknowns[position]] = '.';
        sum += CalculateArrangements(pattern, unknowns, position + 1, numbering);
        pattern[unknowns[position]] = '?';

        Cache[(s, position, unknowns, numbering)] = sum;
        return sum;
    }

    List<int> GetPatternNumbering(StringBuilder pattern)
    {
        var list = new List<int>();
        var streak = 0;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == '#')
                streak++;
            else if (streak > 0)
            {
                list.Add(streak);
                streak = 0;
            }
        }

        if (streak > 0)
            list.Add(streak);

        return list;
    }
}
