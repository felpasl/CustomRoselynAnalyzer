using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CustomRoselynAnalyzer.Configuration;

internal static class RuleConfigurationSourceFactory
{
    public static IRuleConfigurationSource Create(AnalyzerConfigOptionsProvider? optionsProvider)
    {
        if (optionsProvider is null)
        {
            return new StaticRuleConfigurationSource();
        }

        return new AnalyzerConfigRuleConfigurationSource(optionsProvider);
    }
}
