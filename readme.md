# Benchmark example

This ad-hoc solution provides an example of How to benchmark WebAPI methods with HttpContext as parameter.

## Solution items

- WeatherAPI: our service that contains methods to be microbenchmarked
- BenchmarkConsoleApp: our benchmarker project that will trigger requests to WeatherAPI project

## Remarks

Both projects are set as start up projects because must be run at the same time in Release configuration mode.

## Example result

```
BenchmarkDotNet=v0.13.3, OS=Windows 10 (10.0.19045.2364)
Intel Core i7-3632QM CPU 2.20GHz (Ivy Bridge), 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 6.0.12 (6.0.1222.56807), X64 RyuJIT AVX
  Job-WYCUUD : .NET 6.0.12 (6.0.1222.56807), X64 RyuJIT AVX

MinIterationCount=30

|                       Method |     Mean |    Error |   StdDev |   StdErr |      Min |       Q1 |   Median |       Q3 |      Max |     Op/s | Ratio | RatioSD | Rank |   Gen0 | Allocated | Alloc Ratio |
|----------------------------- |---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|------:|--------:|-----:|-------:|----------:|------------:|
| GetWithPrimitiveTypeArgument | 13.75 us | 0.227 us | 0.325 us | 0.062 us | 13.11 us | 13.57 us | 13.73 us | 13.86 us | 14.47 us | 72,727.1 |  0.96 |    0.04 |    1 | 0.8087 |   2.48 KB |        1.00 |
|   GetWithComplexTypeArgument | 14.13 us | 0.279 us | 0.409 us | 0.076 us | 13.22 us | 13.80 us | 14.12 us | 14.35 us | 14.92 us | 70,775.8 |  0.98 |    0.05 |    2 | 0.8087 |   2.51 KB |        1.01 |
|        GetWithoutAnyArgument | 14.32 us | 0.282 us | 0.564 us | 0.081 us | 13.38 us | 13.84 us | 14.24 us | 14.73 us | 15.73 us | 69,835.7 |  1.00 |    0.00 |    2 | 0.8087 |   2.48 KB |        1.00 |

// * Hints *
Outliers
  WeatherAPIBenchmarker.GetWithPrimitiveTypeArgument: MinIterationCount=30 -> 2 outliers were removed (14.94 us, 15.48 us)
  WeatherAPIBenchmarker.GetWithComplexTypeArgument: MinIterationCount=30   -> 2 outliers were removed (15.43 us, 20.79 us)

// * Legends *
  Mean        : Arithmetic mean of all measurements
  Error       : Half of 99.9% confidence interval
  StdDev      : Standard deviation of all measurements
  StdErr      : Standard error of all measurements
  Min         : Minimum
  Q1          : Quartile 1 (25th percentile)
  Median      : Value separating the higher half of all measurements (50th percentile)
  Q3          : Quartile 3 (75th percentile)
  Max         : Maximum
  Op/s        : Operation per second
  Ratio       : Mean of the ratio distribution ([Current]/[Baseline])
  RatioSD     : Standard deviation of the ratio distribution ([Current]/[Baseline])
  Rank        : Relative position of current benchmark mean among all benchmarks (Arabic style)
  Gen0        : GC Generation 0 collects per 1000 operations
  Allocated   : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  Alloc Ratio : Allocated memory ratio distribution ([Current]/[Baseline])
  1 us        : 1 Microsecond (0.000001 sec)
```