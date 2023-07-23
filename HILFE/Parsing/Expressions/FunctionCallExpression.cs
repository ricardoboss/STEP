using System.Runtime.CompilerServices;
using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Expressions;

public class FunctionCallExpression : Expression
{
    private readonly Token identifier;
    private readonly IReadOnlyList<Expression> args;

    public FunctionCallExpression(Token identifier, IReadOnlyList<Expression> args)
    {
        this.identifier = identifier;
        this.args = args;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        async IAsyncEnumerable<ExpressionResult> EvaluateArgs([EnumeratorCancellation] CancellationToken ct)
        {
            foreach (var arg in args)
                yield return await arg.EvaluateAsync(interpreter, ct);
        }

        var functionVariable = interpreter.CurrentScope.GetByIdentifier(identifier.Value);
        var functionDefinition = functionVariable.Value as string;
        switch (functionDefinition)
        {
            case "StdIn.ReadLine":
                if (interpreter.StdIn is not { } stdIn)
                    return new("null");

                var line = await stdIn.ReadLineAsync(cancellationToken);

                return new("string", line);

            case "StdOut.Write":
            case "StdOut.WriteLine":
                if (interpreter.StdOut is not { } stdOut)
                    return new("void", IsVoid: true);

                var stringArgs = await EvaluateArgs(cancellationToken).Select(r => r.Value?.ToString() ?? string.Empty).Cast<string>().ToListAsync(cancellationToken);

                if (functionDefinition == "StdOut.WriteLine")
                    await stdOut.WriteLineAsync(string.Join("", stringArgs));
                else
                    await stdOut.WriteAsync(string.Join("", stringArgs));

                return new("void", IsVoid: true);

            case "Framework.TypeName":
                var exp = args.Single();
                if (exp is not VariableExpression varExp)
                    throw new InterpreterException($"Invalid type, expected {nameof(VariableExpression)}, got {exp.GetType().Name}");

                var variable = interpreter.CurrentScope.GetByIdentifier(varExp.Identifier.Value);

                return new("string", variable.TypeName);

            default:
                throw new InterpreterException($"Undefined function: {functionDefinition}");
        }
    }

    protected override string DebugDisplay() => $"{identifier}({string.Join(',', args)})";
}