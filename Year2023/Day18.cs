namespace AdventOfCode.Year2023;
[AocDay(18)]
internal class Day18 : Day
{
    public override void PartOne()
    {
        var entries = Input.Split('\n').Select(f => f.Split(' ')).ToList();
        var coordinates = new List<(long x, long y)>(entries.Count);

        (long x, long y) position = (0, 0);
        var perim = 0L;
        for (int i = 0; i < entries.Count; i++)
        {
            var length = long.Parse(entries[i][1]);
            perim += length;
            position = entries[i][0] switch
            {
                "U" => (position.x, position.y + length),
                "D" => (position.x, position.y - length),
                "L" => (position.x - length, position.y),
                "R" => (position.x + length, position.y),
                _ => (position.x, position.y)
            };

            coordinates.Add(position);
        }

        var sum = 0L;
        for (int i = 0; i < coordinates.Count; i++)
        {
            var nextI = i == coordinates.Count - 1 ? 0 : i + 1;
            sum += coordinates[i].x * coordinates[nextI].y;
            sum -= coordinates[i].y * coordinates[nextI].x;
        }

        Console.WriteLine(Math.Abs((sum) / 2) + (perim + 2) / 2);
    }

    public override void PartTwo()
    {
        var entries = Input
            .Split('\n', StringSplitOptions.TrimEntries)
            .Select(f => f.Split(' ', StringSplitOptions.TrimEntries)).ToList();
        var coordinates = new List<(long x, long y)>(entries.Count);

        (long x, long y) position = (0, 0);
        var perim = 0L;
        for (int i = 0; i < entries.Count; i++)
        {
            var length = Convert.ToInt64(entries[i][2][2..7], 16);
            perim += length;
            var dir = entries[i][2][^2];
            position = dir switch
            {
                '0' => (position.x + length, position.y),
                '1' => (position.x, position.y - length),
                '2' => (position.x - length, position.y),
                _ => (position.x, position.y + length)
            };

            coordinates.Add(position);
        }

        var sum = 0L;
        for (int i = 0; i < coordinates.Count; i++)
        {
            var nextI = i == coordinates.Count - 1 ? 0 : i + 1;
            sum += coordinates[i].x * coordinates[nextI].y;
            sum -= coordinates[i].y * coordinates[nextI].x;
        }

        Console.WriteLine(Math.Abs((sum) / 2) + (perim + 2) / 2);
    }
}
