namespace HILFE;

public class Callstack : Stack<CallFrame>
{
    public TypedVariable ResolveVariable(string name)
    {
        var frame = this.FirstOrDefault(frame => frame.Variables.ContainsKey(name));
        return frame != null ?
            frame.Variables[name] :
            throw new UnresolvedVariableError(name, Peek());
    }
}