using System.Diagnostics.CodeAnalysis;
using StepLang.Framework.Conversion;
using StepLang.Framework.IO;
using StepLang.Framework.Reflection;
using StepLang.Parsing.Expressions;

namespace StepLang.Interpreting;

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
        SetVariable(PrintFunction.Identifier, ExpressionResult.Function(new PrintFunction()));
        SetVariable(PrintlnFunction.Identifier, ExpressionResult.Function(new PrintlnFunction()));
        SetVariable(ReadlineFunction.Identifier, ExpressionResult.Function(new ReadlineFunction()));
        SetVariable(TypenameFunction.Identifier, ExpressionResult.Function(new TypenameFunction()));
        SetVariable(ParseFunction.Identifier, ExpressionResult.Function(new ParseFunction()));
        SetVariable(JsonEncodeFunction.Identifier, ExpressionResult.Function(new JsonEncodeFunction()));
        SetVariable(JsonDecodeFunction.Identifier, ExpressionResult.Function(new JsonDecodeFunction()));

        SetVariable("null", ExpressionResult.Null);

        SetVariable("EOL", ExpressionResult.String(Environment.NewLine));
    }

    public void SetVariable(string identifier, ExpressionResult value)
    {
        // only look for variable in the current scope for assigning
        // this enables use to shadow variables from parent scopes
        if (identifiers.TryGetValue(identifier, out var variable))
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

    public override string ToString()
    {
        return $"Scope: {{{string.Join(", ", identifiers.Select(kvp => kvp.Value.ToString()))}}}";
    }
}