# BenchmarkChoco
This is a benchmark application to compare performances of two approaches of obtaining installed software information: parsing choco.exe output and using chocolatey.lib library.

The overhead of creating a new process for every query is hugeThere are better options:
1. Use one `choco` command to list all applications in detail at once. However, it would create an overhead of parsing huge load of text, complicating the code. It will add huge performance gain.
2. Use Chocolatey's own `chocolatey.lib` library to extract thr list of applications. Tests show significant performance gain and the amount of gain seems suspicious. There's a need for checking loss off data. Also it adds two dependencies: `chocolatey.lib` and it's unused dependency `log4net`.

## Summary
Test results show significant (about 400 times faster) performance improvement. However, there might be a difference in the information provided by each query. I will add a unit teast to check the integrity of the information provided by each methods.

### Benchmark 1
``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.18363.1801 (1909/November2019Update/19H2)
AMD A10-9600P RADEON R5, 10 COMPUTE CORES 4C+6G, 1 CPU, 4 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
  Job-TABFKX : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
  
IterationCount=20  LaunchCount=1  RunStrategy=Monitoring WarmupCount=1  
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