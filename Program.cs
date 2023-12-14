using AdventOfCode.Year2023;
using System.Reflection;

namespace AdventOfCode;

internal class Program
{
    static void Main(string[] args)
    {
        Run();
    }

    static void Run()
    {
        var days = GetOrderedDescendingDays();
        if (!days.Any())
            Console.WriteLine("No suitable days found");
        else
            ExecuteDay(days.First());
    }

    static void ExecuteDay((Day day, AocDayAttribute attr) pair)
    {
        var (day, attr) = (pair.day, pair.attr);
        var dayNumber = attr.DayNumber;

        var path = @$".\Year2023\Input\Day{dayNumber}.txt";
        var input = File.ReadAllText(path);
        day.Input = input;

        Console.WriteLine("Part one:");
        try
        {
            day.PartOne();
        }
        catch (Exception exc)
        {
            Console.WriteLine("Catched an exception");
            Console.WriteLine(exc);
        }

        Console.WriteLine();
        Console.WriteLine("Part two:");
        try
        {
            day.PartTwo();
        }
        catch (Exception exc)
        {
            Console.WriteLine("Catched an exception");
            Console.WriteLine(exc);
        }
    }

    static IEnumerable<(Day day, AocDayAttribute attr)> GetOrderedDescendingDays()
    {
        var dayInherits = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(f => f.IsSubclassOf(typeof(Day)));

        var results = new List<(Day day, AocDayAttribute attr)>();
        foreach (var v in dayInherits)
        {
            var obj = Activator.CreateInstance(v);
            var attr = v.GetCustomAttribute<AocDayAttribute>();
            if (attr is not null && obj is not null)
                results.Add(((Day)obj, attr));
        }

        return results.OrderByDescending(f => f.attr.DayNumber);
    }
}

public abstract class Day
{
    public string Input { get; set; } = default!;

    public virtual void PartOne() { }

    public virtual void PartTwo() { }
}

[AttributeUsage(AttributeTargets.Class)]
public class AocDayAttribute : Attribute
{
    public int DayNumber { get; set; }

    public AocDayAttribute(int dayNumber)
    {
        DayNumber = dayNumber;
    }
}