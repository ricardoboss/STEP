using System.Diagnostics.CodeAnalysis;
using HILFE.Interpreting;

namespace HILFE.Parsing.Statements;

public abstract class Statement
{
    protected Statement(StatementType type)
    {
        Type = type;
    }

    public StatementType Type { get; }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        var content = DebugRenderContent();
        if (content.Length > 0)
            content = $": {content}";

        return $"[{Type}{content}]";
    }

    [ExcludeFromCodeCoverage]
    protected virtual string DebugRenderContent() => "";

    public abstract Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default);
}
