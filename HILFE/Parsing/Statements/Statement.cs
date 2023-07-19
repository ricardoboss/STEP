namespace HILFE.Parsing.Statements;

public abstract class Statement
{
    public Statement(StatementType type)
    {
        Type = type;
    }

    public readonly StatementType Type;

    /// <inheritdoc />
    public override string ToString()
    {
        var content = DebugRenderContent();
        if (content.Length > 0)
            content = $": {content}";

        return $"[{Type}{content}]";
    }

    protected virtual string DebugRenderContent() => "";
}
