var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Create metrics instance
var metrics = new PrometheusMetricsExporter();

// Example route that increments a metric
app.MapGet("/", () =>
{
    metrics.IncrementCounter("myapp_requests_total", help: "Total number of processed requests");
    return "Web app is running!";
});

// /metrics endpoint for Prometheus
app.MapGet("/metrics", () =>
{
    var result = metrics.GetMetrics();
    return Results.Text(result, "text/plain");
});

app.Run();
