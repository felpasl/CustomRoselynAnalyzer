using CustomRoslynAnalyzer.Core;

namespace CustomRoslynAnalyzer.Configuration;

internal sealed class StaticRuleConfigurationSource : IRuleConfigurationSource
{
    public RuleConfiguration GetConfiguration(RuleDescriptorInfo info) =>
        RuleConfiguration.FromDefaults(info);
}
