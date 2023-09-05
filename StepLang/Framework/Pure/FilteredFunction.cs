using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class FilteredFunction : ListManipulationFunction
{
    public const string Identifier = "filtered";

    protected override IAsyncEnumerable<ExpressionResult> EvaluateListManipulationAsync(Interpreter interpreter, IAsyncEnumerable<ConstantExpression[]> arguments, FunctionDefinition callback, CancellationToken cancellationToken = default)
    {
        return arguments
            .WhereAwait(async args =>
            {
                var result = await callback.EvaluateAsync(interpreter, args, cancellationToken);

                return result.ExpectBool().Value;
            })
            .Select(args => args[0].Result);
    }
}