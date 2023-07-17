using HILFE.Parsing.Statements;

namespace HILFE.Interpreting;

public class Interpreter
{
    public readonly ScopeManager Scope;
    public readonly TextWriter StdOut;
    public readonly TextWriter StdErr;
    public readonly TextReader StdIn;
    public int ExitCode = 0;

    public Interpreter(TextWriter stdOut, TextWriter stdErr, TextReader stdIn)
    {
        Scope = new();
        StdOut = stdOut;
        StdErr = stdErr;
        StdIn = stdIn;
    }

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