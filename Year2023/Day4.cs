using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023;
internal class Day4 : Day
{
    public Day4()
    {
        this.DayNumber = 4;
    }

    public override void PartOne()
    {
        var pattern = @"Card +\d+: +(?:(?<win>\d+) *)+\|(?: *(?<have>\d+))+";
        var total = 0;

        foreach (var line in Input.Split('\n'))
        {
            var match = Regex.Match(line, pattern);
            var occurences = match.Groups["win"].Captures.Select(f => f.Value)
                .Intersect(match.Groups["have"].Captures.Select(f => f.Value))
                .Count();

            total += (int)Math.Pow(2, occurences - 1);
        }

        Console.WriteLine(total);
    }

    public override void PartTwo()
    {
        var pattern = @"Card +\d+: +(?:(?<win>\d+) *)+\|(?: *(?<have>\d+))+";
        var lines = Input.Split('\n');
        var cards = new int[lines.Length];
        for (int i = 0; i < cards.Length; i++)
            cards[i] = 1;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            var cardCount = cards[i];
            var match = Regex.Match(line, pattern);
            var occurences = match.Groups["win"].Captures.Select(f => f.Value)
                .Intersect(match.Groups["have"].Captures.Select(f => f.Value))
                .Count();

            var maxInd = Math.Min(cards.Length, i + 1 + occurences);
            for (int j = i + 1; j < maxInd ; j++)
            {
                cards[j] += cardCount;
            }
        }

        Console.WriteLine(cards.Sum());

    }
}
