using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace CustomRoslynAnalyzer.Tests.Helpers;

internal static class CSharpAnalyzerVerifier<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor) => new(descriptor);

    public static Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expectedDiagnostics)
    {
        var test = new Test
        {
            TestCode = source
        };

        if (expectedDiagnostics.Length > 0)
        {
            test.ExpectedDiagnostics.AddRange(expectedDiagnostics);
        }

        return test.RunAsync(CancellationToken.None);
    }

    private sealed class Test : CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
    {
        public Test()
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net60;
        }
    }
}
