using System.Security.Cryptography;

namespace AdventOfCode.Year2023;
[AocDay(14)]
internal class Day14 : Day
{
    public override void PartOne()
    {
        var map = Input
            .Split('\n', StringSplitOptions.TrimEntries)
            .Select(f => f.ToCharArray())
            .ToArray();

        TiltNorth(map);
        var value = TotalLoad(map);
        Console.WriteLine(value);
    }

    public override void PartTwo()
    {
        var map = Input
            .Split('\n', StringSplitOptions.TrimEntries)
            .Select(f => f.ToCharArray())
            .ToArray();

        var hashes = new HashSet<int>();
        var hashesList = new List<(int, long)>();

        var n = 10000;
        SpinCycle(map);
        for (int i = 1; i <= n; i++)
        {
            var hash = GetUniqueHashCode(map);
            if (hashes.Contains(hash))
            {
                var ind = hashesList.FindIndex(f => f.Item1 == hash) + 1;
                var loop = i - ind;
                var bignum = 1000000000;
                var newind = ind + ((bignum - ind - 1) % loop);
                var result = hashesList[newind].Item2;
                Console.WriteLine(result);
                break;
            }

            var load = TotalLoad(map);
            hashes.Add(hash);
            hashesList.Add((hash, load));
            SpinCycle(map);
        }
    }

    int GetUniqueHashCode(char[][] map)
    {
        var hash = 0;
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[0].Length; j++)
            {
                hash = HashCode.Combine(hash, map[i][j]);
            }
        }

        return hash;
    }

    void TiltNorth(char[][] map)
    {
        for (int i = 0; i < map[0].Length; i++)
        {
            var pointer = 0;
            for (int j = 0; j < map.Length; j++)
            {
                if (map[j][i] == 'O')
                {
                    map[j][i] = '.';
                    map[pointer++][i] = 'O';
                }
                else if (map[j][i] == '#')
                    pointer = j + 1;
            }
        }
    }

    void TiltSouth(char[][] map)
    {
        for (int i = 0; i < map[0].Length; i++)
        {
            var pointer = map.Length - 1;
            for (int j = map.Length - 1; j >= 0; j--)
            {
                if (map[j][i] == 'O')
                {
                    map[j][i] = '.';
                    map[pointer--][i] = 'O';
                }
                else if (map[j][i] == '#')
                    pointer = j - 1;
            }
        }
    }

    void TiltWest(char[][] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            var pointer = 0;
            for (int j = 0; j < map[0].Length; j++)
            {
                if (map[i][j] == 'O')
                {
                    map[i][j] = '.';
                    map[i][pointer++] = 'O';
                }
                else if (map[i][j] == '#')
                    pointer = j + 1;
            }
        }
    }

    void TiltEast(char[][] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            var pointer = map[0].Length - 1;
            for (int j = map[0].Length - 1; j >= 0; j--)
            {
                if (map[i][j] == 'O')
                {
                    map[i][j] = '.';
                    map[i][pointer--] = 'O';
                }
                else if (map[i][j] == '#')
                    pointer = j - 1;
            }
        }
    }

    void SpinCycle(char[][] map)
    {
        TiltNorth(map);
        TiltWest(map);
        TiltSouth(map);
        TiltEast(map);
    }

    long TotalLoad(char[][] map)
    {
        var sum = 0L;
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == 'O')
                    sum += map.Length - i;
            }
        }

        return sum;
    }
}
