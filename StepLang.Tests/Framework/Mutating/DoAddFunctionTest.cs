using StepLang.Expressions.Results;
using StepLang.Framework.Mutating;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Framework.Mutating;

public class DoAddFunctionTest
{
	[Theory]
	[ClassData(typeof(DoAddData))]
	public void TestDoAdd(ListResult list, ExpressionNode elementExpression)
	{
		const string listIdentifier = "mylist";

		var interpreter = new Interpreter();
		interpreter.CurrentScope.CreateVariable(listIdentifier, list);
		var listVarExpression = new IdentifierExpressionNode(new Token(TokenType.Identifier, listIdentifier));
		var function = new DoAddFunction();

		var previousCount = list.Value.Count;

		var result = function.Invoke(new TokenLocation(), interpreter, [listVarExpression, elementExpression]);

		Assert.Equal(VoidResult.Instance, result);
		Assert.Equal(previousCount + 1, list.Value.Count);
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
	private sealed class DoAddData : TheoryData<ListResult, ExpressionNode>
	{
		public DoAddData()
		{
			Add(ListResult.Empty, LiteralExpressionNode.FromString("test"));
			Add(ListResult.Empty, LiteralExpressionNode.FromInt32(1));
			Add(ListResult.Empty, LiteralExpressionNode.FromBoolean(true));
			Add(ListResult.Empty,
				new AddExpressionNode(new Token(TokenType.PlusSymbol, "+"), LiteralExpressionNode.FromInt32(1),
					LiteralExpressionNode.FromInt32(2)));

			Add(ListResult.From(BoolResult.True), LiteralExpressionNode.FromBoolean(false));
		}
	}
}
