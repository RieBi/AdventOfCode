using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023;
internal class Day3 : Day
{
    private readonly HashSet<char> notMatchable = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.'];
    private Dictionary<(int x, int y), List<int>> gears = [];
    private int result = 0;
    private int result2 = 0;

    public Day3()
    {
        this.DayNumber = 3;
    }

    public override void PartOne()
    {
        var matrix = Input.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        for (int row = 0; row < matrix.Length; row++)
        {
            var matches = Regex.Matches(matrix[row], @"\d+");
            foreach (Match match in matches)
            {
                processGivenMatch(match, matrix, row);
            }
        }

        Console.WriteLine(result);
    }

    public override void PartTwo()
    {
        foreach (var kv in gears)
        {
            if (kv.Value.Count != 2)
                continue;

            result2 += kv.Value.Aggregate((a, b) => a * b);
        }

        Console.WriteLine(result2);
    }

    private void processGivenMatch(Match match, string[] matrix, int row)
    {
        // Iterate over positions
        for (int x = row - 1; x <= row + 1; x++)
        {
            if (x < 0 || x >= matrix.Length)
                continue;
            for (int y = match.Index - 1; y <= (match.Index + match.Length); y++)
            {
                if (y < 0 || y >= matrix[row].Length)
                    continue;

                if (notMatchable.Contains(matrix[x][y]))
                    continue;

                var num = int.Parse(match.Value);
                result += num;

                if (matrix[x][y] == '*')
                {
                    if (!gears.TryGetValue((x, y), out var list))
                        gears[(x, y)] = new List<int>() { num };
                    else
                        list.Add(num);
                }

                return;
            }
        }
    }
}
