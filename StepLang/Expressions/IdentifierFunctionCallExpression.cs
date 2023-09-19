using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public class IdentifierFunctionCallExpression : BaseFunctionCallExpression
{
    private readonly Token identifier;

    public IdentifierFunctionCallExpression(Token identifier, IReadOnlyList<Expression> args) : base(args)
    {
        this.identifier = identifier;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var functionVariable = interpreter.CurrentScope.GetVariable(identifier);
        var definition = functionVariable.Value.ExpectFunction().Value;

        return await ExecuteFunction(definition, interpreter, cancellationToken);
    }

    protected override string DebugDisplay() => $"{identifier}({string.Join(',', Args)})";

    protected override TokenLocation? GetCallLocation() => identifier.Location;
}