using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using WeatherAPI;
using WeatherAPI.Controllers;

[MemoryDiagnoser]
[RankColumn, AllStatisticsColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[MinIterationCount(30)]
public class WeatherAPIBenchmarker
{
    private DefaultHttpContext httpContext;
    private WeatherForecastController weatherForecastController;

    // this will deal the methods returning IEnumerable<> 
    private readonly Consumer consumer = new Consumer();

    // common initialization for every test that we do not want to include in measurement
    [GlobalSetup]
    public void GlobalSetup()
    {
        httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Add("MaxRecords", "50");

        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        var factory = serviceProvider.GetService<ILoggerFactory>();

        var logger = factory.CreateLogger<WeatherForecastController>();

        weatherForecastController = new WeatherForecastController(logger);
    }

    [Benchmark(Baseline = true)]
    public void GetWithoutAnyArgument()
    {
        Consume(weatherForecastController.GetWithoutAnyArgument());
    }

    [Benchmark]
    public void GetWithPrimitiveTypeArgument()
    {
        Consume(weatherForecastController.GetWithPrimitiveArgument(50));
    }

    [Benchmark]
    public void GetWithComplexTypeArgument()
    {
        Consume(weatherForecastController.GetWithComplexArgument(httpContext));
    }

    /// <summary>
    /// Regarding the return of IEnumerables, we need to either:
    /// - change the method declaration to return a materialized result (i.e. List<>)
    /// - or consume it on our own.
    /// </summary>
    /// <param name="forecasts"></param>
    private void Consume(IEnumerable<WeatherForecast> forecasts)
    {
        foreach (var forecast in forecasts)
        {
            consumer.Consume(forecast);
        }
    }

}