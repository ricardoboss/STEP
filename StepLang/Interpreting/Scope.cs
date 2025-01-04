using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Framework.Mutating;
using StepLang.Framework.Other;
using StepLang.Framework.Pure;
using StepLang.Framework.Web;
using StepLang.Tokenizing;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Interpreting;

public class Scope
{
	public static readonly Scope GlobalScope = new();

	private readonly Dictionary<string, Variable> identifiers = new();
	private readonly Scope? parentScope;

	private TokenLocation? scopeResultLocation;
	private ExpressionResult? scopeResult;

	private bool shouldContinue;
	private bool shouldBreak;

	public Scope(Scope parent)
	{
		parentScope = parent;
	}

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
		CreateVariable(AggregateFunction.Identifier, new AggregateFunction().ToResult());
		CreateVariable(SubstringFunction.Identifier, new SubstringFunction().ToResult());
		CreateVariable(IndexOfFunction.Identifier, new IndexOfFunction().ToResult());
		CreateVariable(ContainsFunction.Identifier, new ContainsFunction().ToResult());
		CreateVariable(ContainsKeyFunction.Identifier, new ContainsKeyFunction().ToResult());
		CreateVariable(StartsWithFunction.Identifier, new StartsWithFunction().ToResult());
		CreateVariable(EndsWithFunction.Identifier, new EndsWithFunction().ToResult());
		CreateVariable(PrintFunction.Identifier, new PrintFunction().ToResult());
		CreateVariable(PrintlnFunction.Identifier, new PrintlnFunction().ToResult());
		CreateVariable(PrintlnPrettyFunction.Identifier, new PrintlnPrettyFunction().ToResult());
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
		CreateVariable(ZippedFunction.Identifier, new ZippedFunction().ToResult());

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
		CreateVariable(RegexMatchFunction.Identifier, new RegexMatchFunction().ToResult());
		CreateVariable(EnvFunction.Identifier, new EnvFunction().ToResult());

		// web functions
		CreateVariable(FetchFunction.Identifier, new FetchFunction().ToResult());
		CreateVariable(HttpServerFunction.Identifier, new HttpServerFunction().ToResult());
		CreateVariable(FileResponseFunction.Identifier, new FileResponseFunction().ToResult());

		CreateVariable("null", NullResult.Instance);

		CreateVariable("EOL", new StringResult(Environment.NewLine));

		CreateVariable("pi", new NumberResult(Math.PI));
		CreateVariable("e", new NumberResult(Math.E));
	}

	public void CreateVariable(string identifier, ExpressionResult initialValue, bool nullable = false)
	{
		_ = CreateVariable(new TokenLocation(), new Token(TokenType.Identifier, identifier),
			[initialValue.ResultType], initialValue,
			nullable);
	}

	public Variable CreateVariable(TokenLocation assignmentLocation, Token identifierToken,
		IReadOnlyList<ResultType> types, ExpressionResult initialValue, bool nullable = false)
	{
		if (Exists(identifierToken.Value, false))
		{
			throw new VariableAlreadyDeclaredException(identifierToken);
		}

		var variable = new Variable(identifierToken.Value, types, nullable);

		variable.Assign(assignmentLocation, initialValue);

		identifiers[identifierToken.Value] = variable;

		return variable;
	}

	public bool Exists(string identifier, bool includeParent)
	{
		if (includeParent)
		{
			return TryGetVariable(identifier, out _);
		}

		return identifiers.ContainsKey(identifier);
	}

	internal bool TryGetVariable(string identifier, [NotNullWhen(true)] out Variable? variable)
	{
		if (identifiers.TryGetValue(identifier, out variable))
		{
			return true;
		}

		if (parentScope != null)
		{
			return parentScope.TryGetVariable(identifier, out variable);
		}

		variable = null;
		return false;
	}

	public Variable GetVariable(Token identifierToken)
	{
		if (TryGetVariable(identifierToken.Value, out var variable))
		{
			return variable;
		}

		throw new UndefinedIdentifierException(identifierToken);
	}

	public void SetResult(TokenLocation location, ExpressionResult result)
	{
		scopeResultLocation = location;
		scopeResult = result;
	}

	public bool ShouldReturn()
	{
		return scopeResult is not null;
	}

	public bool TryGetResult([NotNullWhen(true)] out ExpressionResult? result,
		[NotNullWhen(true)] out TokenLocation? location)
	{
		location = scopeResultLocation;
		result = scopeResult;

		return result is not null;
	}

	public void SetContinue()
	{
		shouldContinue = true;
	}

	public void SetBreak()
	{
		shouldBreak = true;
	}

	public bool ShouldContinue()
	{
		return shouldContinue;
	}

	public bool ShouldBreak()
	{
		return shouldBreak;
	}

	public override string ToString()
	{
		return
			$"Scope: {(shouldBreak ? "BREAK " : "")}{(shouldContinue ? "CONTINUE " : "")}{(scopeResult is { } result ? "RESULT: " + result : "")}{{{string.Join(", ", identifiers.Select(kvp => kvp.Value.ToString()))}}}";
	}
}
