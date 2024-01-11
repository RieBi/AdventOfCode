using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;
internal static partial class Utils
{
    [GeneratedRegex(@"[0-9]+")]
    private static partial Regex ints();

    public static List<int> Ints(string str)
    {
        return ints()
            .Matches(str)
            .Select(f => int.Parse(f.Value))
            .ToList();
    }

    public static string[] Lines(string str) => str.Split(
        '\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    public static long GCD(long a, long b)
    {
        if (a < b)
            (a, b) = (b, a);

        while (true)
        {
            var r = a % b;
            if (r == 0)
                return b;

            (a, b) = (b, r);
        }
    }

    public static int GCD(int a, int b)
    {
        if (a < b)
            (a, b) = (b, a);

        while (true)
        {
            var r = a % b;
            if (r == 0)
                return b;

            (a, b) = (b, r);
        }
    }

    public static long LCM(long a, long b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }

    public static int LCM(int a, int b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }
}
    