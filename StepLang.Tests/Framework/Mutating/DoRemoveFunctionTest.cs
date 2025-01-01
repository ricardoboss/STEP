using StepLang.Expressions.Results;
using StepLang.Framework.Mutating;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes;
using StepLang.Tokenizing;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Framework.Mutating;

public class DoRemoveFunctionTest
{
	[Theory]
	[ClassData(typeof(DoRemoveData))]
	public void TestDoRemove(ListResult list, ExpressionNode elementExpression, ListResult resultingList)
	{
		const string listIdentifier = "mylist";

		var interpreter = new Interpreter();
		interpreter.CurrentScope.CreateVariable(listIdentifier, list);
		var listVarExpression = new IdentifierExpressionNode(new Token(TokenType.Identifier, listIdentifier));
		var function = new DoRemoveFunction();

		var result = function.Invoke(new TokenLocation(), interpreter, [listVarExpression, elementExpression]);

		Assert.Equal(VoidResult.Instance, result);
		Assert.Equal(resultingList.Value.Count, list.Value.Count);
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
	private sealed class DoRemoveData : TheoryData<ListResult, ExpressionNode, ListResult>
	{
		public DoRemoveData()
		{
			Add(ListResult.Empty, LiteralExpressionNode.FromString("test"), ListResult.Empty);
			Add(ListResult.Empty, LiteralExpressionNode.FromInt32(1), ListResult.Empty);
			Add(ListResult.Empty, LiteralExpressionNode.FromBoolean(true), ListResult.Empty);
			Add(ListResult.Empty,
				new AddExpressionNode(new TokenLocation(), LiteralExpressionNode.FromInt32(1),
					LiteralExpressionNode.FromInt32(2)), ListResult.Empty);

			Add(ListResult.From(BoolResult.True), LiteralExpressionNode.FromBoolean(false),
				ListResult.From(BoolResult.True));

			Add(ListResult.From(BoolResult.False), LiteralExpressionNode.FromBoolean(false), ListResult.Empty);
		}
	}
}
