using System.Text;
using MathNet.Numerics;

namespace AdventOfCode.Year2023;
[AocDay(21)]
internal class Day21 : Day
{
    private List<(int i, int j)> transitions = [ (0, 1), (0, -1), (-1, 0), (1, 0) ];

    public override void PartOne()
    {
        var stepsTaken = 0;
        var positions = new HashSet<(int i, int j)>
        {
            GetStartPosition()
        };

        var stepsNeeded = 64;

        for (; stepsTaken < stepsNeeded; stepsTaken++)
        {
            var newPositions = new HashSet<(int i, int j)>();
            foreach (var (i, j) in positions)
            {
                foreach (var adjacent in GetAdjacentPlots(i, j))
                {
                    newPositions.Add(adjacent);
                }
            }

            positions = newPositions;
        }

        Console.WriteLine(positions.Count);
    }

    public override void PartTwo()
    {
        List<double> points = [];
        List<double> values = [];

        for (int k = 1; k <= 3; k++)
        {
            points.Add(k);
            values.Add(BruteForce(k));
        }

        var func = Fit.PolynomialFunc(points.ToArray(), values.ToArray(), 2);
        var result = (long)func(202300);

        Console.WriteLine($"Supposedly the result is: {result}");
    }

    private int BruteForce(int distance)
    {
        var newInput = new string[(distance * 2 + 1) * InputLines.Length];
        for (int i = 0; i < newInput.Length; i++)
        {
            var str = new StringBuilder();
            for (int j = 0; j < newInput.Length; j++)
            {
                str.Append(InputLines[i % InputLines.Length][j % InputLines.Length]);
            }

            newInput[i] = str.ToString();
        }

        var stepsTaken = 0;
        var positions = new HashSet<(int i, int j)>
        {
            (distance * InputLines.Length + (InputLines.Length/2), distance * InputLines.Length + (InputLines.Length/2))
        };

        var stepsNeeded = distance * InputLines.Length + (InputLines.Length/2);

        for (; stepsTaken < stepsNeeded; stepsTaken++)
        {
            var newPositions = new HashSet<(int i, int j)>();
            foreach (var (i, j) in positions)
            {
                foreach (var adjacent in GetAdjacentPlots(i, j, newInput))
                {
                    newPositions.Add(adjacent);
                }
            }

            positions = newPositions;
        }

        return positions.Count;
    }

    private IEnumerable<(int i, int j)> GetAdjacentPlots(int i, int j)
    {
        return transitions
            .Select(f => (f.i + i, f.j + j))
            .Where(f => f.Item1 >= 0 && f.Item2 >= 0 && f.Item1 < InputLines.Length && f.Item2 < InputLines[0].Length
            && InputLines[f.Item1][f.Item2] != '#');
    }

    private IEnumerable<(int i, int j)> GetAdjacentPlots(int i, int j, string[] input)
    {
        return transitions
            .Select(f => (f.i + i, f.j + j))
            .Where(f => f.Item1 >= 0 && f.Item2 >= 0 && f.Item1 < input.Length && f.Item2 < input[0].Length
            && input[f.Item1][f.Item2] != '#');
    }

    private (int i, int j) GetStartPosition()
    {
        for (int i = 0; i < InputLines.Length; i++)
        {
            for (int j = 0; j < InputLines[i].Length; j++)
            {
                if (InputLines[i][j] == 'S')
                    return (i, j);

            }
        }

        throw new Exception("Input is incorrect");
    }
}
