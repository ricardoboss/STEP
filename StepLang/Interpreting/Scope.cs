using System.Diagnostics.CodeAnalysis;
using StepLang.Framework.Conversion;
using StepLang.Framework.IO;
using StepLang.Framework.Reflection;
using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

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
        InitializeVariable(PrintFunction.Identifier, ExpressionResult.Function(new PrintFunction()));
        InitializeVariable(PrintlnFunction.Identifier, ExpressionResult.Function(new PrintlnFunction()));
        InitializeVariable(ReadlineFunction.Identifier, ExpressionResult.Function(new ReadlineFunction()));
        InitializeVariable(TypenameFunction.Identifier, ExpressionResult.Function(new TypenameFunction()));
        InitializeVariable(ParseFunction.Identifier, ExpressionResult.Function(new ParseFunction()));
        InitializeVariable(JsonEncodeFunction.Identifier, ExpressionResult.Function(new JsonEncodeFunction()));
        InitializeVariable(JsonDecodeFunction.Identifier, ExpressionResult.Function(new JsonDecodeFunction()));

        InitializeVariable("null", ExpressionResult.Null);

        InitializeVariable("EOL", ExpressionResult.String(Environment.NewLine));
    }

    public void InitializeVariable(string identifier, ExpressionResult value)
    {
        // only look for variable in the current scope for assigning
        // this enables use to shadow variables from parent scopes
        if (!identifiers.TryGetValue(identifier, out var variable))
        {
            variable = new(identifier, value);

            SetVariable(variable);
        }

        variable.Assign(value);
    }

    public void SetVariable(Variable variable)
    {
        identifiers[variable.Identifier] = variable;
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

    public Variable GetVariable(Token identifierToken)
    {
        if (TryGetVariable(identifierToken.Value, out var variable))
            return variable;

        throw new UndefinedIdentifierException(identifierToken);
    }

    public void UpdateValue(Token identifierToken, ExpressionResult value)
    {
        var variable = GetVariable(identifierToken);

        variable.Assign(value);
    }

    public void SetResult(ExpressionResult result) => InitializeVariable("$$RETURN", result);

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