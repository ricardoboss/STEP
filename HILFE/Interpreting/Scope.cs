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

        AddIdentifier("0", new("0", "int", 0));
        AddIdentifier("1", new("1", "int", 1));
        AddIdentifier("2", new("2", "int", 2));
        AddIdentifier("3", new("3", "int", 3));
        AddIdentifier("4", new("4", "int", 4));
        AddIdentifier("5", new("5", "int", 5));
        AddIdentifier("6", new("6", "int", 6));
        AddIdentifier("7", new("7", "int", 7));
        AddIdentifier("8", new("8", "int", 8));
        AddIdentifier("9", new("9", "int", 9));

        AddIdentifier("true", new("true", "bool", true));
        AddIdentifier("false", new("false", "bool", false));

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