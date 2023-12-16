namespace AdventOfCode.Year2023;
[AocDay(16)]
internal class Day16 : Day
{
    public override void PartOne()
    {
        var map = Input
            .Split('\n', StringSplitOptions.TrimEntries)
            .Select(f => f.ToCharArray())
            .ToArray();

        var energyMap = EnergyMap(map, ((0, 0), "right"));
        var energizedCount = EnergyMapCount(energyMap);

        Console.WriteLine(energizedCount);
    }

    public override void PartTwo()
    {
        var map = Input
            .Split('\n', StringSplitOptions.TrimEntries)
            .Select(f => f.ToCharArray())
            .ToArray();

        var starts = new List<((int, int), string)>();
        for (int i = 0; i < map.Length; i++)
        {
            starts.Add(((i, 0), "right"));
            starts.Add(((i, map[0].Length - 1), "left"));
        }

        for (int j = 0; j < map[0].Length; j++)
        {
            starts.Add(((0, j), "down"));
            starts.Add(((map.Length - 1, j), "up"));
        }

        var max = 0;
        foreach (var start in starts)
        {
            var count = EnergyMapCount(EnergyMap(map, start));
            max = Math.Max(max, count);
        }

        Console.WriteLine(max);
    }

    int EnergyMapCount(int[][] map)
    {
        var energizedCount = 0;
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[0].Length; j++)
            {
                if (map[i][j] > 0)
                    energizedCount++;
            }
        }

        return energizedCount;
    }

    int[][] EnergyMap(char[][] map, ((int i, int j), string dir) startBeam)
    {
        var energyMap = new int[map.Length][];
        for (int i = 0; i < map.Length; i++)
            energyMap[i] = new int[map[i].Length];

        var queue = new Queue<((int i, int j) pos, string direction)>();
        queue.Enqueue(startBeam);

        while (queue.Count > 0)
        {
            var beam = queue.Dequeue();
            if (beam.pos.i < 0 || beam.pos.i >= map.Length || beam.pos.j < 0 || beam.pos.j >= map[0].Length)
                continue;

            var mapTile = energyMap[beam.pos.i][beam.pos.j];

            if ((GetDirValue(beam.direction) & mapTile) != 0)
                continue;

            energyMap[beam.pos.i][beam.pos.j] = (mapTile | GetDirValue(beam.direction));

            var tile = map[beam.pos.i][beam.pos.j];
            var newDirs = GetNewDirections(beam, tile);
            foreach (var dir in newDirs)
            {
                (int i, int j) newPos = (0, 0);

                if (dir == "right")
                    newPos = (0, 1);
                else if (dir == "left")
                    newPos = (0, -1);
                else if (dir == "up")
                    newPos = (-1, 0);
                else if (dir == "down")
                    newPos = (1, 0);

                var newBeam = ((beam.pos.i + newPos.i, beam.pos.j + newPos.j), dir);
                queue.Enqueue(newBeam);
            }
        }

        return energyMap;
    }

    List<string> GetNewDirections(((int i, int j) pos, string direction) beam, char tile)
    {
        if (tile is '.')
            return [beam.direction];

        if (beam.direction is "right" or "left")
        {
            if (tile is '-')
                return [beam.direction];
            else if (tile is '|')
                return ["up", "down"];
            else if (tile is '/')
                return [counterClockwise(beam.direction)];
            else if (tile is '\\')
                return [clockwise(beam.direction)];
        }
        else if (beam.direction is "up" or "down")
        {
            if (tile is '|')
                return [beam.direction];
            else if (tile is '-')
                return ["left", "right"];
            else if (tile is '/')
                return [clockwise(beam.direction)];
            else if (tile is '\\')
                return [counterClockwise(beam.direction)];
        }

        return [];

        string clockwise(string s)
        {
            return s switch
            {
                "right" => "down",
                "down" => "left",
                "left" => "up",
                "up" => "right",
                _ => ""
            };
        }

        string counterClockwise(string s)
        {
            return s switch
            {
                "right" => "up",
                "down" => "right",
                "left" => "down",
                "up" => "left",
                _ => ""
            };
        }
    }

    int GetDirValue(string dir)
    {
        return dir switch
        {
            "right" => 1,
            "down" => 2,
            "left" => 4,
            "up" => 8,
            _ => 0
        };
    }
}
