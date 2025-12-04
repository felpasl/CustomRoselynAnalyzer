using CustomRoslynAnalyzer.Core;

namespace CustomRoslynAnalyzer.Configuration;

public interface IRuleConfigurationSource
{
    RuleConfiguration GetConfiguration(RuleDescriptorInfo info);
}
