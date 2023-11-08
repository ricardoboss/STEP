using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class ConvertedFunction : ListManipulationFunction
{
    public const string Identifier = "converted";

    protected override IEnumerable<ExpressionResult> EvaluateListManipulation(Interpreter interpreter, IEnumerable<ExpressionNode[]> arguments, FunctionDefinition callback)
    {
        return arguments.Select(args => callback.Invoke(interpreter, args));
    }
}