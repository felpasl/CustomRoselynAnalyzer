using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CustomRoslynAnalyzer.Core;

internal interface IAnalyzerRule
{
    DiagnosticDescriptor Descriptor { get; }
    void Register(CompilationStartAnalysisContext context);
}
