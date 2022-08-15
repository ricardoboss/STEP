namespace HILFE;

public class Error : Exception
{
    public readonly CallFrame CallFrame;

    protected Error(string message, CallFrame callFrame) : base(message)
    {
        CallFrame = callFrame;
    }
}