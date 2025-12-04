using CustomRoslynAnalyzer.Core;

namespace CustomRoslynAnalyzer.Configuration;

internal interface IRuleConfigurationSource
{
    RuleConfiguration GetConfiguration(RuleDescriptorInfo info);
}
