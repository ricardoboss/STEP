using System.Diagnostics.CodeAnalysis;
using HILFE.Framework.Conversion;
using HILFE.Framework.IO;
using HILFE.Framework.Reflection;
using HILFE.Parsing.Expressions;

namespace HILFE.Interpreting;

public class Scope
{
    public static readonly Scope GlobalScope = new();

    private readonly Dictionary<string, Variable> identifiers = new();
    private readonly Scope? parentScope;

    public Scope(Scope parent) => parentScope = parent;

    private Scope()
    {
        parentScope = null;

        // globally defined identifiers
        SetVariable(PrintFunction.Identifier, new("function", new PrintFunction()));
        SetVariable(PrintlnFunction.Identifier, new("function", new PrintlnFunction()));
        SetVariable(ReadlineFunction.Identifier, new("function", new ReadlineFunction()));
        SetVariable(TypenameFunction.Identifier, new("function", new TypenameFunction()));
        SetVariable(ParseFunction.Identifier, new("function", new ParseFunction()));

        SetVariable("null", ExpressionResult.Null);

        SetVariable("EOL", new("string", Environment.NewLine));
    }

    public void SetVariable(string identifier, ExpressionResult value)
    {
        if (TryGetVariable(identifier, out var variable))
        {
            variable.Assign(value);
        }
        else
        {
            identifiers[identifier] = new(identifier, value);
        }
    }

    private bool TryGetVariable(string identifier, [NotNullWhen(true)] out Variable? variable)
    {
        if (identifiers.TryGetValue(identifier, out variable))
            return true;

        if (parentScope != null)
            return parentScope.TryGetVariable(identifier, out variable);

        variable = null;
        return false;
    }

    public Variable GetVariable(string identifier)
    {
        if (TryGetVariable(identifier, out var variable))
            return variable;

        throw new UndefinedIdentifierException(identifier);
    }

    public void UpdateValue(string identifier, ExpressionResult value)
    {
        var variable = GetVariable(identifier);

        variable.Assign(value);
    }

    public void SetResult(ExpressionResult result) => SetVariable("$$RETURN", result);

    public bool TryGetResult([NotNullWhen(true)] out ExpressionResult? result)
    {
        result = null;
        if (!TryGetVariable("$$RETURN", out var resultVar))
            return false;

        result = resultVar.Value;
        return true;
    }
}