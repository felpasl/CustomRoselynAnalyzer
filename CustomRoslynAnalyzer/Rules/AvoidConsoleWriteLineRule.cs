using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using CustomRoslynAnalyzer.Configuration;
using CustomRoslynAnalyzer.Core;

namespace CustomRoslynAnalyzer.Rules;

/// <summary>
/// Analyzer rule that discourages direct usage of <see cref="System.Console.WriteLine(string)"/> in favor of logging abstractions.
/// </summary>
public sealed class AvoidConsoleWriteLineRule : IAnalyzerRule
{
    private const string DiagnosticId = "CR0001";
    private const string Title = "Avoid Console.WriteLine";
    private const string MessageFormat = "Use a logging abstraction instead of Console.WriteLine";
    private const string Category = "Usage";
    private const string Description =
        "Console.WriteLine makes automated testing harder. Use ILogger or another abstraction instead.";

    private static readonly RuleDescriptorInfo Info = new(
        id: DiagnosticId,
        title: Title,
        messageFormat: MessageFormat,
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        enabledByDefault: true,
        description: Description);

    private static readonly DiagnosticDescriptor DefaultRuleDescriptor = new(
        id: DiagnosticId,
        title: Title,
        messageFormat: MessageFormat,
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    /// <summary>
    /// Gets the default descriptor used when no configuration overrides are provided.
    /// </summary>
    public static DiagnosticDescriptor DefaultDescriptor => DefaultRuleDescriptor;

    /// <summary>
    /// Gets the descriptor instance configured for the consuming compilation.
    /// </summary>
    public DiagnosticDescriptor Descriptor { get; }

    private readonly bool _isEnabled;

    /// <summary>
    /// Initializes the rule using the provided configuration source.
    /// </summary>
    public AvoidConsoleWriteLineRule(IRuleConfigurationSource configurationSource)
    {
        var configuration = configurationSource.GetConfiguration(Info);
        Descriptor = RuleDescriptorFactory.Create(Info, configuration);
        _isEnabled = configuration.IsEnabled;
    }

    /// <summary>
    /// Registers analyzer callbacks for invocation expressions.
    /// </summary>
    public void Register(CompilationStartAnalysisContext context)
    {
        if (!_isEnabled)
        {
            return;
        }

        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    private void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not InvocationExpressionSyntax invocation ||
            invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
        {
            return;
        }

        var symbol = context.SemanticModel.GetSymbolInfo(memberAccess).Symbol as IMethodSymbol;
        if (symbol is null)
        {
            return;
        }

        if (symbol.ContainingType?.ToDisplayString() == "System.Console" &&
            symbol.Name == "WriteLine")
        {
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, memberAccess.Name.GetLocation()));
        }
    }
}
