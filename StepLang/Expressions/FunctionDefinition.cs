using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Expressions;

public abstract class FunctionDefinition
{
    protected virtual string DebugParamsString => string.Join(", ", Parameters.Select(p =>
    {
        if (p is NullableVariableDeclarationNode nullable)
            return $"{string.Join("|", nullable.Types.Select(t => t.Value))}{nullable.NullabilityIndicator.Value} {nullable.Identifier.Value}";

        return $"{string.Join("|", p.Types.Select(t => t.Value))} {p.Identifier.Value}";
    }));

    protected virtual string DebugReturnTypeString => string.Join("|", ReturnTypes.Select(r => r.ToString()));

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

    // TODO: add async interface
    public abstract ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments);

    public abstract IReadOnlyCollection<IVariableDeclarationNode> Parameters { get; }

    public abstract IEnumerable<ResultType> ReturnTypes { get; }
}