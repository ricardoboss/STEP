using StepLang.Expressions.Results;
using StepLang.Framework.Mutating;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Framework.Mutating;

public class DoRemoveFunctionTest
{
	[TestCaseSource(typeof(DoRemoveData))]
	public void TestDoRemove(ListResult list, ExpressionNode elementExpression, ListResult resultingList)
	{
		const string listIdentifier = "mylist";

		var interpreter = new Interpreter();
		interpreter.CurrentScope.CreateVariable(listIdentifier, list);
		var listVarExpression = new IdentifierExpressionNode(new Token(TokenType.Identifier, listIdentifier));
		var function = new DoRemoveFunction();

		var result = function.Invoke(new TokenLocation(), interpreter, [listVarExpression, elementExpression]);

		Assert.That(result, Is.SameAs(VoidResult.Instance));
		Assert.That(list.Value.Count, Is.EqualTo(resultingList.Value.Count));
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by NUnit")]
	private sealed class DoRemoveData : IEnumerable<TestCaseData>
	{
		public IEnumerator<TestCaseData> GetEnumerator()
		{
			yield return new TestCaseData(ListResult.Empty, LiteralExpressionNode.FromString("test"), ListResult.Empty);
			yield return new TestCaseData(ListResult.Empty, LiteralExpressionNode.FromInt32(1), ListResult.Empty);
			yield return new TestCaseData(ListResult.Empty, LiteralExpressionNode.FromBoolean(true), ListResult.Empty);
			yield return new TestCaseData(ListResult.Empty,
					new AddExpressionNode(new Token(TokenType.MinusSymbol, "-"), LiteralExpressionNode.FromInt32(1),
							LiteralExpressionNode.FromInt32(2)), ListResult.Empty);

			yield return new TestCaseData(ListResult.From(BoolResult.True), LiteralExpressionNode.FromBoolean(false),
					ListResult.From(BoolResult.True));

			yield return new TestCaseData(ListResult.From(BoolResult.False), LiteralExpressionNode.FromBoolean(false),
					ListResult.Empty);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
