using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Conversion;

public class ToTypeNameFunction : NativeFunction
{
    public const string Identifier = "toTypeName";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (Enum.GetValues<ResultType>(), "value") };

    /// <inheritdoc />
    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count is not 1)
            throw new InvalidArgumentCountException(1, arguments.Count);

        var exp = arguments.Single();
        if (exp is not VariableExpression varExp)
            throw new InvalidExpressionTypeException(nameof(VariableExpression), exp.GetType().Name);

        var variable = interpreter.CurrentScope.GetVariable(varExp.Identifier);
        var result = new StringResult(variable.Value.ResultType.ToTypeName());

        return Task.FromResult<ExpressionResult>(result);
    }
}