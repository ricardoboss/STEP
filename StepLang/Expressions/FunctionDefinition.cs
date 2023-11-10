using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Expressions;

public abstract class FunctionDefinition
{
    private string DebugParamsString => string.Join(", ", Parameters.Select(p =>
    {
        if (p is NullableVariableDeclarationNode nullable)
            return $"{nullable.ResultTypesToString()}{nullable.NullabilityIndicator.Value} {nullable.Identifier.Value}";

        return $"{p.ResultTypesToString()} {p.Identifier.Value}";
    }));

    private string DebugReturnTypeString => string.Join("|", ReturnTypes.Select(r => r.ToString()));

    protected abstract string DebugBodyString { get; }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        var paramStr = DebugParamsString;
        var returnStr = DebugReturnTypeString;
        var bodyStr = DebugBodyString;

        return $"<Function: ({paramStr}): {returnStr} => {{ {bodyStr} }}>";
    }

    public FunctionResult ToResult() => new(this);

    public abstract ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments);

    public abstract IReadOnlyCollection<IVariableDeclarationNode> Parameters { get; }

    protected virtual IEnumerable<ResultType> ReturnTypes { get; } = new[] { ResultType.Void };
}