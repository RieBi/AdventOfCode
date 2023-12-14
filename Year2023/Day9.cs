using System.Diagnostics;

namespace AdventOfCode.Year2023;
internal partial class Day9 : Day
{
    public override void PartOne()
    {
        Console.WriteLine(CalculateTotal(CalculateExtrapolatedLast));
    }

    public override void PartTwo()
    {
        Console.WriteLine(CalculateTotal(CalculateExtrapolatedFirst));
    }

    [GeneratedRegex(@"(?:(-?\d+)[^\d\n-]*)+")]
    public partial Regex IntegerLines();

    long CalculateExtrapolatedLast(long[] values)
    {
        var pointer = values.Length - 2;

        var allZeroes = false;
        
        while (!allZeroes)
        {
            allZeroes = true;
            for (int i = 0; i <= pointer; i++)
            {
                values[i] = values[i + 1] - values[i];
                if (values[i] != 0)
                    allZeroes = false;
            }

            pointer--;
        }

        return values.Sum();
    }

    long CalculateExtrapolatedFirst(long[] values)
    {
        var pointer = 1;

        var allZeroes = false;

        while (!allZeroes)
        {
            var t = 0L;
            var nt = values[pointer - 1];

            allZeroes = true;
            for (int i = pointer; i < values.Length; i++)
            {
                (t, nt) = (nt, values[i]);
                values[i] = nt - t;
                if (values[i] != 0)
                    allZeroes = false;
            }

            pointer++;
        }

        return values.Reverse().Aggregate((a, b) => b - a);
    }

    long CalculateTotal(Func<long[], long> extrapolation)
    {
        var histories = IntegerLines().Matches(Input);
        long total = 0;
        foreach (Match historySequence in histories)
        {
            var values = historySequence
                .Groups[1]
                .Captures.Select(f => long.Parse(f.Value))
                .ToArray();

            total += extrapolation(values);
        }

        return total;
    }
}
