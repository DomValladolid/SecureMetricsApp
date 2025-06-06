var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var metrics = new PrometheusMetricsExporter();

app.MapGet("/", () =>
{
    metrics.IncrementCounter("myapp_requests_total", help: "Total number of processed requests");
    return "Hello Secure World!";
});

app.MapGet("/metrics", () =>
{
    var result = metrics.GetMetrics();
    return Results.Text(result, "text/plain");
});

app.Run();
