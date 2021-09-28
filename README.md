# BenchmarkChoco
This is a banchmark application to compare performances of two approaches of obtaining installed software information: parsing choco.exe output and using chocolatey.lib library.

First test result with 20 iterations is surprising. I'll try with 100 attempts.

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.18363.1801 (1909/November2019Update/19H2)
AMD A10-9600P RADEON R5, 10 COMPUTE CORES 4C+6G, 1 CPU, 4 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
  Job-TABFKX : .NET Framework 4.8 (4.8.4400.0), X86 LegacyJIT
  
IterationCount=20  LaunchCount=1  RunStrategy=Monitoring WarmupCount=1  
```
| Method	| Mean	| Error	| StdDev |
|-|-|-|-|
| ParsingExecutableOutputStrategy	| 335,724.0 ms	| 18,837.78 ms |	21,693.6 ms |
| UsingLibraryStrategy	| 520.5 ms	| 91.09 ms	| 104.9 ms |
