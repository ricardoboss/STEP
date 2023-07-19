namespace HILFE.Interpreting;

public class Scope
{
    public static readonly Scope GlobalScope = new();

    private readonly Dictionary<string, TypedVariable> identifiers = new();

    public Scope(Scope parent)
    {
        ParentScope = parent;
    }

    private Scope()
    {
        ParentScope = null;

        // globally defined identifiers
        AddIdentifier("print", new("print", "function", "StdOut.Write"));
        AddIdentifier("readline", new("readline", "function", "StdIn.ReadLine"));
        AddIdentifier("clear", new("clear", "function", "StdOut.Clear"));
        AddIdentifier("typeName", new("typeName", "function", "Framework.TypeName"));

        AddIdentifier("null", new("null", "object", null));

        AddIdentifier("EOL", new("EOL", "string", Environment.NewLine));
    }

    public Scope? ParentScope { get; }

    public void AddIdentifier(string identifier, TypedVariable variable)
    {
        identifiers[identifier] = variable;
    }

    public bool HasIdentifier(string identifier) => identifiers.ContainsKey(identifier) || (ParentScope?.HasIdentifier(identifier) ?? false);

    public TypedVariable GetByIdentifier(string identifier)
    {
        if (identifiers.TryGetValue(identifier, out var variable))
        {
            return variable;
        }

        if (ParentScope != null)
        {
            return ParentScope.GetByIdentifier(identifier);
        }

        throw new UndefinedIdentifierException(identifier);
    }

    public void SetByIdentifier(string identifier, dynamic? value)
    {
        var variable = GetByIdentifier(identifier);

        variable.Assign(value);
    }
}