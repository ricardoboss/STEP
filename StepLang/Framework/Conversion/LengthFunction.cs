using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class LengthFunction : NativeFunction
{
    public const string Identifier = "length";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter,
        IReadOnlyList<Expression> arguments,
        CancellationToken cancellationToken = default)
    {
        if (arguments.Count is not 1)
            throw new InvalidArgumentCountException(1, arguments.Count);

        var exp = arguments.Single();

        int length;

        const string unexpectedTypeMessage = "First and only argument must be a string, list, or map";

        switch (exp)
        {
            case ConstantExpression conExp:
                switch (conExp.Result.ResultType)
                {
                    case ResultType.Str:
                        var constStr = conExp.Result.ExpectString();
                        length = constStr.Value.Length;

                        break;
                    case ResultType.List:
                        var constList = conExp.Result.ExpectList();
                        length = constList.Value.Count;

                        break;
                    case ResultType.Map:
                        var constMap = conExp.Result.ExpectMap();
                        length = constMap.Value.Count;

                        break;
                    default:
                        throw new ArgumentException(message: unexpectedTypeMessage);
                }

                break;
            case ListExpression listExp:
                var listExpressionResult = await listExp.EvaluateAsync(interpreter, cancellationToken);
                var list = listExpressionResult.ExpectList();
                length = list.Value.Count;

                break;
            case MapExpression mapExp:
                var mapExpressionResult = await mapExp.EvaluateAsync(interpreter, cancellationToken);
                var map = mapExpressionResult.ExpectMap();
                length = map.Value.Count;

                break;
            default:
                throw new ArgumentException(message: unexpectedTypeMessage);
        }

        var numberResult = new NumberResult(length);
        return await Task.FromResult<ExpressionResult>(numberResult);
    }

    protected override string DebugParamsString => "a string, list, or map";
}