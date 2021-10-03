# BenchmarkChoco
This is a benchmark application to compare performances of two approaches of obtaining installed software information: parsing `choco.exe` output and using chocolatey.lib library.

The overhead of creating a new process for every query is huge. Yet,there are better options:
1. Use one `choco` command to list all applications in detail at once. However, it would create an overhead of parsing huge load of text, complicating the code. It will add huge performance gain. With Benchmark 5, I added the results of this approach. The result is actually impressive.
2. Use Chocolatey's own `chocolatey.lib` library to extract the list of applications. Tests show significant performance gain and the amount of gain seems suspicious. There's a need for checking loss off data. Also it adds two dependencies: `chocolatey.lib` and it's unused dependency `log4net`.

## Summary
Test results show significant (about 600 times faster) performance improvement. With unit tests, it's ensured that there is not a difference in the outputs. Also, this numbers include console output times. Considering the speed of the actual data extraction, it would probably take less time to use `Debug.Write` instead of `Console.WriteLine` in actual use. But in test, I used `Console.WriteLine` for a fair comparison. Actual PR will probably include only `Debug.Write`

## TO-DO
[x] Use parallel (`PLINQ` or `Parallel.ForEach`)

## Tests
The solution includes 2 projects:
1. **BenchmarkChoco:** Uses BenchmarkDotNet to compare results of two different methods. To disable the effects of a cold start, 5 warmup iterations were added. Afterwards 100 iterations were run. It takes hours so it is advised to start small at first. With the setup in the tests and 68 packages, the benchmark takes about 10 hours.
2. **BenchmarkChoco.Tests:** Simple unit tests to ensure the results of the methods are the same.
3. **BenchmarkChoco.App:** A light console application for integrity tests and small checks.

## Run the tests
1. Clone the repository.
2. Open the solution in Visual Studio.
3. Select Release mode and start (required by BenchmarkDotNet).
4. For unit tests, just go to the test class and run all tests.

### Benchmark 1
``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.18363.1801 (1909/November2019Update/19H2)
AMD A10-9600P RADEON R5, 10 COMPUTE CORES 4C+6G, 1 CPU, 4 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
  Job-TABFKX : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
  
IterationCount=20  LaunchCount=1  RunStrategy=Monitoring
WarmupCount=1  
```
|                          Method |         Mean |        Error |      StdDev |
|-------------------------------- |-------------:|-------------:|------------:|
| ParsingExecutableOutputStrategy	| 335,724.0 ms | 18,837.78 ms |	21,693.6 ms |
|            UsingLibraryStrategy |     520.5 ms |     91.09 ms	|    104.9 ms |


### Benchmark 2
``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.18363.1801 (1909/November2019Update/19H2)
AMD A10-9600P RADEON R5, 10 COMPUTE CORES 4C+6G, 1 CPU, 4 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
  Job-IUGHYI : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT

IterationCount=100  LaunchCount=1  RunStrategy=Monitoring  
WarmupCount=5  

```
|                          Method |         Mean |     Error |      StdDev |
|-------------------------------- |-------------:|----------:|------------:|
| ParsingExecutableOutputStrategy | 331,028.4 ms | 816.17 ms | 2,406.48 ms |
|            UsingLibraryStrategy |     692.6 ms |  20.90 ms |    61.64 ms |

### Benchmark 3
``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.18363.1801 (1909/November2019Update/19H2)
AMD A10-9600P RADEON R5, 10 COMPUTE CORES 4C+6G, 1 CPU, 4 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT  [AttachedDebugger]
  Job-IUGHYI : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT

IterationCount=100  LaunchCount=1  RunStrategy=Monitoring  
WarmupCount=5  

```
|                          Method |         Mean |       Error |       StdDev |
|-------------------------------- |-------------:|------------:|-------------:|
| ParsingExecutableOutputStrategy | 349,816.4 ms | 3,934.71 ms | 11,601.57 ms |
|            UsingLibraryStrategy |     680.6 ms |    33.53 ms |     98.86 ms |

### Benchmark 4
``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.18363.1801 (1909/November2019Update/19H2)
AMD A10-9600P RADEON R5, 10 COMPUTE CORES 4C+6G, 1 CPU, 4 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT  [AttachedDebugger]
  Job-AJPCRR : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT

IterationCount=100  LaunchCount=1  RunStrategy=Monitoring  
WarmupCount=5  

```
|                          Method |         Mean |        Error |       StdDev |       Median |
|-------------------------------- |-------------:|-------------:|-------------:|-------------:|
| ParsingExecutableOutputStrategy | 373,823.1 ms | 17,892.15 ms | 52,755.42 ms | 350,595.3 ms |
|            UsingLibraryStrategy |     557.4 ms |     32.48 ms |     95.76 ms |     588.8 ms |

### Benchmark 5
``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.18363.1801 (1909/November2019Update/19H2)
AMD A10-9600P RADEON R5, 10 COMPUTE CORES 4C+6G, 1 CPU, 4 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT  [AttachedDebugger]
  Job-AJPCRR : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT

IterationCount=100  LaunchCount=1  RunStrategy=Monitoring  
WarmupCount=5  

```
|                          Method |         Mean |       Error |       StdDev |       Median |
|-------------------------------- |-------------:|------------:|-------------:|-------------:|
| ParsingExecutableOutputStrategy | 339,615.4 ms | 7,150.17 ms | 21,082.43 ms | 322,660.8 ms |
|            UsingLibraryStrategy |     679.2 ms |    20.87 ms |     61.54 ms |     697.2 ms |
|           ParsingSingleStrategy |   6,021.2 ms |   116.28 ms |    342.86 ms |   6,091.6 ms |
