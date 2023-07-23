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
        SetVariable(new("print", "function", "StdOut.Write"));
        SetVariable(new("println", "function", "StdOut.WriteLine"));
        SetVariable(new("readline", "function", "StdIn.ReadLine"));
        SetVariable(new("clear", "function", "StdOut.Clear"));
        SetVariable(new("typeName", "function", "Framework.TypeName"));

        SetVariable(new("null", "null", null));

        SetVariable(new("EOL", "string", Environment.NewLine));
    }

    public Scope? ParentScope { get; }

    public void SetVariable(TypedVariable variable)
    {
        identifiers[variable.Identifier] = variable;
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