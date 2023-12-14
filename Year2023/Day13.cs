using Microsoft.Win32.SafeHandles;

namespace AdventOfCode.Year2023;
[AocDay(13)]
internal class Day13 : Day
{
    public override void PartOne()
    {
        var patterns = Input.Split("\n\n", StringSplitOptions.TrimEntries);

        var sum = 0;
        for (int i = 0; i < patterns.Length; i++)
        {
            var arrayed = patterns[i]
                .Split('\n', StringSplitOptions.TrimEntries)
                .Select(f => f.ToCharArray())
                .ToArray();

            sum += GetPatternNumber(arrayed);
        }

        Console.WriteLine(sum);
    }

    public override void PartTwo()
    {
        var patterns = Input.Split("\n\n", StringSplitOptions.TrimEntries);

        var sum = 0;
        for (int i = 0; i < patterns.Length; i++)
        {
            var pattern = patterns[i]
                .Split('\n', StringSplitOptions.TrimEntries)
                .Select(f => f.ToCharArray())
                .ToArray();

            var num1 = GetPatternNumber(pattern);
            for (int x = 0; x < pattern[0].Length; x++)
            {
                for (int y = 0; y < pattern.Length; y++)
                {
                    pattern[y][x] = Swapped(pattern[y][x]);
                    var num2 = GetPatternNumber(pattern, num1);
                    pattern[y][x] = Swapped(pattern[y][x]);

                    if (num1 != 0 && num2 != 0)
                        sum += num2;
                }
            }
        }

        Console.WriteLine(sum / 2);

        static char Swapped(char ch) => ch == '.' ? '#' : '.';
    }

    bool IsVerticalReflectionPresent(char[][] pattern, int rightInd)
    {
        for (int left = rightInd - 1, right = rightInd; left >= 0 && right < pattern[0].Length; left--, right++)
        {
            for (int row = 0; row < pattern.Length; row++)
            {
                if (pattern[row][left] != pattern[row][right])
                    return false;
            }
        }

        return true;
    }

    bool IsHorizontalReflectionPresent(char[][] pattern, int lowInd)
    {
        for (int top = lowInd - 1, low = lowInd; top >= 0 && low < pattern.Length; top--, low++)
        {
            for (int col = 0; col < pattern[0].Length; col++)
            {
                if (pattern[top][col] != pattern[low][col])
                    return false;
            }
        }

        return true;
    }

    int GetPatternNumber(char[][] pattern, int ignore = -1)
    {
        var sum = 0;
        for (int i = 1; i < pattern[0].Length; i++)
        {
            if (IsVerticalReflectionPresent(pattern, i) && i != ignore)
            {
                sum += i;
            }
        }

        for (int i = 1; i < pattern.Length; i++)
        {
            if (IsHorizontalReflectionPresent(pattern, i) && i * 100 != ignore)
            {
                sum += 100 * i;
            }
        }

        return sum;
    }
}
