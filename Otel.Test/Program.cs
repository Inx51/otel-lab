using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Otel.Test;
using Serilog;

ActivitySourceContext.Context = new ActivitySource("DemoApp");
MeterContext.Context = new Meter("DemoApp");

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithMetrics(c =>
    {
        c.AddOtlpExporter()
            .AddMeter("DemoApp")
            .AddConsoleExporter();
    })
    .WithTracing(c =>
    {
        c.AddOtlpExporter()
            .AddSource("DemoApp")
            .AddConsoleExporter();
    })
    .WithLogging(c =>
    {
        c.AddOtlpExporter()
            .AddConsoleExporter();
    });
    
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.Run();