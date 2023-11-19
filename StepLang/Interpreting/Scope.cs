using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Framework.Mutating;
using StepLang.Framework.Other;
using StepLang.Framework.Pure;
using StepLang.Framework.Web;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

/// <summary>
/// Represents a scope in which variables can be declared and accessed.
/// Also, a scope can have a result.
/// </summary>
public class Scope
{
    /// <summary>
    /// The global scope containing all built-in functions and variables.
    /// This is used as the default scope.
    /// </summary>
    /// <remarks>
    /// This is the only scope with no parent.
    /// </remarks>
    public static readonly Scope GlobalScope = new();

    private readonly Dictionary<string, Variable> identifiers = new();
    private readonly Scope? parentScope;

    private TokenLocation? scopeResultLocation;
    private ExpressionResult? scopeResult;

    private bool shouldContinue;
    private bool shouldBreak;

    /// <summary>
    /// Creates a new <see cref="Scope"/> with the given parent scope.
    /// </summary>
    /// <param name="parent"></param>
    public Scope(Scope parent) => parentScope = parent;

    private Scope()
    {
        parentScope = null;

        // mutating functions
        CreateVariable(DoAddFunction.Identifier, new DoAddFunction().ToResult());
        CreateVariable(DoRemoveFunction.Identifier, new DoRemoveFunction().ToResult());
        CreateVariable(DoRemoveAtFunction.Identifier, new DoRemoveAtFunction().ToResult());
        CreateVariable(DoPopFunction.Identifier, new DoPopFunction().ToResult());
        CreateVariable(DoShiftFunction.Identifier, new DoShiftFunction().ToResult());
        CreateVariable(DoInsertAtFunction.Identifier, new DoInsertAtFunction().ToResult());
        CreateVariable(DoSwapFunction.Identifier, new DoSwapFunction().ToResult());

        // pure functions
        CreateVariable(SubstringFunction.Identifier, new SubstringFunction().ToResult());
        CreateVariable(IndexOfFunction.Identifier, new IndexOfFunction().ToResult());
        CreateVariable(ContainsFunction.Identifier, new ContainsFunction().ToResult());
        CreateVariable(ContainsKeyFunction.Identifier, new ContainsKeyFunction().ToResult());
        CreateVariable(StartsWithFunction.Identifier, new StartsWithFunction().ToResult());
        CreateVariable(EndsWithFunction.Identifier, new EndsWithFunction().ToResult());
        CreateVariable(PrintFunction.Identifier, new PrintFunction().ToResult());
        CreateVariable(PrintlnFunction.Identifier, new PrintlnFunction().ToResult());
        CreateVariable(ReadlineFunction.Identifier, new ReadlineFunction().ToResult());
        CreateVariable(ReadFunction.Identifier, new ReadFunction().ToResult());
        CreateVariable(AbsFunction.Identifier, new AbsFunction().ToResult());
        CreateVariable(CeilFunction.Identifier, new CeilFunction().ToResult());
        CreateVariable(FloorFunction.Identifier, new FloorFunction().ToResult());
        CreateVariable(RoundFunction.Identifier, new RoundFunction().ToResult());
        CreateVariable(ClampFunction.Identifier, new ClampFunction().ToResult());
        CreateVariable(SinFunction.Identifier, new SinFunction().ToResult());
        CreateVariable(CosFunction.Identifier, new CosFunction().ToResult());
        CreateVariable(TanFunction.Identifier, new TanFunction().ToResult());
        CreateVariable(InterpolateFunction.Identifier, new InterpolateFunction().ToResult());
        CreateVariable(MaxFunction.Identifier, new MaxFunction().ToResult());
        CreateVariable(MinFunction.Identifier, new MinFunction().ToResult());
        CreateVariable(SqrtFunction.Identifier, new SqrtFunction().ToResult());
        CreateVariable(ConvertedFunction.Identifier, new ConvertedFunction().ToResult());
        CreateVariable(FilteredFunction.Identifier, new FilteredFunction().ToResult());
        CreateVariable(ReversedFunction.Identifier, new ReversedFunction().ToResult());
        CreateVariable(SortedFunction.Identifier, new SortedFunction().ToResult());
        CreateVariable(CloneFunction.Identifier, new CloneFunction().ToResult());
        CreateVariable(LengthFunction.Identifier, new LengthFunction().ToResult());
        CreateVariable(CompareToFunction.Identifier, new CompareToFunction().ToResult());
        CreateVariable(SplitFunction.Identifier, new SplitFunction().ToResult());

        // conversion functions
        CreateVariable(ToJsonFunction.Identifier, new ToJsonFunction().ToResult());
        CreateVariable(FromJsonFunction.Identifier, new FromJsonFunction().ToResult());
        CreateVariable(ToTypeNameFunction.Identifier, new ToTypeNameFunction().ToResult());
        CreateVariable(ToKeysFunction.Identifier, new ToKeysFunction().ToResult());
        CreateVariable(ToValuesFunction.Identifier, new ToValuesFunction().ToResult());
        CreateVariable(ToStringFunction.Identifier, new ToStringFunction().ToResult());
        CreateVariable(ToRadixFunction.Identifier, new ToRadixFunction().ToResult());
        CreateVariable(ToNumberFunction.Identifier, new ToNumberFunction().ToResult());
        CreateVariable(ToBoolFunction.Identifier, new ToBoolFunction().ToResult());

        // other functions
        CreateVariable(FileExistsFunction.Identifier, new FileExistsFunction().ToResult());
        CreateVariable(FileReadFunction.Identifier, new FileReadFunction().ToResult());
        CreateVariable(FileWriteFunction.Identifier, new FileWriteFunction().ToResult());
        CreateVariable(FileDeleteFunction.Identifier, new FileDeleteFunction().ToResult());
        CreateVariable(SeedFunction.Identifier, new SeedFunction().ToResult());
        CreateVariable(RandomFunction.Identifier, new RandomFunction().ToResult());

        // web functions
        CreateVariable(FetchFunction.Identifier, new FetchFunction().ToResult());
        CreateVariable(HttpServerFunction.Identifier, new HttpServerFunction().ToResult());
        CreateVariable(FileResponseFunction.Identifier, new FileResponseFunction().ToResult());

        CreateVariable("null", NullResult.Instance);

        CreateVariable("EOL", new StringResult(Environment.NewLine));

        CreateVariable("pi", new NumberResult(Math.PI));
        CreateVariable("e", new NumberResult(Math.E));
    }

