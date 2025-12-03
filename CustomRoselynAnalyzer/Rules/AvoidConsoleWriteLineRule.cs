using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using CustomRoselynAnalyzer.Configuration;
using CustomRoselynAnalyzer.Core;

namespace CustomRoselynAnalyzer.Rules;

internal sealed class AvoidConsoleWriteLineRule : IAnalyzerRule
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

    public static DiagnosticDescriptor DefaultDescriptor => DefaultRuleDescriptor;

    public DiagnosticDescriptor Descriptor { get; }

    private readonly bool _isEnabled;

    public AvoidConsoleWriteLineRule(IRuleConfigurationSource configurationSource)
    {
        var configuration = configurationSource.GetConfiguration(Info);
        Descriptor = RuleDescriptorFactory.Create(Info, configuration);
        _isEnabled = configuration.IsEnabled;
    }

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
