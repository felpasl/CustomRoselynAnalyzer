using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using CustomRoslynAnalyzer.Configuration;
using CustomRoslynAnalyzer.Core;

namespace CustomRoslynAnalyzer.Rules;

public sealed class PublicAsyncSuffixRule : IAnalyzerRule
{
    private const string DiagnosticId = "CR0002";
    private const string Title = "Async method names should end with Async";
    private const string MessageFormat = "Rename '{0}' to end with Async to clarify asynchronous usage";
    private const string Category = "Naming";
    private const string Description =
        "Async methods should end with Async so consumers understand they run asynchronously.";

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

    public PublicAsyncSuffixRule(IRuleConfigurationSource configurationSource)
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

        context.RegisterSymbolAction(AnalyzeMethodSymbol, SymbolKind.Method);
    }

    private void AnalyzeMethodSymbol(SymbolAnalysisContext context)
    {
        if (context.Symbol is not IMethodSymbol methodSymbol)
        {
            return;
        }

        if (!methodSymbol.IsAsync &&
            !ReturnsTask(methodSymbol))
        {
            return;
        }

        if (methodSymbol.MethodKind is
            MethodKind.PropertyGet or
            MethodKind.PropertySet or
            MethodKind.EventAdd or
            MethodKind.EventRemove or
            MethodKind.EventRaise or
            MethodKind.Destructor or
            MethodKind.StaticConstructor or
            MethodKind.Constructor)
        {
            return;
        }

        if (methodSymbol.Name.EndsWith("Async", StringComparison.Ordinal))
        {
            return;
        }

        if (methodSymbol.DeclaredAccessibility != Accessibility.Public)
        {
            return;
        }

        var location = methodSymbol.Locations.FirstOrDefault();
        if (location != null)
        {
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, location, methodSymbol.Name));
        }
    }

    private static bool ReturnsTask(IMethodSymbol methodSymbol)
    {
        var returnType = methodSymbol.ReturnType;
        return returnType.Name is "Task" or "ValueTask";
    }
}
