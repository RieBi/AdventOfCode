namespace AdventOfCode.Year2023;
[AocDay(19)]
internal class Day19 : Day
{
    public override void PartOne()
    {
        var workflows = new Dictionary<string, List<Condition>>();
        var parts = new List<int[]>();

        foreach (var line in Input.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            // Part.
            if (line[0] == '{')
            {
                parts.Add(Utils.Ints(line).ToArray());
            }
            // Workflow.
            else
            {
                var start = line.IndexOf('{');
                var name = line[..start];
                var list = new List<Condition>();
                var conds = line[(start + 1)..^1].Split(',');
                for (int i = 0; i < conds.Length - 1; i++)
                {
                    var cond = conds[i];
                    var ind = cond[0] switch
                    {
                        'x' => 0,
                        'm' => 1,
                        'a' => 2,
                        _ => 3
                    };

                    var mult = cond[1] == '>' ? 1 : -1;

                    var colonInd = cond.IndexOf(':');
                    var value = int.Parse(cond[2..colonInd]);
                    var redirect = cond[(colonInd + 1)..];

                    list.Add(new Condition(ind, mult, value, redirect));
                }

                list.Add(new Condition(0, 1, int.MinValue, conds[^1]));
                workflows[name] = list;
            }
        }

        var total = 0L;
        foreach (var approved in parts.Where(f => isApproved(f, "in", workflows)))
            total += approved.Sum();

        Console.WriteLine(total);

        bool isApproved(int[] part, string workflow, Dictionary<string, List<Condition>> workflows)
        {
            if (workflow == "A")
                return true;
            if (workflow == "R")
                return false;

            foreach (var condition in workflows[workflow])
            {
                var (partVal, val) = (part[condition.PartInd] * condition.Mult, condition.Value * condition.Mult);
                if (partVal > val)
                    return isApproved(part, condition.Redirect, workflows);
            }

            throw new ArgumentException("Something's wrong with a workflow");
        }
    }

    public override void PartTwo()
    {
        var workflows = new Dictionary<string, List<Condition>>();

        foreach (var line in Input.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (line[0] == '{')
                continue;

            var start = line.IndexOf('{');
            var name = line[..start];
            var list = new List<Condition>();
            var conds = line[(start + 1)..^1].Split(',');
            for (int i = 0; i < conds.Length - 1; i++)
            {
                var cond = conds[i];
                var ind = cond[0] switch
                {
                    'x' => 0,
                    'm' => 1,
                    'a' => 2,
                    _ => 3
                };

                var mult = cond[1] == '>' ? 1 : -1;

                var colonInd = cond.IndexOf(':');
                var value = int.Parse(cond[2..colonInd]);
                var redirect = cond[(colonInd + 1)..];

                list.Add(new Condition(ind, mult, value, redirect));
            }

            list.Add(new Condition(0, 1, int.MinValue, conds[^1]));
            workflows[name] = list;
        }

        var result = getCombinations(
            [
                new Interval(1, 4000),
                new Interval(1, 4000),
                new Interval(1, 4000),
                new Interval(1, 4000)
            ], "in");

        Console.WriteLine(result);

        long getCombinations(Interval[] interval, string workflow)
        {
            if (interval.Any(f => f.Start > f.End) || workflow == "R")
                return 0;
            if (workflow == "A")
                return interval.Aggregate(1L, (a, b) => a * (b.End - b.Start + 1));

            var conditions = workflows[workflow];
            var rest = interval;
            var combs = 0L;

            foreach (var condition in conditions)
            {
                var res = separate(rest, condition);
                rest = res.rest;
                combs += getCombinations(res.match, condition.Redirect);
            }

            return combs;
        }

        (Interval[] match, Interval[] rest) separate(Interval[] intervals, Condition condition)
        {
            var match = new Interval[4];
            var rest = new Interval[4];
            intervals.CopyTo(match, 0);
            intervals.CopyTo(rest, 0);

            var ind = condition.PartInd;
            var value = condition.Value;
            if (condition.Mult == 1)
            {
                match[ind].Start = Math.Max(value + 1, match[ind].Start);
                rest[ind].End = Math.Min(value, rest[ind].End);
            }
            else
            {
                match[ind].End = Math.Min(value - 1, match[ind].End);
                rest[ind].Start = Math.Max(value, rest[ind].Start);
            }

            return (match, rest);
        }
    }


    private record Condition(int PartInd, int Mult, int Value, string Redirect);

    private record struct Interval(int Start, int End);
}
