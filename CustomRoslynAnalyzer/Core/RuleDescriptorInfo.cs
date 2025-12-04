using Microsoft.CodeAnalysis;

namespace CustomRoslynAnalyzer.Core;

/// <summary>
/// Immutable metadata describing how a diagnostic rule presents itself to users.
/// </summary>
public sealed class RuleDescriptorInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RuleDescriptorInfo"/> class.
    /// </summary>
    public RuleDescriptorInfo(
        string id,
        string title,
        string messageFormat,
        string category,
        DiagnosticSeverity defaultSeverity,
        bool enabledByDefault,
        string description)
    {
        Id = id;
        Title = title;
        MessageFormat = messageFormat;
        Category = category;
        DefaultSeverity = defaultSeverity;
        EnabledByDefault = enabledByDefault;
        Description = description;
    }

    /// <summary>
    /// Gets the diagnostic identifier (e.g., CR0001).
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the short title displayed with the diagnostic.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the message format template for diagnostic instances.
    /// </summary>
    public string MessageFormat { get; }

    /// <summary>
    /// Gets the diagnostic category (Usage, Naming, etc.).
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Gets the default severity applied when the rule is enabled.
    /// </summary>
    public DiagnosticSeverity DefaultSeverity { get; }

    /// <summary>
    /// Gets a value indicating whether the rule is enabled by default.
    /// </summary>
    public bool EnabledByDefault { get; }

    /// <summary>
    /// Gets the detailed description shown in IDEs and documentation.
    /// </summary>
    public string Description { get; }
}
