using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Expressions;

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
        var functionVariable = interpreter.CurrentScope.GetByIdentifier(identifier.Value);
        if (functionVariable.TypeName != "function")
            throw new InterpreterException($"Cannot call non-function variable {identifier.Value} (type {functionVariable.TypeName}))");
        
        if (functionVariable.Value is not FunctionDefinition definition)
            throw new InterpreterException($"Function variable does not contain a {nameof(FunctionDefinition)}, instead it contains a {functionVariable.Value?.GetType().Name}");

        return await definition.EvaluateAsync(interpreter, args, cancellationToken);
    }

    protected override string DebugDisplay() => $"{identifier}({string.Join(',', args)})";
}