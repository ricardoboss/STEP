using StepLang.Parsing.Statements;

namespace StepLang.Interpreting;

public class Interpreter
{
    public TextWriter? StdOut { get; }
    public TextWriter? StdErr { get; }
    public TextReader? StdIn { get; }
    public TextWriter? DebugOut { get; }
    public int ExitCode { get; set; }

    public int BreakDepth { get; set; }

    public int ContinueDepth { get; set; }

    private readonly Stack<Scope> scopes = new();

    public Interpreter(TextWriter? stdOut = null, TextWriter? stdErr = null, TextReader? stdIn = null,
        TextWriter? debugOut = null)
    {
        StdOut = stdOut;
        StdErr = stdErr;
        StdIn = stdIn;
        DebugOut = debugOut;

        PushScope(Scope.GlobalScope);
    }

    public Scope CurrentScope => scopes.Peek();

    public Scope PushScope(Scope? parent = null)
    {
        var newScope = new Scope(parent ?? CurrentScope);

        scopes.Push(newScope);

        DebugOut?.WriteLine($"Pushed new scope (new depth: {scopes.Count - 1})");

        return newScope;
    }

    public Scope PopScope()
    {
        DebugOut?.WriteLine($"Popping scope (new depth: {scopes.Count - 2})");

        return scopes.Pop();
    }

    public async Task InterpretAsync(IAsyncEnumerable<Statement> statements,
        CancellationToken cancellationToken = default)
    {
        await foreach (var statement in statements.WithCancellation(cancellationToken))
        {
            if (DebugOut is not null)
                await DebugOut.WriteLineAsync(statement.ToString());

            if (ContinueDepth > 0)
            {
                ContinueDepth--;

                break;
            }

            await statement.ExecuteAsync(this, cancellationToken);

            if (CurrentScope.TryGetResult(out _))
                break;
        }
    }
}