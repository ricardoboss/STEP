using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Converts the list of values to new values using the callback.
/// </summary>
public class ConvertedFunction : ListManipulationFunction
{
    /// <summary>
    /// The identifier of the <see cref="ConvertedFunction"/> function.
    /// </summary>
    public const string Identifier = "converted";

    /// <inheritdoc />
    protected override IEnumerable<ExpressionResult> EvaluateListManipulation(TokenLocation callLocation, Interpreter interpreter, IEnumerable<ExpressionNode[]> arguments, FunctionDefinition callback)
    {
        return arguments.Select(args => callback.Invoke(callLocation, interpreter, args));
    }
}