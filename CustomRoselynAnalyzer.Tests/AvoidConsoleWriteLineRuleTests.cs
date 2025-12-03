using System.Threading.Tasks;
using CustomRoselynAnalyzer.Rules;
using Microsoft.CodeAnalysis.Testing;
using VerifyCS = CustomRoselynAnalyzer.Tests.Helpers.CSharpAnalyzerVerifier<CustomRoselynAnalyzer.CustomUsageAnalyzer>;

namespace CustomRoselynAnalyzer.Tests;

public sealed class AvoidConsoleWriteLineRuleTests
{
    [Fact]
    public async Task ReportsConsoleWriteLineInvocation()
    {
        const string testCode = @"
using System;

class C
{
    void M()
    {
        System.Console.{|#0:WriteLine|}(""diagnostic"");
    }
}";

        var expected = VerifyCS.Diagnostic(AvoidConsoleWriteLineRule.DefaultDescriptor)
            .WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(testCode, expected);
    }

    [Fact]
    public async Task DoesNotReportOtherCalls()
    {
        const string testCode = @"
using System.Diagnostics;

class C
{
    void M()
    {
        Trace.WriteLine(""ok"");
    }
}";

        await VerifyCS.VerifyAnalyzerAsync(testCode);
    }
}
