using CustomRoslynAnalyzer.Rules;
using Microsoft.CodeAnalysis.Testing;
using VerifyCS = CustomRoslynAnalyzer.Tests.Helpers.CSharpAnalyzerVerifier<CustomRoslynAnalyzer.CustomUsageAnalyzer>;

namespace CustomRoslynAnalyzer.Tests;

public sealed class NestedInfrastructureLoopRuleTests
{
    [Fact]
    public async Task ReportsInfrastructureCallInsideLoop()
    {
        const string testCode = @"
namespace Demo
{
    public class Processor
    {
        private readonly Sample.Infrastructure.Repository _repository = new();

        public void Process()
        {
            for (var i = 0; i < 3; i++)
            {
                {|#0:_repository.Save()|};
            }
        }
    }
}

namespace Sample.Infrastructure
{
    public sealed class Repository
    {
        public void Save()
        {
        }
    }
}";

        var expected = VerifyCS.Diagnostic(NestedInfrastructureLoopRule.DefaultDescriptor)
            .WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(testCode, expected);
    }

    [Fact]
    public async Task DoesNotReportInfrastructureCallOutsideLoop()
    {
        const string testCode = @"
namespace Demo
{
    public class Processor
    {
        private readonly Sample.Infrastructure.Repository _repository = new();

        public void Process()
        {
            var snapshots = _repository.GetAll();
            foreach (var item in snapshots)
            {
                _ = item;
            }
        }
    }
}

namespace Sample.Infrastructure
{
    public sealed class Repository
    {
        public int[] GetAll() => new int[0];
    }
}";

        await VerifyCS.VerifyAnalyzerAsync(testCode);
    }
}
