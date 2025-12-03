using CustomRoselynAnalyzer.Core;

namespace CustomRoselynAnalyzer.Configuration;

internal interface IRuleConfigurationSource
{
    RuleConfiguration GetConfiguration(RuleDescriptorInfo info);
}
