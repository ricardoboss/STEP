namespace HILFE;

public class ScopeManager
{
    public Scope Push()
    {
        var scope = new Scope(CurrentScope);

        CurrentScope = scope;
        return CurrentScope;
    }

    public Scope Pop()
    {
        CurrentScope = CurrentScope.ParentScope ?? throw new InvalidScopeException("No parent scope is available but Pop was called");

        return CurrentScope;
    }

    public Scope CurrentScope { get; private set; } = Scope.GlobalScope;
}