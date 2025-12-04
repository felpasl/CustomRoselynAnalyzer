using Microsoft.CodeAnalysis;
using CustomRoslynAnalyzer.Core;

namespace CustomRoslynAnalyzer.Configuration;

/// <summary>
/// Represents the effective configuration for a single analyzer rule.
/// </summary>
public sealed class RuleConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RuleConfiguration"/> class.
    /// </summary>
    /// <param name="isEnabled">Whether the rule should be evaluated.</param>
    /// <param name="severity">The diagnostic severity to use when reporting.</param>
    public RuleConfiguration(bool isEnabled, DiagnosticSeverity severity)
    {
        IsEnabled = isEnabled;
        Severity = severity;
    }

    /// <summary>
    /// Gets a value indicating whether the rule is enabled.
    /// </summary>
    public bool IsEnabled { get; }

    /// <summary>
    /// Gets the severity at which diagnostics should be reported.
    /// </summary>
    public DiagnosticSeverity Severity { get; }

    /// <summary>
    /// Returns a copy of this configuration with a different enabled flag.
    /// </summary>
    public RuleConfiguration WithEnabled(bool isEnabled) =>
        new(isEnabled, Severity);

    /// <summary>
    /// Returns a copy of this configuration with a different severity.
    /// </summary>
    public RuleConfiguration WithSeverity(DiagnosticSeverity severity) =>
        new(IsEnabled, severity);

    /// <summary>
    /// Creates a configuration using the defaults defined on the descriptor.
    /// </summary>
    public static RuleConfiguration FromDefaults(RuleDescriptorInfo info) =>
        new(info.EnabledByDefault, info.DefaultSeverity);
}
