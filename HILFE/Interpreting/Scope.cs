using System.Diagnostics.CodeAnalysis;
using HILFE.Framework.Functions;
using HILFE.Parsing.Expressions;

namespace HILFE.Interpreting;

public class Scope
{
    public static readonly Scope GlobalScope = new();

    private readonly Dictionary<string, TypedVariable> identifiers = new();
    private readonly Scope? parentScope;

    public Scope(Scope parent) => parentScope = parent;

    private Scope()
    {
        parentScope = null;

        // globally defined identifiers
        SetVariable(new("print", "function", new PrintFunction()));
        SetVariable(new("println", "function", new PrintlnFunction()));
        SetVariable(new("readline", "function", new ReadlineFunction()));
        SetVariable(new("typename", "function", new TypenameFunction()));
        SetVariable(new("parse", "function", new ParseFunction()));

        SetVariable(new("null", "null", null));

        SetVariable(new("EOL", "string", Environment.NewLine));
    }

    public void SetVariable(TypedVariable variable)
    {
        identifiers[variable.Identifier] = variable;
    }

    private bool TryGetByIdentifier(string identifier, [NotNullWhen(true)] out TypedVariable? variable)
    {
        if (identifiers.TryGetValue(identifier, out variable))
            return true;

        if (parentScope != null)
            return parentScope.TryGetByIdentifier(identifier, out variable);

        variable = null;
        return false;
    }

    public TypedVariable GetByIdentifier(string identifier)
    {
        if (TryGetByIdentifier(identifier, out var variable))
            return variable;

        throw new UndefinedIdentifierException(identifier);
    }

    public void SetByIdentifier(string identifier, dynamic? value)
    {
        var variable = GetByIdentifier(identifier);

        variable.Assign(value);
    }

    public void SetResult(ExpressionResult result) => SetVariable(new("$$RETURN", result.ValueType, result.Value));

    public bool TryGetResult([NotNullWhen(true)] out ExpressionResult? result)
    {
        result = null;
        if (!TryGetByIdentifier("$$RETURN", out var resultVar))
            return false;

        result = new(resultVar.TypeName, resultVar.Value, IsVoid: resultVar.TypeName == "void");
        return true;
    }
}