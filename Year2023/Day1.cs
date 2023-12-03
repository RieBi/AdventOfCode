using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023;
internal class Day1 : Day
{
    private char[] numbers = ['1', '2', '3', '4', '5', '6', '7', '8', '9'];

	public Day1()
	{
		this.DayNumber = 1;
	}

    public override void PartOne()
    {
        var sum = 0;
        foreach (var line in Input.Split('\n'))
        {
            var firstNumber = line.First(f => numbers.Contains(f)).ToString();
            var secondNumber = line.Last(f => numbers.Contains(f)).ToString();
            var combinedInt = int.Parse(firstNumber + secondNumber);
            sum += combinedInt;
        }

        Console.WriteLine(sum);
    }

    public override void PartTwo()
    {
        var pattern = @"(?=(one|two|three|four|five|six|seven|eight|nine|\d))";
        var sum = 0;

        var transform = new Dictionary<string, string>
        {
            { "one", "1" },
            { "two", "2" },
            { "three", "3" },
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eight", "8" },
            { "nine", "9" },
            { "1", "1" },
            { "2", "2" },
            { "3", "3" },
            { "4", "4" },
            { "5", "5" },
            { "6", "6" },
            { "7", "7" },
            { "8", "8" },
            { "9", "9" }
        };

        foreach (var line in Input.Split('\n'))
        {
            var matches = Regex.Matches(line, pattern);
            var firstNumber = transform[matches.First().Groups[1].Value];
            var secondNumber = transform[matches.Last().Groups[1].Value];
            var combined = firstNumber + secondNumber;
            sum += int.Parse(combined);
        }

        Console.WriteLine(sum);
    }
}
