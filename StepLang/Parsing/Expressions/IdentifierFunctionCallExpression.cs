using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

public class IdentifierFunctionCallExpression : Expression
{
    private readonly Token identifier;
    private readonly IReadOnlyList<Expression> args;

    public IdentifierFunctionCallExpression(Token identifier, IReadOnlyList<Expression> args)
    {
        this.identifier = identifier;
        this.args = args;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var functionVariable = interpreter.CurrentScope.GetVariable(identifier);
        var definition = functionVariable.Value.ExpectFunction();
        return await definition.EvaluateAsync(interpreter, args, cancellationToken);
    }

    protected override string DebugDisplay() => $"{identifier}({string.Join(',', args)})";
}