using HILFE.Interpreting;

namespace HILFE.Parsing.Statements;

public interface IExecutableStatement
{
    Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default);
}