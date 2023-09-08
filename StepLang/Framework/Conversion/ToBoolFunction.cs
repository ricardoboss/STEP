using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class ToBoolFunction : NativeFunction
{
    public const string Identifier = "toBool";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (Enum.GetValues<ResultType>(), Identifier) };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var value = await arguments.Single().EvaluateAsync(interpreter, cancellationToken);

        var result = value switch
        {
            StringResult stringResult when string.IsNullOrWhiteSpace(stringResult.Value) => false,
            StringResult stringResult => stringResult.Value.ToUpperInvariant() switch
            {
                "TRUE" => true,
                "1" => true,
                _ => false,
            },
            NumberResult numberResult => numberResult.Value > 0,
            BoolResult boolResult => boolResult.Value,
            ListResult listResult => listResult.Value.Count > 0,
            MapResult mapResult => mapResult.Value.Count > 0,
            FunctionResult => true,
            NullResult => false,
            VoidResult => false,
            _ => throw new NotImplementedException(),
        };

        return new BoolResult(result);
    }
}