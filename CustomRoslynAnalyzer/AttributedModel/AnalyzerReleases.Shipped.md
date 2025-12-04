; Shipped analyzer releases
; https://github.com/dotnet/roslyn-analyzers/blob/master/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

## Release 1.0.0

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|------
CR0001 | Usage | Warning | Avoid Console.WriteLine in favor of structured logging.
CR0002 | Naming | Warning | Require Async suffix on async public methods.
CR0003 | Usage | Warning | Prevent infrastructure calls inside loop bodies.
