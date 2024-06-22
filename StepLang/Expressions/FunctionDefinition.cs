using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

/// <summary>
/// A generic function definition.
/// </summary>
public abstract class FunctionDefinition
{
    /// <summary>
    /// A string representation of the parameters of this function.
    /// </summary>
    [ExcludeFromCodeCoverage]
    private string DebugParamsString => string.Join(", ", Parameters.Select(p =>
    {
        if (p is NullableVariableDeclarationNode nullable)
            return $"{nullable.ResultTypesToString()}{nullable.NullabilityIndicator.Value} {nullable.Identifier.Value}";

        return $"{p.ResultTypesToString()} {p.Identifier.Value}";
    }));

    [ExcludeFromCodeCoverage]
    private string DebugReturnTypeString => string.Join("|", ReturnTypes.Select(r => r.ToString()));

    /// <summary>
    /// A string representation of the body of this function.
    /// </summary>
    [ExcludeFromCodeCoverage]
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

    /// <summary>
    /// Converts this function definition to a <see cref="FunctionResult"/>.
    /// </summary>
    /// <returns>The <see cref="FunctionResult"/> representation of this function definition.</returns>
    public FunctionResult ToResult() => new(this);

    /// <summary>
    /// Evaluates the function with the given arguments.
    /// </summary>
    /// <param name="callLocation">The location of the call.</param>
    /// <param name="interpreter">The interpreter to use.</param>
    /// <param name="arguments">The arguments to evaluate the function with.</param>
    /// <returns>The result of the function.</returns>
    public abstract ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments);

    /// <summary>
    /// The parameters accepted by this function.
    /// </summary>
    public abstract IReadOnlyCollection<IVariableDeclarationNode> Parameters { get; }

    /// <summary>
    /// The function's return types. By default, this is only <see cref="ResultType.Void"/>.
    /// </summary>
    protected virtual IEnumerable<ResultType> ReturnTypes { get; } = new[] { ResultType.Void };
}