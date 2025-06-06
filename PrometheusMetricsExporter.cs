using System.Collections.Concurrent;
using System.Text;

public class PrometheusMetricsExporter
{
    private readonly ConcurrentDictionary<string, double> _counters = new();
    private readonly ConcurrentDictionary<string, string> _helpText = new();

    public void IncrementCounter(string name, double increment = 1.0, string help = "")
    {
        _counters.AddOrUpdate(name, increment, (_, val) => val + increment);
        if (!string.IsNullOrWhiteSpace(help))
        {
            _helpText.TryAdd(name, help);
        }
    }

    public string GetMetrics()
    {
        var sb = new StringBuilder();

        foreach (var kvp in _counters)
        {
            string name = kvp.Key;
            double value = kvp.Value;

            if (_helpText.TryGetValue(name, out var help))
            {
                sb.AppendLine($"# HELP {name} {help}");
            }

            sb.AppendLine($"# TYPE {name} counter");
            sb.AppendLine($"{name} {value}");
        }

        return sb.ToString();
    }
}
