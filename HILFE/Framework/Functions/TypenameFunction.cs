using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework.Functions;

public class TypenameFunction : FunctionDefinition
{
    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        var exp = arguments.Single();
        if (exp is not VariableExpression varExp)
            throw new InterpreterException($"Invalid type, expected {nameof(VariableExpression)}, got {exp.GetType().Name}");

        var variable = interpreter.CurrentScope.GetByIdentifier(varExp.Identifier.Value);

        return new("string", variable.TypeName);
    }

    /// <inheritdoc />
    protected override string DebugBodyString => "[native code]";

    /// <inheritdoc />
    protected override string DebugParamsString => "object variable";
}