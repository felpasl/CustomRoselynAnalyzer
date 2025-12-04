using System.Threading.Tasks;
using CustomRoslynAnalyzer.Rules;
using Microsoft.CodeAnalysis.Testing;
using VerifyCS = CustomRoslynAnalyzer.Tests.Helpers.CSharpAnalyzerVerifier<CustomRoslynAnalyzer.CustomUsageAnalyzer>;

namespace CustomRoslynAnalyzer.Tests;

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

    [Fact]
    public async Task ReportsConsoleWriteLineWithoutSystemPrefix()
    {
        const string testCode = @"
using System;

class C
{
    void M()
    {
        Console.{|#0:WriteLine|}(""diagnostic"");
    }
}";

        var expected = VerifyCS.Diagnostic(AvoidConsoleWriteLineRule.DefaultDescriptor)
            .WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(testCode, expected);
    }

    [Fact]
    public async Task ReportsConsoleWriteLineWithOverload()
    {
        const string testCode = @"
using System;

class C
{
    void M()
    {
        Console.{|#0:WriteLine|}();
    }
}";

        var expected = VerifyCS.Diagnostic(AvoidConsoleWriteLineRule.DefaultDescriptor)
            .WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(testCode, expected);
    }
}
