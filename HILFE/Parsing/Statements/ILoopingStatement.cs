using HILFE.Interpreting;

namespace HILFE.Parsing.Statements;

public interface ILoopingStatement
{
    Task InitializeLoopAsync(Interpreter interpreter, CancellationToken cancellationToken = default);

    Task<bool> ShouldLoopAsync(Interpreter interpreter, CancellationToken cancellationToken = default);

    Task ExecuteLoopAsync(Interpreter interpreter, CancellationToken cancellationToken = default);

    Task FinalizeLoopAsync(Interpreter interpreter, CancellationToken cancellationToken = default);
}