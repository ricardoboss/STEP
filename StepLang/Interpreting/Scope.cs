using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Framework.Mutating;
using StepLang.Framework.Other;
using StepLang.Framework.Pure;
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
        SetVariable(DoAddFunction.Identifier, new DoAddFunction().ToResult());
        SetVariable(DoRemoveFunction.Identifier, new DoRemoveFunction().ToResult());
        SetVariable(DoRemoveAtFunction.Identifier, new DoRemoveAtFunction().ToResult());
        SetVariable(DoPopFunction.Identifier, new DoPopFunction().ToResult());
        SetVariable(DoShiftFunction.Identifier, new DoShiftFunction().ToResult());
        SetVariable(DoInsertAtFunction.Identifier, new DoInsertAtFunction().ToResult());
        SetVariable(DoSwapFunction.Identifier, new DoSwapFunction().ToResult());

        // pure functions
        SetVariable(SubstringFunction.Identifier, new SubstringFunction().ToResult());
        SetVariable(IndexOfFunction.Identifier, new IndexOfFunction().ToResult());
        SetVariable(ContainsFunction.Identifier, new ContainsFunction().ToResult());
        SetVariable(StartsWithFunction.Identifier, new StartsWithFunction().ToResult());
        SetVariable(EndsWithFunction.Identifier, new EndsWithFunction().ToResult());
        SetVariable(PrintFunction.Identifier, new PrintFunction().ToResult());
        SetVariable(PrintlnFunction.Identifier, new PrintlnFunction().ToResult());
        SetVariable(ReadlineFunction.Identifier, new ReadlineFunction().ToResult());
        SetVariable(ReadFunction.Identifier, new ReadFunction().ToResult());
        SetVariable(AbsFunction.Identifier, new AbsFunction().ToResult());
        SetVariable(CeilFunction.Identifier, new CeilFunction().ToResult());
        SetVariable(FloorFunction.Identifier, new FloorFunction().ToResult());
        SetVariable(RoundFunction.Identifier, new RoundFunction().ToResult());
        SetVariable(ClampFunction.Identifier, new ClampFunction().ToResult());
        SetVariable(SinFunction.Identifier, new SinFunction().ToResult());
        SetVariable(CosFunction.Identifier, new CosFunction().ToResult());
        SetVariable(TanFunction.Identifier, new TanFunction().ToResult());
        SetVariable(InterpolateFunction.Identifier, new InterpolateFunction().ToResult());
        SetVariable(MaxFunction.Identifier, new MaxFunction().ToResult());
        SetVariable(MinFunction.Identifier, new MinFunction().ToResult());
        SetVariable(SqrtFunction.Identifier, new SqrtFunction().ToResult());
        SetVariable(ConvertedFunction.Identifier, new ConvertedFunction().ToResult());
        SetVariable(FilteredFunction.Identifier, new FilteredFunction().ToResult());
        SetVariable(ReversedFunction.Identifier, new ReversedFunction().ToResult());
        SetVariable(SortedFunction.Identifier, new SortedFunction().ToResult());
        SetVariable(CloneFunction.Identifier, new CloneFunction().ToResult());
        SetVariable(LengthFunction.Identifier, new LengthFunction().ToResult());

        // conversion functions
        SetVariable(ToJsonFunction.Identifier, new ToJsonFunction().ToResult());
        SetVariable(FromJsonFunction.Identifier, new FromJsonFunction().ToResult());
        SetVariable(ToTypeNameFunction.Identifier, new ToTypeNameFunction().ToResult());
        SetVariable(ToKeysFunction.Identifier, new ToKeysFunction().ToResult());
        SetVariable(ToValuesFunction.Identifier, new ToValuesFunction().ToResult());
        SetVariable(ToStringFunction.Identifier, new ToStringFunction().ToResult());
        SetVariable(ToRadixFunction.Identifier, new ToRadixFunction().ToResult());
        SetVariable(ToNumberFunction.Identifier, new ToNumberFunction().ToResult());
        SetVariable(ToBoolFunction.Identifier, new ToBoolFunction().ToResult());

        // other functions
        SetVariable(FetchFunction.Identifier, new FetchFunction().ToResult());
        SetVariable(FileExistsFunction.Identifier, new FileExistsFunction().ToResult());
        SetVariable(FileReadFunction.Identifier, new FileReadFunction().ToResult());
        SetVariable(FileWriteFunction.Identifier, new FileWriteFunction().ToResult());
        SetVariable(FileDeleteFunction.Identifier, new FileDeleteFunction().ToResult());
        SetVariable(SeedFunction.Identifier, new SeedFunction().ToResult());
        SetVariable(RandomFunction.Identifier, new RandomFunction().ToResult());

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