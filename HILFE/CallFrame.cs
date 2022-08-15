namespace HILFE;

public class CallFrame
{
    public readonly Dictionary<string, TypedVariable> Variables = new();
    public readonly CallFrame? Parent;
    public readonly string SourceFile;
    public readonly int SourceLine;
    public readonly int SourceColumn;

    public CallFrame(CallFrame? parent, string sourceFile, int sourceLine, int sourceColumn)
    {
        Parent = parent;
        SourceFile = sourceFile;
        SourceLine = sourceLine;
        SourceColumn = sourceColumn;
    }
}