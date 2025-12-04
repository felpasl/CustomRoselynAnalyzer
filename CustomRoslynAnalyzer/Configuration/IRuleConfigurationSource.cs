using CustomRoslynAnalyzer.Core;

namespace CustomRoslynAnalyzer.Configuration;

/// <summary>
/// Provides configuration settings for analyzer rules at runtime.
/// </summary>
public interface IRuleConfigurationSource
{
    /// <summary>
    /// Retrieves the configuration to apply for the specified rule descriptor.
    /// </summary>
    /// <param name="info">Descriptor metadata describing the analyzer rule.</param>
    /// <returns>The resolved configuration for the target rule.</returns>
    RuleConfiguration GetConfiguration(RuleDescriptorInfo info);
}
