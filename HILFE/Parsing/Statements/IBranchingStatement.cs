using HILFE.Interpreting;

namespace HILFE.Parsing.Statements;

public interface IBranchingStatement
{
    Task<bool> ShouldBranch(Interpreter interpreter);
}