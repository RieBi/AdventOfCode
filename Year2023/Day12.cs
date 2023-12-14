using System.Text;
using System.Collections.Immutable;

namespace AdventOfCode.Year2023;
[AocDay(12)]
internal class Day12 : Day
{
    Dictionary<(string, int, ImmutableList<int>, int), long> Cache = new Dictionary<(string, int, ImmutableList<int>, int), long>();

    public override void PartOne()
    {
        var lines = Input.Split('\n', StringSplitOptions.TrimEntries);

        var sum = 0L;
        foreach (var line in lines)
        {
            var lineSum = GetArrangements(line);
            sum += lineSum;
        }

        Console.WriteLine(sum);
    }

    public override void PartTwo()
    {
        var lines = Input.Split('\n', StringSplitOptions.TrimEntries);

        var sum = 0L;
        foreach (var line in lines)
        {
            var lineSum = GetArrangements(line, true);
            sum += lineSum;
        }

        Console.WriteLine(sum);
    }

    long GetArrangements(string line, bool partTwo = false)
    {
        var parts = line.Split(' ');
        if (partTwo)
        {
            parts[0] = RepeatString(parts[0], "?", 5);
            parts[1] = RepeatString(parts[1], ",", 5);
        }

        var pattern = parts[0] + ".";
        var groups = parts[1].Split(',').Select(int.Parse).ToImmutableList();

        var s = CalculateArrangements(pattern, 0, groups, 0);
        return s;
    }

    string RepeatString(string str, string separator, long count)
    {
        var builder = new StringBuilder();
        for (long i = 0; i < count - 1; i++)
        {
            builder.Append(str);
            builder.Append(separator);
        }

        builder.Append(str);
        return builder.ToString();
    }

    long CalculateArrangements(string pattern, int position, ImmutableList<int> groups, int groupNum)
    {
        if (Cache.TryGetValue((pattern, position, groups, groupNum), out long cachedVal))
            return cachedVal;

        if (groupNum == groups.Count && !pattern[position..].Contains('#'))
            return 1;
        else if (position == pattern.Length || groups.Count == groupNum)
            return 0;

        var sum = 0L;

        if (pattern[position] is '#' or '?' && IsPossibleGroup(pattern, position, groups[groupNum]))
            sum += CalculateArrangements(pattern, position + groups[groupNum] + 1, groups, groupNum + 1);
        
        if (pattern[position] is '.' or '?')
            sum += CalculateArrangements(pattern, position + 1, groups, groupNum);

        Cache[(pattern, position, groups, groupNum)] = sum;
        return sum;
    }

    bool IsPossibleGroup(string pattern, int position, int group)
    {
        if (pattern.Length < group + position)
            return false;

        for (int i = position; i < group + position; i++)
        {
            if (!(pattern[i] is '?' or '#'))
                return false;
        }

        if (pattern.Length == group + position || pattern[group + position] is '?' or '.')
            return true;
        else
            return false;
    }
}
