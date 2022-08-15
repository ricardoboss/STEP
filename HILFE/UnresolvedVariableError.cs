namespace HILFE;

public class UnresolvedVariableError : Error
{
    public UnresolvedVariableError(string variable, CallFrame callFrame) : base($"Unresolved variable: {variable} in {callFrame.SourceFile}:{callFrame.SourceLine}", callFrame)
    {
    }
}