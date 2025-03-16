using System.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Otel.Test;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    
    Counter<int> counter = MeterContext.Context.CreateCounter<int>("iterations.count");

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var workerActivity = ActivitySourceContext.Context.StartActivity("Worker Executing"))
        {
            var i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                counter.Add(1);
                i++;
                using (var iterationActvity = ActivitySourceContext.Context.StartActivity($"Iteration {i}"))
                {
                    _logger.LogInformation("Tick! {@currentDateTime}", DateTime.Now);
                    DoSomething();
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }

    private void DoSomething()
    {
        using (var activity = ActivitySourceContext.Context.StartActivity($"DoSomething"))
        {
            //Doing something..
        }
    }
}