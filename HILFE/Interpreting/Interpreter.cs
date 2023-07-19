using HILFE.Parsing.Statements;

namespace HILFE.Interpreting;

public class Interpreter
{
    public readonly TextWriter StdOut;
    public readonly TextWriter StdErr;
    public readonly TextReader StdIn;
    public readonly TextWriter? DebugOut;
    public int ExitCode = 0;

    private readonly Stack<Scope> scopes = new();

    public Interpreter(TextWriter stdOut, TextWriter stdErr, TextReader stdIn, TextWriter? debugOut = null)
    {
        StdOut = stdOut;
        StdErr = stdErr;
        StdIn = stdIn;
        DebugOut = debugOut;

        scopes.Push(Scope.GlobalScope);
    }

    public Scope CurrentScope => scopes.Peek();

    public void PushScope() => scopes.Push(new(CurrentScope));

    public void PopScope() => scopes.Pop();

    public async Task InterpretAsync(IAsyncEnumerable<Statement> statements, CancellationToken cancellationToken = default)
    {
        await foreach (var statement in statements.WithCancellation(cancellationToken))
        {
            if (DebugOut is not null)
                await DebugOut.WriteLineAsync(statement.ToString());

            await InterpretStatement(statement, cancellationToken);
        }
    }

    private async Task InterpretStatement(Statement statement, CancellationToken cancellationToken)
    {
        switch (statement)
        {
            case IExecutableStatement executableStatement:
                await executableStatement.ExecuteAsync(this, cancellationToken);
                break;
            case ILoopingStatement loopingStatement:
                await InterpretLoopingStatement(loopingStatement, cancellationToken);
                break;
            case IBranchingStatement branchingStatement:
                await InterpretBranchingStatement(branchingStatement, cancellationToken);
                break;
            default:
                throw new NotImplementedException($"Given statement cannot be interpreted: {statement}");
        }
    }

    private async Task InterpretLoopingStatement(ILoopingStatement statement, CancellationToken cancellationToken)
    {
        await statement.InitializeLoopAsync(this, cancellationToken);

        while (await statement.ShouldLoopAsync(this, cancellationToken))
            await statement.ExecuteLoopAsync(this, cancellationToken);

        await statement.FinalizeLoopAsync(this, cancellationToken);
    }

    private async Task InterpretBranchingStatement(IBranchingStatement statement, CancellationToken cancellationToken)
    {
        if (await statement.ShouldBranch(this))
            await statement.ExecuteTrueBranch(this, cancellationToken);
        else
            await statement.ExecuteFalseBranch(this, cancellationToken);
    }
}