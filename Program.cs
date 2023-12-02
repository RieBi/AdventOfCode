using AdventOfCode.Year2023;

namespace AdventOfCode;

internal class Program
{
    static void Main(string[] args)
    {
        Solve<Day2>();
    }

    static void Solve<T>() where T : Day, new()
    {
        var day = new T();
        var path = @$".\Year2023\Input\Day{day.DayNumber}.txt";
        var input = File.ReadAllText(path);
        day.Input = input;

        Console.WriteLine("Part one:");
        try
        {
            day.PartOne();
        }
        catch
        {
            Console.WriteLine("Catched an exception");
        }
        Console.WriteLine();
        Console.WriteLine("Part two:");
        try
        {
            day.PartTwo();
        }
        catch
        {
            Console.WriteLine("Catched an exception");
        }
    }
}

public abstract class Day
{
    public string Input { get; set; } = default!;
    public int DayNumber { get; set; } = default!;

    public virtual void PartOne() { }

    public virtual void PartTwo() { }
}