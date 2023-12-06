namespace AdventOfCode.Year2023; internal class Day5 : Day {     string lineSeparator = "\r\n";      public Day5()     {         this.DayNumber = 5;     }      public override void PartOne()     {         var records = Input.Split($"{lineSeparator}{lineSeparator}");         var seeds = new List<long>();          var pattern1 = @"seeds:(?: +(\d+))+";         var match1 = Regex.Match(records[0], pattern1);         foreach (Capture num in match1.Groups[1].Captures)             seeds.Add(long.Parse(num.Value));          var pattern2 = @" *(?<destination>\d+) *(?<start>\d+) *(?<length>\d+)";         foreach (var record in records.Skip(1))         {             var matches2 = Regex.Matches(record, pattern2);              var newSeeds = new List<long>();             // Through each three numbers             foreach (Match match in matches2)             {                 var destination = long.Parse(match.Groups["destination"].Value);                 var start = long.Parse(match.Groups["start"].Value);                 var length = long.Parse(match.Groups["length"].Value);                 var diff = destination - start;                  for (int i = 0; i < seeds.Count; i++)                 {                     long seed = seeds[i];                     if (seed >= start && seed < start + length)                     {                         var newSeed = seed + diff;                         newSeeds.Add(newSeed);                         seeds.RemoveAt(i);                         i--;                     }                 }              }              // All seeds that have no mapped value             foreach (var seed in seeds)                 newSeeds.Add(seed);              seeds = newSeeds;         }          Console.WriteLine(seeds.Min());     }      public override void PartTwo()     {         var records = Input.Split($"{lineSeparator}{lineSeparator}");         var seeds = new List<(long, long)>();          var pattern1 = @" *(?<num1>\d+) *(?<num2>\d+)";         var seedMatches = Regex.Matches(records[0], pattern1);         foreach (Match match in seedMatches)
        {
            var num1 = long.Parse(match.Groups["num1"].Value);
            var num2 = long.Parse(match.Groups["num2"].Value);
            seeds.Add((num1, num2));
        }          var pattern2 = @" *(?<destination>\d+) *(?<start>\d+) *(?<length>\d+)";         foreach (var record in records.Skip(1))         {             var matches2 = Regex.Matches(record, pattern2);              var newSeeds = new List<(long, long)>();             // Through each three numbers             foreach (Match match in matches2)             {                 var destination = long.Parse(match.Groups["destination"].Value);                 var start = long.Parse(match.Groups["start"].Value);                 var length = long.Parse(match.Groups["length"].Value);                 var diff = destination - start;                  for (int i = 0; i < seeds.Count; i++)                 {                     var seed = seeds[i];                     if ((seed.Item1 + seed.Item2 <= start || start + length <= seed.Item1))                         continue;                     var leftOnes = TransformInterval(seed, (start, length, diff), newSeeds);                     if (leftOnes.Count > 0)                         seeds[i] = leftOnes[0];                     if (leftOnes.Count == 2)                         seeds.Add(leftOnes[1]);                     if (leftOnes.Count == 0)
                    {
                        seeds.RemoveAt(i);
                        i--;
                    }                 }              }

            // All seeds that have no mapped value
            foreach (var seed in seeds)                 newSeeds.Add(seed);              seeds = newSeeds;         }          Console.WriteLine(seeds.MinBy(f => f.Item1).Item1);     }      private List<(long start, long length)> TransformInterval((long start, long length) seedInterval, (long start, long length, long diff) transformInterval, List<(long, long)> newSeeds)
    {
        var result = new List<(long start, long length)>();
        if (seedInterval.start + seedInterval.length <= transformInterval.start || transformInterval.start + transformInterval.length <= seedInterval.start)
            return result;

        if (seedInterval.start < transformInterval.start)
        {
            var start = seedInterval.start;
            var length = transformInterval.start - start;
            result.Add((start, length));
        }

        if (seedInterval.start + seedInterval.length > transformInterval.start + transformInterval.length)
        {
            var start = transformInterval.start + transformInterval.length;
            var seedEnd = seedInterval.start + seedInterval.length;
            var transformEnd = transformInterval.start + transformInterval.length;
            var length = seedEnd - transformEnd;
            result.Add((start, length));
        }

        var resultStart = Math.Max(seedInterval.start, transformInterval.start);
        var resultEnd = Math.Min(seedInterval.start + seedInterval.length, transformInterval.start + transformInterval.length);
        var resultLength = resultEnd - resultStart;

        if (resultLength > 0)
            newSeeds.Add((resultStart + transformInterval.diff, resultLength));

        return result;
    } } 