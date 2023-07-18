using HILFE.Interpreting;

namespace HILFE.Parsing.Statements;

public interface ILoopingStatement
{
    Task InitializeLoop(Interpreter interpreter);

    Task<bool> ShouldLoop(Interpreter interpreter);

    Task ExecuteLoop(Interpreter interpreter, CancellationToken cancellationToken = default);
}