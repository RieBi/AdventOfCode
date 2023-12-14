using AdventOfCode.Year2023;
using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode;

internal class Program
{
    static void Main(string[] args)
    {
        Run(13);
    }

    static void Run(int num)
    {
        var days = GetOrderedDescendingDays();
        var concreteDay = days.Where(f => f.attr.DayNumber == num);
        if (!concreteDay.Any())
            Console.WriteLine("No suitable days found");
        else
            ExecuteDay(concreteDay.First());
    }

    static void ExecuteDay((Day day, AocDayAttribute attr) pair)
    {
        var (day, attr) = (pair.day, pair.attr);
        var dayNumber = attr.DayNumber;

        var path = @$".\Year2023\Input\Day{dayNumber}.txt";
        var input = File.ReadAllText(path);
        day.Input = input;
        var watch = new Stopwatch();

        Console.WriteLine("Part one:");
        watch.Start();
        try
        {
            day.PartOne();
        }
        catch (Exception exc)
        {
            Console.WriteLine("Catched an exception");
            Console.WriteLine(exc);
        }
        watch.Stop();
        Console.WriteLine($"Part one taken: {watch.ElapsedMilliseconds} ms");

        Console.WriteLine();
        Console.WriteLine("Part two:");
        watch.Restart();
        try
        {
            day.PartTwo();
        }
        catch (Exception exc)
        {
            Console.WriteLine("Catched an exception");
            Console.WriteLine(exc);
        }
        watch.Stop();
        Console.WriteLine($"Part two taken: {watch.ElapsedMilliseconds} ms");
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