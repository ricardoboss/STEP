using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public class VariableExpression : Expression
{
    public Token Identifier { get; }

    public VariableExpression(Token identifier)
    {
        Identifier = identifier;
    }

    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var variable = interpreter.CurrentScope.GetVariable(Identifier);

        return Task.FromResult(variable.Value);
    }

    protected override string DebugDisplay() => Identifier.ToString();
}