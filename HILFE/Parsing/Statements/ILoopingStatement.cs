using HILFE.Interpreting;

namespace HILFE.Parsing.Statements;

public interface ILoopingStatement
{
    Task InitializeLoop(Interpreter interpreter);

    Task<bool> ShouldLoop(Interpreter interpreter);

    Task<bool> ExecuteLoop(IReadOnlyList<BaseStatement> statements);
}