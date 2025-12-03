using CustomRoselynAnalyzer.Core;

namespace CustomRoselynAnalyzer.Configuration;

internal sealed class StaticRuleConfigurationSource : IRuleConfigurationSource
{
    public RuleConfiguration GetConfiguration(RuleDescriptorInfo info) =>
        RuleConfiguration.FromDefaults(info);
}
