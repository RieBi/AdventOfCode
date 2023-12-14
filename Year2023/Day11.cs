using System.Runtime;

namespace AdventOfCode.Year2023;
internal class Day11 : Day
{
    public override void PartOne()
    {
        var space = Input.Split('\n', StringSplitOptions.TrimEntries);
        var newRows = new int[space.Length];
        var newCols = new int[space[0].Length];

        var cur = -1;
        for (int i = 0; i < space.Length; i++)
        {
            if (space[i].All(f => f == '.'))
                cur++;
            cur++;
            newRows[i] = cur;
        }

        cur = -1;
        for (int i = 0; i < space[0].Length; i++)
        {
            if (space.All(f => f[i] == '.'))
                cur++;
            cur++;
            newCols[i] = cur;
        }

        var galaxies = new List<(long x, long y)>();
        for (int i = 0; i < space.Length; i++)
        {
            for (int j = 0; j < space[0].Length; j++)
            {
                if (space[i][j] == '#')
                    galaxies.Add((newCols[j], newRows[i]));
            }
        }

        var totalSum = 0L;
        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                var distance = Math.Abs(galaxies[i].x - galaxies[j].x) + Math.Abs(galaxies[i].y - galaxies[j].y);
                totalSum += distance;
            }
        }

        Console.WriteLine(totalSum);
    }

    public override void PartTwo()
    {
        var space = Input.Split('\n', StringSplitOptions.TrimEntries);
        var newRows = new int[space.Length];
        var newCols = new int[space[0].Length];

        var cur = -1;
        for (int i = 0; i < space.Length; i++)
        {
            if (space[i].All(f => f == '.'))
                cur += 1_000_000 - 1;
            cur++;
            newRows[i] = cur;
        }

        cur = -1;
        for (int i = 0; i < space[0].Length; i++)
        {
            if (space.All(f => f[i] == '.'))
                cur += 1_000_000 - 1;
            cur++;
            newCols[i] = cur;
        }

        var galaxies = new List<(long x, long y)>();
        for (int i = 0; i < space.Length; i++)
        {
            for (int j = 0; j < space[0].Length; j++)
            {
                if (space[i][j] == '#')
                    galaxies.Add((newCols[j], newRows[i]));
            }
        }

        var totalSum = 0L;
        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                var distance = Math.Abs(galaxies[i].x - galaxies[j].x) + Math.Abs(galaxies[i].y - galaxies[j].y);
                totalSum += distance;
            }
        }

        Console.WriteLine(totalSum);
    }
}
