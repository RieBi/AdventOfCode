namespace AdventOfCode.Year2023;
[AocDay(15)]
internal class Day15 : Day
{
    public override void PartOne()
    {
        var steps = Input.Split(',');
        var result = steps.Aggregate(0, (a, b) => a + Hash(b));
        Console.WriteLine(result);
    }

    public override void PartTwo()
    {
        var steps = Input.Split(',');
        var boxes = new List<List<(string label, int focal)>>(256);
        for (int i = 0; i < 256; i++)
            boxes.Add(new List<(string label, int focal)>(9));

        foreach (var step in steps)
        {
            var parts = step.Split(['=', '-']);
            var box = Hash(parts[0]);

            if (parts[1] == "")
            {
                boxes[box].RemoveAll(f => f.label == parts[0]);
            }
            else
            {
                var newLens = (parts[0], int.Parse(parts[1]));
                var ind = boxes[box].FindIndex(f => f.label == parts[0]);
                if (ind == -1)
                    boxes[box].Add(newLens);
                else
                    boxes[box][ind] = newLens;
            }
        }

        var totalPower = 0L;
        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = 0; j < boxes[i].Count; j++)
               totalPower += (i + 1) * (j + 1) * (boxes[i][j].focal);
        }

        Console.WriteLine(totalPower);
    }

    int Hash(string s)
    {
        var value = 0;
        foreach (var ch in s)
        {
            value += ch;
            value *= 17;
            value %= 256;
        }

        return value;
    }
}
