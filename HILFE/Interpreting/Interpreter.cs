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

            switch (statement)
            {
                case IExecutableStatement executableStatement:
                    await executableStatement.ExecuteAsync(this);
                    break;
                case ILoopingStatement loopingStatement:
                    await loopingStatement.InitializeLoop(this);
                    while (await loopingStatement.ShouldLoop(this))
                    {
                        await loopingStatement.ExecuteLoop(this, cancellationToken);
                    }

                    break;
                case IBranchingStatement branchingStatement:
                    if (await branchingStatement.ShouldBranch(this))
                    {
                        await branchingStatement.ExecuteTrueBranch(this, cancellationToken);
                    }
                    else
                    {
                        await branchingStatement.ExecuteFalseBranch(this, cancellationToken);
                    }

                    break;
            }
        }
    }
}