using System.Diagnostics.CodeAnalysis;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework.Reflection;

public class TypenameFunction : NativeFunction
{
    public const string Identifier = "typename";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count is not 1)
            throw new InvalidArgumentCountException(1, arguments.Count);

        var exp = arguments.Single();
        if (exp is not VariableExpression varExp)
            throw new InvalidExpressionTypeException(nameof(VariableExpression), exp.GetType().Name);

        var variable = interpreter.CurrentScope.GetVariable(varExp.Identifier.Value);

        return ExpressionResult.String(variable.Value.ValueType);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugParamsString => "any value";
}