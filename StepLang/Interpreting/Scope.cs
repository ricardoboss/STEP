using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using StepLang.Framework;
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
        InitBuiltInFunctions();

        SetVariable("null", NullResult.Instance);

        SetVariable("EOL", new StringResult(Environment.NewLine));
    }

    private void InitBuiltInFunctions()
    {
        var nativeFunctionType = typeof(NativeFunction);
        var identifierProp = nativeFunctionType.GetProperty("Identifier")!;
        var functionTypes = GetNativeFunctionTypes(nativeFunctionType);

        foreach (var functionType in functionTypes.Select(
                     ([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                                 DynamicallyAccessedMemberTypes.PublicProperties)]
                      ft) => new NativeFunctionTypeDescriptor {Type = ft}))
        {
            var function = GetNativeFunctionWithIdentifier(functionType, identifierProp, out var functionIdentifier);

            SetVariable(functionIdentifier, new FunctionResult(function));
        }
    }

    internal struct NativeFunctionTypeDescriptor
    {
        // https://github.com/dotnet/sdk/issues/27997#issuecomment-1260011790
        
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                    DynamicallyAccessedMemberTypes.PublicProperties)]
        public Type Type { get; init; }
    }
    
    private static NativeFunction GetNativeFunctionWithIdentifier(
        NativeFunctionTypeDescriptor functionType,
        PropertyInfo identifierProp,
        out string functionIdentifier)
    {
        var function = (NativeFunction)Activator.CreateInstance(functionType.Type)!;
        functionIdentifier = (string)identifierProp.GetValue(function)!;
        return function;
    }

    private static IEnumerable<Type> GetNativeFunctionTypes(Type nativeFunctionType)
    {
        return nativeFunctionType.Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(nativeFunctionType)
                        && !t.IsAbstract);
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