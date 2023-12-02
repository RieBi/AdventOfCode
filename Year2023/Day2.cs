﻿using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023;
internal class Day2 : Day
{
    const int redMax = 12;
    const int greenMax = 13;
    const int blueMax = 14;
    readonly string pattern = @"Game (?<num>\w+): (?:(\w+ (?:green|red|blue))[,;]? ?)*";

    public Day2()
    {
        this.DayNumber = 2;
    }

    public override void PartOne()
    {
        var resultSum = 0;
        var games = Input.Split('\n');
        foreach (var game in games)
        {
            var groups = Regex.Match(game, pattern).Groups;
            var isGameValid = true;
            foreach (Capture capture in groups[1].Captures)
            {
                var subset = capture.Value.Split(' ');
                var num = int.Parse(subset[0]);
                var color = subset[1];

                if ((color == "blue" && num > blueMax) ||
                    (color == "red" && num > redMax) ||
                    (color == "green" && num > greenMax))
                {
                    isGameValid = false;
                    break;
                }
            }

            if (!isGameValid)
                continue;
            resultSum += int.Parse(groups["num"].Value);
        }

        Console.WriteLine(resultSum);
    }

    public override void PartTwo()
    {
        var resultSum = 0;
        var games = Input.Split('\n');
  
        foreach (var game in games)
        {
            var leastRed = 0;
            var leastGreen = 0;
            var leastBlue = 0;

            var groups = Regex.Match(game, pattern).Groups;
            foreach (Capture capture in groups[1].Captures)
            {
                var subset = capture.Value.Split(' ');
                var num = int.Parse(subset[0]);
                var color = subset[1];

                switch (color)
                {
                    case "red":
                        leastRed = Math.Max(leastRed, num);
                        break;
                    case "green":
                        leastGreen = Math.Max(leastGreen, num);
                        break;
                    case "blue":
                        leastBlue = Math.Max(leastBlue, num);
                        break;
                }
            }

            var power = leastRed * leastGreen * leastBlue;
            resultSum += power;
        }

        Console.WriteLine(resultSum);
    }
}
