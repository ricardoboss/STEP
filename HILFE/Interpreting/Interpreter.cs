using HILFE.Parsing.Statements;

namespace HILFE.Interpreting;

public class Interpreter
{
    public readonly TextWriter StdOut;
    public readonly TextWriter StdErr;
    public readonly TextReader StdIn;
    public int ExitCode = 0;

    private readonly Stack<Scope> scopes = new();

    public Interpreter(TextWriter stdOut, TextWriter stdErr, TextReader stdIn)
    {
        StdOut = stdOut;
        StdErr = stdErr;
        StdIn = stdIn;

        scopes.Push(Scope.GlobalScope);
    }

    public Scope CurrentScope => scopes.Peek();

    public void PushScope() => scopes.Push(new(CurrentScope));

    public void PopScope() => scopes.Pop();

    public async Task InterpretAsync(IAsyncEnumerable<BaseStatement> statements, CancellationToken cancellationToken = default)
    {
        await foreach (var statement in statements.WithCancellation(cancellationToken))
        {
            switch (statement)
            {
                case IExecutableStatement executableStatement:
                    await executableStatement.ExecuteAsync(this);
                    break;
                case ILoopingStatement loopingStatement:
                    // TODO: collect statements

                    await loopingStatement.InitializeLoop(this);
                    while (await loopingStatement.ShouldLoop(this))
                    {
                        await loopingStatement.ExecuteLoop(Array.Empty<BaseStatement>());
                    }

                    break;
                case IBranchingStatement branchingStatement:
                    await branchingStatement.ShouldBranch(this);
                    break;
            }
        }
    }
}