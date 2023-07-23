using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Expressions;

public class VariableExpression : Expression
{
    public readonly Token Identifier;

    public VariableExpression(Token identifier)
    {
        Identifier = identifier;
    }

    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var variable = interpreter.CurrentScope.GetByIdentifier(Identifier.Value);

        return Task.FromResult<ExpressionResult>(new(variable.TypeName, variable.Value));
    }

    protected override string DebugDisplay() => Identifier.ToString();
}