    /// <summary>
    /// Creates a new variable with the given identifier and initial value.
    /// </summary>
    /// <param name="identifier">The identifier of the variable.</param>
    /// <param name="initialValue">The initial value of the variable.</param>
    /// <param name="nullable">Whether the variable can be set to <see cref="NullResult"/>.</param>
    public void CreateVariable(string identifier, ExpressionResult initialValue, bool nullable = false) =>
        CreateVariable(new(), new(TokenType.Identifier, identifier), new[] { initialValue.ResultType }, initialValue, nullable);

    /// <summary>
    /// Creates a new variable with the given identifier token, type and initial value.
    /// </summary>
    /// <param name="assignmentLocation">The location of the assignment.</param>
    /// <param name="identifierToken">The identifier token of the variable.</param>
    /// <param name="types">The types of the variable.</param>
    /// <param name="initialValue">The initial value of the variable.</param>
    /// <param name="nullable">Whether the variable can be set to <see cref="NullResult"/>.</param>
    /// <exception cref="VariableAlreadyDeclaredException">Thrown if a variable with the same identifier already exists in this scope.</exception>
    public Variable CreateVariable(TokenLocation assignmentLocation, Token identifierToken, IReadOnlyList<ResultType> types, ExpressionResult initialValue, bool nullable = false)
    {
        if (Exists(identifierToken.Value, false))
            throw new VariableAlreadyDeclaredException(identifierToken);

        var variable = new Variable(identifierToken.Value, types, nullable);

        variable.Assign(assignmentLocation, initialValue);

        identifiers[identifierToken.Value] = variable;

        return variable;
    }

    /// <summary>
    /// Checks whether a variable with the given identifier exists in this scope and optionally in parent scopes.
    /// </summary>
    /// <param name="identifier">The identifier to look for.</param>
    /// <param name="includeParent">Whether to also look for the variable in parent scopes.</param>
    /// <returns><see langword="true"/> if a variable with the given identifier exists, <see langword="false"/> otherwise.</returns>
    public bool Exists(string identifier, bool includeParent)
    {
        if (includeParent)
            return TryGetVariable(identifier, out _);

        return identifiers.ContainsKey(identifier);
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

    /// <summary>
    /// Gets the variable with the given identifier.
    /// </summary>
    /// <param name="identifierToken">The identifier token of the variable.</param>
    /// <returns>The variable with the given identifier.</returns>
    /// <exception cref="UndefinedIdentifierException">Thrown if no variable with the given identifier exists in this or in parent scopes.</exception>
    public Variable GetVariable(Token identifierToken)
    {
        if (TryGetVariable(identifierToken.Value, out var variable))
            return variable;

        throw new UndefinedIdentifierException(identifierToken);
    }

    /// <summary>
    /// Sets the result of this scope.
    /// </summary>
    /// <param name="location">The location of the result setting statement.</param>
    /// <param name="result">The result to set.</param>
    public void SetResult(TokenLocation location, ExpressionResult result)
    {
        scopeResultLocation = location;
        scopeResult = result;
    }

    public bool ShouldReturn() => scopeResult is not null;

    /// <summary>
    /// Tries to get the result of this scope.
    /// </summary>
    /// <param name="result">The result of this scope.</param>
    /// <param name="location">The location where the result was set.</param>
    /// <returns><see langword="true"/> if this scope has a result, <see langword="false"/> otherwise.</returns>
    public bool TryGetResult([NotNullWhen(true)] out ExpressionResult? result, [NotNullWhen(true)] out TokenLocation? location)
    {
        location = scopeResultLocation;
        result = scopeResult;

        return result is not null;
    }

    public void SetContinue() => shouldContinue = true;

    public void SetBreak() => shouldBreak = true;

    public bool ShouldContinue() => shouldContinue;

    public bool ShouldBreak() => shouldBreak;

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Scope: {{{string.Join(", ", identifiers.Select(kvp => kvp.Value.ToString()))}}}";
    }
}