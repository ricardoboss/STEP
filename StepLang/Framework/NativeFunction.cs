using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Parsing.Nodes.VariableDeclarations;
using StepLang.Tokenizing;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Framework;

public abstract class NativeFunction : FunctionDefinition
{
	/// <inheritdoc />
	[ExcludeFromCodeCoverage]
	protected override string DebugBodyString => "[native code]";

	protected static IReadOnlyList<ResultType> AnyType => Enum.GetValues<ResultType>();
	protected static IReadOnlyList<ResultType> AnyValueType =>
		Enum.GetValues<ResultType>().Except([ResultType.Void]).ToList();
	protected static IReadOnlyList<ResultType> OnlyNumber => [ResultType.Number];
	protected static IReadOnlyList<ResultType> NullableNumber => [ResultType.Null, ResultType.Number];
	protected static IReadOnlyList<ResultType> OnlyString => [ResultType.Str];
	protected static IReadOnlyList<ResultType> NullableString => [ResultType.Null, ResultType.Str];
	protected static IReadOnlyList<ResultType> OnlyList => [ResultType.List];
	protected static IReadOnlyList<ResultType> OnlyBool => [ResultType.Bool];
	protected static IReadOnlyList<ResultType> NullableBool => [ResultType.Null, ResultType.Bool];
	protected static IReadOnlyList<ResultType> OnlyMap => [ResultType.Map];
	protected static IReadOnlyList<ResultType> OnlyFunction => [ResultType.Function];

	protected void CheckArgumentCount(TokenLocation location, IReadOnlyList<ExpressionNode> arguments)
	{
		var expectedCount = Parameters.Count;
		if (arguments.Count != expectedCount)
		{
			throw new InvalidArgumentCountException(location, expectedCount, arguments.Count);
		}
	}

	protected static void CheckArgumentCount(TokenLocation location, IReadOnlyList<ExpressionNode> arguments,
		int minCount, int maxCount)
	{
		if (arguments.Count < minCount || arguments.Count > maxCount)
		{
			throw new InvalidArgumentCountException(location, minCount, arguments.Count, maxCount);
		}
	}

	public override IReadOnlyList<IVariableDeclarationNode> Parameters => NativeParameters
		.Select<NativeParameter, IVariableDeclarationNode>(p =>
		{
			var types = p.Types.Select(t => new Token(TokenType.TypeName, t.ToTypeName())).ToList();
			var nullable = p.Types.Any(t => t is ResultType.Null);
			var identifier = new Token(TokenType.Identifier, p.Identifier);

			if (p.DefaultValue is null)
			{
				if (!nullable)
				{
					return new VariableDeclarationNode(types, identifier);
				}

				return new NullableVariableDeclarationNode(types, new Token(TokenType.QuestionMarkSymbol, "?"),
					identifier);
			}

			if (!nullable)
			{
				return new VariableInitializationNode(new TokenLocation(), types, identifier, p.DefaultValue);
			}

			return new NullableVariableInitializationNode(new TokenLocation(), types,
				new Token(TokenType.QuestionMarkSymbol, "?"), identifier, p.DefaultValue);
		})
		.ToList();

	protected abstract IEnumerable<NativeParameter> NativeParameters { get; }

	protected record NativeParameter(
		IReadOnlyList<ResultType> Types,
		string Identifier,
		ExpressionNode? DefaultValue = null);
}
