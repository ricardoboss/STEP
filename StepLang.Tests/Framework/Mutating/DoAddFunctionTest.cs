using StepLang.Expressions.Results;
using StepLang.Framework.Mutating;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Framework.Mutating;

public class DoAddFunctionTest
{
	[TestCaseSource(typeof(DoAddData))]
	public void TestDoAdd(ListResult list, ExpressionNode elementExpression)
	{
		const string listIdentifier = "mylist";

		var interpreter = new Interpreter();
		interpreter.CurrentScope.CreateVariable(listIdentifier, list);
		var listVarExpression = new IdentifierExpressionNode(new Token(TokenType.Identifier, listIdentifier));
		var function = new DoAddFunction();

		var previousCount = list.Value.Count;

		var result = function.Invoke(new TokenLocation(), interpreter, [listVarExpression, elementExpression]);

		Assert.That(result, Is.SameAs(VoidResult.Instance));
		Assert.That(list.Value.Count, Is.EqualTo(previousCount + 1));
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by NUnit")]
	private sealed class DoAddData : IEnumerable<TestCaseData>
	{
		public IEnumerator<TestCaseData> GetEnumerator()
		{
			yield return new TestCaseData(ListResult.Empty, LiteralExpressionNode.FromString("test"));
			yield return new TestCaseData(ListResult.Empty, LiteralExpressionNode.FromInt32(1));
			yield return new TestCaseData(ListResult.Empty, LiteralExpressionNode.FromBoolean(true));
			yield return new TestCaseData(ListResult.Empty,
					new AddExpressionNode(new Token(TokenType.PlusSymbol, "+"), LiteralExpressionNode.FromInt32(1),
							LiteralExpressionNode.FromInt32(2)));

			yield return new TestCaseData(ListResult.From(BoolResult.True), LiteralExpressionNode.FromBoolean(false));
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
