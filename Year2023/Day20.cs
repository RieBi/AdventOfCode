namespace AdventOfCode.Year2023;
[AocDay(20)]
internal class Day20 : Day
{
    public override void PartOne()
    {
        var modules = new Dictionary<string, Module>();
        string[] broadcasted = default!;
        foreach (var line in Utils.Lines(Input))
        {
            var arrowInd = line.IndexOf('>') + 1;
            var whitespaceInd = line.IndexOf(' ');
            var name = line[1..whitespaceInd];
            var destinations = line[arrowInd..].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var start = line[0];

            if (start == 'b')
                broadcasted = destinations;
            else if (start == '%')
                modules[name] = new FlipFlop(modules, destinations);
            else
                modules[name] = new Conjunction(modules, destinations);
        }

        foreach (var module in modules)
        {
            foreach (var dist in module.Value.Destinations)
            {
                if (modules.TryGetValue(dist, out Module? mod) && mod is Conjunction conj)
                    conj.Connections.Add(module.Key, false);
            }
        }

        var lowCount = 0L;
        var highCount = 0L;
        var queue = new Queue<(PulseType pulse, string? caller, string destination)>();

        for (int i = 0; i < 1000; i++)
        {
            lowCount++;
            foreach (var dest in broadcasted)
                queue.Enqueue((PulseType.Low, null, dest));

            while (queue.Count > 0)
            {
                var elem = queue.Dequeue();

                if (elem.pulse == PulseType.Low)
                    lowCount++;
                else if (elem.pulse == PulseType.High)
                    highCount++;

                if (!modules.TryGetValue(elem.destination, out Module? module))
                    continue;
                
                var pulse = module.ProcessPulse(elem.pulse, elem.caller);

                if (pulse != PulseType.None)
                {
                    foreach (var dest in modules[elem.destination].Destinations)
                    {
                        queue.Enqueue((pulse, elem.destination, dest));
                    }
                }
            }
        }

        Console.WriteLine(lowCount * highCount);
    }

    public override void PartTwo()
    {
        var modules = new Dictionary<string, Module>();
        string[] broadcasted = default!;
        foreach (var line in Utils.Lines(Input))
        {
            var arrowInd = line.IndexOf('>') + 1;
            var whitespaceInd = line.IndexOf(' ');
            var name = line[1..whitespaceInd];
            var destinations = line[arrowInd..].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var start = line[0];

            if (start == 'b')
                broadcasted = destinations;
            else if (start == '%')
                modules[name] = new FlipFlop(modules, destinations);
            else
                modules[name] = new Conjunction(modules, destinations);
        }

        foreach (var module in modules)
        {
            foreach (var dist in module.Value.Destinations)
            {
                if (modules.TryGetValue(dist, out Module? mod) && mod is Conjunction conj)
                    conj.Connections.Add(module.Key, false);
            }
        }

        var prefinal = modules.Where(f => f.Value.Destinations.Contains("rx")).First().Key;
        var counter = modules.Where(f => f.Value.Destinations.Contains(prefinal)).Select(f => f.Key).ToList();
        var counterValues = new List<long>();

        var lowCount = 0L;
        var highCount = 0L;
        var queue = new Queue<(PulseType pulse, string? caller, string destination)>();

        for (int i = 0; i < 100000000; i++)
        {
            lowCount++;
            foreach (var dest in broadcasted)
                queue.Enqueue((PulseType.Low, null, dest));

            while (queue.Count > 0)
            {
                var elem = queue.Dequeue();

                if (counter.Contains(elem.destination) && elem.pulse == PulseType.Low)
                {
                    counterValues.Add(i + 1);
                    if (counterValues.Count == counter.Count)
                    {
                        Console.WriteLine(counterValues.Aggregate((a, b) => Utils.LCM(a, b)));
                        return;
                    }
                        
                }

                if (elem.pulse == PulseType.Low)
                    lowCount++;
                else if (elem.pulse == PulseType.High)
                    highCount++;

                if (!modules.TryGetValue(elem.destination, out Module? module))
                    continue;

                var pulse = module.ProcessPulse(elem.pulse, elem.caller);

                if (pulse != PulseType.None)
                {
                    foreach (var dest in modules[elem.destination].Destinations)
                    {
                        queue.Enqueue((pulse, elem.destination, dest));
                    }
                }
            }
        }

        Console.WriteLine("Did not succeed");
    }

    abstract class Module
    {
        public Module(Dictionary<string, Module> modules, string[] destinations)
        {
            Modules = modules;
            Destinations = destinations;
        }

        public string[] Destinations { get; set; }
        public Dictionary<string, Module> Modules { get; set; }

        public abstract PulseType ProcessPulse(PulseType incoming, string? caller);
    }

    class FlipFlop(Dictionary<string, Module> modules, string[] destinations) : Module(modules, destinations)
    {
        public bool IsOn { get; set; } = false;

        public override PulseType ProcessPulse(PulseType incoming, string? _)
        {
            if (incoming == PulseType.High)
                return PulseType.None;

            IsOn = !IsOn;
            return IsOn ? PulseType.High : PulseType.Low;
        }
    }

    class Conjunction(Dictionary<string, Module> modules, string[] destinations) : Module(modules, destinations)
    {
        private int Count = 0;

        public Dictionary<string, bool> Connections { get; set; } = [];

        public override PulseType ProcessPulse(PulseType incoming, string? caller)
        {
            if (caller is not null)
            {
                var stored = Connections[caller];
                if (stored is true && incoming == PulseType.Low)
                    Count--;
                else if (stored is false && incoming == PulseType.High)
                    Count++;

                Connections[caller] = incoming == PulseType.Low ? false : true;
            }

            return Count == Connections.Count ? PulseType.Low : PulseType.High;
        }
    }

    enum PulseType
    {
        None,
        Low,
        High
    }
}
