using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.IO;

public class LengthFunction : NativeFunction
{
    public const string Identifier = "length";

    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments,
        CancellationToken cancellationToken = default)
    {
        if (arguments.Count is not 1)
            throw new InvalidArgumentCountException(1, arguments.Count);

        var exp = arguments.Single();
        if (exp is not ConstantExpression conExp)
        {
            throw new InvalidExpressionTypeException(nameof(ConstantExpression), exp.GetType().Name);
        }

        var str = conExp.Result.ExpectString();

        var expressionResult = new NumberResult(str.Value.Length);
        return Task.FromResult<ExpressionResult>(expressionResult);
    }

    protected override string DebugParamsString => "a string, list, or map";
}