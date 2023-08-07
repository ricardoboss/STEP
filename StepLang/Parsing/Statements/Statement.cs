using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public abstract class Statement
{
    public TokenLocation? Location { get; init; }

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
