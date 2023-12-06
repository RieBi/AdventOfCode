namespace AdventOfCode.Year2023;
internal class Day6 : Day
{
    string pattern = @"(?:[^\d]*(\d+))+";

    public Day6()
    {
        this.DayNumber = 6;
    }

    public override void PartOne()
    {
        var lines = Input.Split('\n');
        var times = lines[0];
        var distances = lines[1];

        var races = Regex.Match(times, pattern).Groups[1].Captures.Select(f => long.Parse(f.Value))
            .Zip(Regex.Match(distances, pattern).Groups[1].Captures.Select(f => long.Parse(f.Value)));

        var result = races.Aggregate(1, (a, b) => a * NumberOfWaysToWinRace((b.First, b.Second)));
        Console.WriteLine(result);
    }

    public override void PartTwo()
    {
        var lines = Input.Split('\n');
        var times = lines[0].Replace(" ", "");
        var distances = lines[1].Replace(" ", "");

        var race = ((long.Parse(Regex.Match(times, pattern).Groups[1].Value)), (long.Parse(Regex.Match(distances, pattern).Groups[1].Value)));

        var result = NumberOfWaysToWinRace((race.Item1, race.Item2));
        Console.WriteLine(result);
    }

    int NumberOfWaysToWinRace((long time, long distance) race)
    {
        (long time, long distance) = (race.time, race.distance);
        var numOfWays = 0;
        for (long i = 1; i < time; i++)
        {
            var curTime = time - i;
            var speed = i;
            if (curTime * speed > distance)
                numOfWays++;
        }

        return numOfWays;
    }
}
