using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class ConvertedFunction : ListManipulationFunction
{
    public const string Identifier = "converted";

    protected override IEnumerable<ExpressionResult> EvaluateListManipulation(TokenLocation callLocation, Interpreter interpreter, IEnumerable<ExpressionNode[]> arguments, FunctionDefinition callback)
    {
        return arguments.Select(args => callback.Invoke(callLocation, interpreter, args));
    }
}