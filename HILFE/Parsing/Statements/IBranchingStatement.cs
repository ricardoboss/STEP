using HILFE.Interpreting;

namespace HILFE.Parsing.Statements;

public interface IBranchingStatement
{
    Task<bool> ShouldBranch(Interpreter interpreter);

    Task ExecuteTrueBranch(Interpreter interpreter, CancellationToken cancellationToken = default);

    Task ExecuteFalseBranch(Interpreter interpreter, CancellationToken cancellationToken = default);
}