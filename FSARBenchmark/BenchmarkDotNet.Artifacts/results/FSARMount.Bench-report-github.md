``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.593 (1809/October2018Update/Redstone5)
AMD E2-7110 APU with AMD Radeon R2 Graphics, 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.1.504
  [Host] : .NET Core 2.1.8 (CoreCLR 4.6.27317.03, CoreFX 4.6.27317.03), 64bit RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|                 Method |     Mean |     Error |    StdDev |   Median |
|----------------------- |---------:|----------:|----------:|---------:|
|      ReadFilesNoDecomp | 27.79 ms | 0.5548 ms | 1.5557 ms | 27.33 ms |
| ReadFilesDeflateStream | 36.95 ms | 0.7363 ms | 1.0560 ms | 36.62 ms |
|       ReadFilesSpreads | 30.79 ms | 0.5905 ms | 0.7469 ms | 30.87 ms |
