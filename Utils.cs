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
}
    