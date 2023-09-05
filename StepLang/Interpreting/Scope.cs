using System.Diagnostics.CodeAnalysis;
using StepLang.Framework.Conversion;
using StepLang.Framework.Mutating;
using StepLang.Framework.Other;
using StepLang.Framework.Pure;
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

        // mutating functions
        SetVariable(DoAddFunction.Identifier, new FunctionResult(new DoAddFunction()));
        SetVariable(DoRemoveFunction.Identifier, new FunctionResult(new DoRemoveFunction()));
        SetVariable(DoRemoveAtFunction.Identifier, new FunctionResult(new DoRemoveAtFunction()));
        SetVariable(DoPopFunction.Identifier, new FunctionResult(new DoPopFunction()));
        SetVariable(DoShiftFunction.Identifier, new FunctionResult(new DoShiftFunction()));
        SetVariable(DoInsertAtFunction.Identifier, new FunctionResult(new DoInsertAtFunction()));
        SetVariable(DoSwapFunction.Identifier, new FunctionResult(new DoSwapFunction()));

        // pure functions
        SetVariable(SubstringFunction.Identifier, new FunctionResult(new SubstringFunction()));
        SetVariable(IndexOfFunction.Identifier, new FunctionResult(new IndexOfFunction()));
        SetVariable(ContainsFunction.Identifier, new FunctionResult(new ContainsFunction()));
        SetVariable(StartsWithFunction.Identifier, new FunctionResult(new StartsWithFunction()));
        SetVariable(EndsWithFunction.Identifier, new FunctionResult(new EndsWithFunction()));
        SetVariable(PrintFunction.Identifier, new FunctionResult(new PrintFunction()));
        SetVariable(PrintlnFunction.Identifier, new FunctionResult(new PrintlnFunction()));
        SetVariable(ReadlineFunction.Identifier, new FunctionResult(new ReadlineFunction()));
        SetVariable(ReadFunction.Identifier, new FunctionResult(new ReadFunction()));
        SetVariable(AbsFunction.Identifier, new FunctionResult(new AbsFunction()));
        SetVariable(CeilFunction.Identifier, new FunctionResult(new CeilFunction()));
        SetVariable(FloorFunction.Identifier, new FunctionResult(new FloorFunction()));
        SetVariable(RoundFunction.Identifier, new FunctionResult(new RoundFunction()));
        SetVariable(ClampFunction.Identifier, new FunctionResult(new ClampFunction()));
        SetVariable(SinFunction.Identifier, new FunctionResult(new SinFunction()));
        SetVariable(CosFunction.Identifier, new FunctionResult(new CosFunction()));
        SetVariable(TanFunction.Identifier, new FunctionResult(new TanFunction()));
        SetVariable(InterpolateFunction.Identifier, new FunctionResult(new InterpolateFunction()));
        SetVariable(MaxFunction.Identifier, new FunctionResult(new MaxFunction()));
        SetVariable(MinFunction.Identifier, new FunctionResult(new MinFunction()));
        SetVariable(SqrtFunction.Identifier, new FunctionResult(new SqrtFunction()));
        SetVariable(ConvertedFunction.Identifier, new FunctionResult(new ConvertedFunction()));
        SetVariable(FilteredFunction.Identifier, new FunctionResult(new FilteredFunction()));
        SetVariable(ReversedFunction.Identifier, new FunctionResult(new ReversedFunction()));

        // conversion functions
        SetVariable(ToJsonFunction.Identifier, new FunctionResult(new ToJsonFunction()));
        SetVariable(FromJsonFunction.Identifier, new FunctionResult(new FromJsonFunction()));
        SetVariable(ToTypeNameFunction.Identifier, new FunctionResult(new ToTypeNameFunction()));
        SetVariable(ToKeysFunction.Identifier, new FunctionResult(new ToKeysFunction()));
        SetVariable(ToValuesFunction.Identifier, new FunctionResult(new ToValuesFunction()));
        SetVariable(ToStringFunction.Identifier, new FunctionResult(new ToStringFunction()));
        SetVariable(ToRadixFunction.Identifier, new FunctionResult(new ToRadixFunction()));
        SetVariable(ToNumberFunction.Identifier, new FunctionResult(new ToNumberFunction()));
        SetVariable(ToBoolFunction.Identifier, new FunctionResult(new ToBoolFunction()));

        // other functions
        SetVariable(FileExistsFunction.Identifier, new FunctionResult(new FileExistsFunction()));
        SetVariable(FileReadFunction.Identifier, new FunctionResult(new FileReadFunction()));
        SetVariable(FileWriteFunction.Identifier, new FunctionResult(new FileWriteFunction()));
        SetVariable(FileDeleteFunction.Identifier, new FunctionResult(new FileDeleteFunction()));
        SetVariable(SeedFunction.Identifier, new FunctionResult(new SeedFunction()));
        SetVariable(RandomFunction.Identifier, new FunctionResult(new RandomFunction()));

        SetVariable("null", NullResult.Instance);

        SetVariable("EOL", new StringResult(Environment.NewLine));

        SetVariable("pi", new NumberResult(Math.PI));
        SetVariable("e", new NumberResult(Math.E));
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

    internal bool TryGetVariable(string identifier, [NotNullWhen(true)] out Variable? variable)
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