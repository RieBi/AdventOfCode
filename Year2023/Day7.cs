namespace AdventOfCode.Year2023;
internal class Day7 : Day
{
    public Day7()
    {
        this.DayNumber = 7;
    }

    public override void PartOne()
    {
        List<(string hand, long bid)> lines = Input.Split('\n').Select(f =>
        {
            var values = f.Split(' ');
            var value1 = values[0];
            var value2 = long.Parse(values[1]);
            return (value1, value2);
        }).ToList();

        var sorted = lines.OrderBy(f => f.hand, new HandsComparer()).ToList();

        var sum = 0L;
        for (long i = 1; i <= sorted.Count; i++)
        {
            sum += i * sorted[(int)(i - 1)].bid;
        }

        Console.WriteLine(sum);
    }

    public override void PartTwo()
    {
        List<(string hand, long bid)> lines = Input.Split('\n').Select(f =>
        {
            var values = f.Split(' ');
            var value1 = values[0];
            var value2 = long.Parse(values[1]);
            return (value1, value2);
        }).ToList();

        var sorted = lines.OrderBy(f => f.hand, new HandsComparer() { part2 = true }).ToList();

        var sum = 0L;
        for (long i = 1; i <= sorted.Count; i++)
        {
            sum += i * sorted[(int)(i - 1)].bid;
        }

        Console.WriteLine(sum);
    }

    class HandsComparer : IComparer<string>
    {
        public bool part2 = false;

        public int Compare(string? a, string? b)
        {
            if (a == null && b != null)
                return -1;
            else if (a == null && b == null)
                return 0;
            else if (a != null && b == null)
                return 1;

            var strengthA = FindHandStrength(a!);
            var strengthB = FindHandStrength(b!);

            if (strengthA > strengthB)
                return 1;
            else if (strengthA < strengthB)
                return -1;

            for (int i = 0; i < 5; i++)
            {
                var comparedResult = CompareCards(a![i], b![i]);
                if (comparedResult != 0)
                    return comparedResult;
            }

            return 0;
        }

        int FindHandStrength(string hand)
        {
            var grouping = hand.GroupBy(f => f);
            var max = grouping.Max(f => f.Count());

            if (!part2)
            {
                if (max == 5)
                    return 7;
                if (max == 4)
                    return 6;
                if (grouping.Any(f => f.Count() == 3) && grouping.Any(f => f.Count() == 2))
                    return 5;
                if (max == 3)
                    return 4;
                if (grouping.Where(f => f.Count() == 2).Count() == 2)
                    return 3;
                if (max == 2)
                    return 2;

                return 1;
            }
            else
            {
                var jCount = hand.Count(f => f == 'J');
                var filtered = grouping.Where(f => f.Key != 'J');
                max = !filtered.Any() ? 0 : filtered.Max(f => f.Count());

                if (max == 5 || max + jCount == 5)
                    return 7;
                if (max == 4 || max + jCount == 4)
                    return 6;
                if ((grouping.Any(f => f.Count() == 3) && grouping.Any(f => f.Count() == 2)) ||
                    ((grouping.Where(f => f.Count() == 2).Count() == 2) && jCount == 1))
                    return 5;
                if (max == 3 || max + jCount == 3)
                    return 4;
                if ((grouping.Where(f => f.Count() == 2).Count() == 2) ||
                    (max == 2 && jCount == 1))
                    return 3;
                if (max == 2 || jCount == 1)
                    return 2;

                return 1;
            }
        }

        int CompareCards(char a, char b)
        {
            string order;
            if (!part2)
                order = "AKQJT98765432";
            else
                order = "AKQT98765432J";

            var aIndex = order.IndexOf(a);
            var bIndex = order.IndexOf(b);
            if (aIndex < bIndex)
                return 1;
            else if (aIndex == bIndex)
                return 0;
            else
                return -1;
        }
    }
}
