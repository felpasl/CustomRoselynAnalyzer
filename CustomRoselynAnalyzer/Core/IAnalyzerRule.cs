using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CustomRoselynAnalyzer.Core;

internal interface IAnalyzerRule
{
    DiagnosticDescriptor Descriptor { get; }
    void Register(CompilationStartAnalysisContext context);
}
