using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework.Functions;

public class ReadlineFunction : FunctionDefinition
{
    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count is not 0)
            throw new InterpreterException("readline takes no arguments");
        
        if (interpreter.StdIn is not { } stdIn)
            return new("null");

        var line = await stdIn.ReadLineAsync(cancellationToken);

        return new("string", line);
    }

    /// <inheritdoc />
    protected override string DebugBodyString => "[native code]";
}