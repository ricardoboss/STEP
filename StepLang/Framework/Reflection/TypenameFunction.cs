using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Reflection;

public class TypenameFunction : NativeFunction
{
    public override string Identifier { get; } = "typename";

    /// <inheritdoc />
    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
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

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => "any value";
}