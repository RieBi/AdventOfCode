namespace AdventOfCode.Year2023;
internal class Day10 : Day
{
    public override void PartOne()
    {
        var map = Input.Split('\n', StringSplitOptions.TrimEntries);
        var startPosition = FindChar(map, 'S');
        List<(int x, int y)> possibleContinuations =
                [(startPosition.x, startPosition.y - 1),
                (startPosition.x, startPosition.y + 1),
                (startPosition.x - 1, startPosition.y),
                (startPosition.x + 1, startPosition.y)];

        var startAdjacent = possibleContinuations.Where(f => GetConnections(map, f).Contains(startPosition));
        var second = startAdjacent.First();
        var last = startAdjacent.Last();

        var count = 1;
        var prev = startPosition;
        var curr = second;
        while (prev != last)
        {
            (prev, curr) = (curr, FindNext(map, curr, prev));
            count++;
        }

        var farthestDistance = count / 2;

        Console.WriteLine(farthestDistance);
    }

    public override void PartTwo()
    {
        CalculatePartTwo("A");
        CalculatePartTwo("D");
    }

    void CalculatePartTwo(string order)
    {
        var map = Input.Split('\n', StringSplitOptions.TrimEntries);
        var startPosition = FindChar(map, 'S');
        List<(int x, int y)> possibleContinuations =
                [(startPosition.x, startPosition.y - 1),
                    (startPosition.x, startPosition.y + 1),
                    (startPosition.x - 1, startPosition.y),
                    (startPosition.x + 1, startPosition.y)];

        var startAdjacent = possibleContinuations.Where(f => GetConnections(map, f).Contains(startPosition));
        if (order == "A")
            startAdjacent = startAdjacent.Order();
        else if (order == "D")
            startAdjacent = startAdjacent.OrderDescending();

        var second = startAdjacent.First();
        var last = startAdjacent.Last();

        var encloseds = new char[map.Length][];
        for (int i = 0; i < encloseds.Length; i++)
        {
            encloseds[i] = new char[map[0].Length];
            for (int j = 0; j < encloseds[i].Length; j++)
                encloseds[i][j] = map[i][j];
        }

        var prev = startPosition;
        var curr = second;

        encloseds[startPosition.y][startPosition.x] = 'Z';
        encloseds[curr.y][curr.x] = 'Z';
        var count = 0;
        while (prev != last)
        {
            (prev, curr) = (curr, FindNext(map, curr, prev));
            encloseds[curr.y][curr.x] = 'Z';
            count++;

            var rights = FindRights(map, curr, prev);
            foreach (var v in rights)
            {
                if (encloseds[v.y][v.x] != 'Z')
                    encloseds[v.y][v.x] = 'X';
            }
        }

        foreach (var x in FindAllChars(encloseds, 'X'))
            Prolongate(encloseds, x);

        for (int i = 0; i < encloseds.Length; i++)
        {
            bool wrong = false;
            for (int j = 0; j < encloseds[0].Length; j++)
            {
                if (encloseds[i][j] == 'X'
                    &&
                    (i == 0 || j == 0 || i + 1 == encloseds.Length || j + 1 == encloseds[0].Length))
                {
                    Console.Write("Wrong: ");
                    wrong = true;
                    break;
                }
            }

            if (wrong)
                break;
        }
        Console.WriteLine(FindAllChars(encloseds, 'X').Count());
    }

    List<(int x, int y)> GetConnections(string[] map, (int x, int y) position)
    {
        (var x, var y) = (position.x, position.y);
        if (!Validate(map, [position]).Any())
            return [(-1, -1)];

        var ch = map[y][x];

        List<(int x, int y)> positions = ch switch
        {
            '|' => [(x, y - 1), (x, y + 1)],
            '-' => [(x - 1, y), (x + 1, y)],
            'L' => [(x, y - 1), (x + 1, y)],
            'J' => [(x, y - 1), (x - 1, y)],
            '7' => [(x, y + 1), (x - 1, y)],
            'F' => [(x, y + 1), (x + 1, y)],
            'S' => [position, position],
            _ => []
        };

        return Validate(map, positions);
    }

    (int x, int y) FindChar(string[] map, char ch)
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == ch)
                    return (j, i);
            }
        }

        return (-1, -1);
    }

    IEnumerable<(int x, int y)> FindAllChars(char[][] map, char ch)
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == ch)
                    yield return (j, i);
            }
        }
    }

    (int x, int y) FindNext(string[] map, (int x, int y) position, (int x, int y) prev)
    {
        var connections = GetConnections(map, position);
        return connections.Where(f => f != prev).First();
    }

    List<(int x, int y)> FindRights(string[] map, (int x, int y) position, (int x, int y) prev)
    {
        (var posX, var posY) = (position.x, position.y);
        List<(int x, int y)> right;
        var ch = map[posY][posX];
        if (ch == '|')
        {
            if (prev.y > posY)
                right = [(posX + 1, posY)];
            else
                right = [(posX - 1, posY)];
        }
        else if (ch == '-')
        {
            if (prev.x < posX)
                right = [(posX, posY + 1)];
            else
                right = [(posX, posY - 1)];
        }
        else if (ch == 'L')
        {
            if (prev.y < posY)
                right = [(posX - 1, posY), (posX, posY + 1)];
            else
                right = [];
        }
        else if (ch == 'J')
        {
            if (prev.x < posX)
                right = [(posX, posY + 1), (posX + 1, posY)];
            else
                right = [];
        }
        else if (ch == '7')
        {
            if (prev.y > posY)
                right = [(posX + 1, posY), (posX, posY - 1)];
            else
                right = [];
        }
        else if (ch == 'F')
        {
            if (prev.x > posX)
                right = [(posX, posY - 1), (posX - 1, posY)];
            else
                right = [];
        }
        else
        {
            right = [];
        }

        return Validate(map, right);
    }

    void Prolongate(char[][] map, (int x, int y) position)
    {
        List<(int x, int y)> newPoses = [(position.x, position.y - 1),
            (position.x, position.y + 1),
            (position.x - 1, position.y),
            (position.x + 1, position.y)];

        newPoses = Validate(map, newPoses);

        foreach (var p in newPoses)
        {
            if (map[p.y][p.x] != 'Z' && map[p.y][p.x] != 'X')
            {
                map[p.y][p.x] = 'X';
                Prolongate(map, (p.x, p.y));
            }
        }
    }

    List<(int x, int y)> Validate(char[][] map,  List<(int x, int y)> positions)
    {
        return positions.Where(p =>
                p.x >= 0 && p.x < map[0].Length &&
                p.y >= 0 && p.y < map.Length)
            .ToList();
    }

    List<(int x, int y)> Validate(string[] map, List<(int x, int y)> positions)
    {
        return positions.Where(p =>
                p.x >= 0 && p.x < map[0].Length &&
                p.y >= 0 && p.y < map.Length)
            .ToList();
    }
}
