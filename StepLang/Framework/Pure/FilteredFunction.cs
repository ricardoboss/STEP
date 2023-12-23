using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// A function that filters a list and returns a new list with only the elements that satisfy the given condition.
/// </summary>
public class FilteredFunction : ListManipulationFunction
{
    /// <summary>
    /// The identifier of the <see cref="FilteredFunction"/> function.
    /// </summary>
    public const string Identifier = "filtered";

    /// <inheritdoc />
    protected override IEnumerable<ExpressionResult> EvaluateListManipulation(TokenLocation callLocation, Interpreter interpreter, IEnumerable<ExpressionNode[]> arguments, FunctionDefinition callback)
    {
        return arguments.Where(args =>
        {
            var result = callback.Invoke(callLocation, interpreter, args);
            if (result is not BoolResult boolResult)
                throw new InvalidResultTypeException(callLocation, result, ResultType.Bool);

            return boolResult;
        }).Select(args => args[0].EvaluateUsing(interpreter));
    }
}