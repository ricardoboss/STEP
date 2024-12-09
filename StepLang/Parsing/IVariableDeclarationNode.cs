using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public interface IVariableDeclarationNode : IEvaluatableNode<IVariableDeclarationEvaluator, Variable>
{
	IEnumerable<Token> Types { get; }

	Token Identifier { get; }

	bool HasValue { get; }
}

public static class VariableDeclarationNodeExtensions
{
	public static IEnumerable<ResultType> GetResultTypes(this IVariableDeclarationNode node)
	{
		return node.Types
			.Where(t => t.Type == TokenType.TypeName)
			.Select(t => ResultTypes.FromTypeName(t.Value));
	}

	public static bool HasResultType(this IVariableDeclarationNode node, ResultType type)
	{
		return node.GetResultTypes().Contains(type);
	}

	public static string ResultTypesToString(this IVariableDeclarationNode node)
	{
		return string.Join("|", node.GetResultTypes().Select(t => t.ToTypeName()));
	}
}
